using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.Abilities;
using Characters.Abilities.Constraints;
using Characters.Movements;
using Characters.Projectiles;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Artifact : SimpleStatBonusKeyword
{
	[Serializable]
	private class Buff : IAbility, IAbilityInstance
	{
		[Constraint.Subcomponent]
		[SerializeField]
		private Constraint.Subcomponents _constraints;

		[SerializeField]
		[Header("4세트 효과")]
		private Sprite _icon;

		[SerializeField]
		private AttackDamage _attackDamage;

		[SerializeField]
		private CharacterTypeBoolArray _targetTypes;

		[SerializeField]
		private Collider2D _range;

		[SerializeField]
		private float _interval;

		[SerializeField]
		private Projectile _projectile;

		[Header("6세트 효과")]
		[Tooltip("cycle번째 마다 강화된 추가공격")]
		[SerializeField]
		private int _projectileCount;

		[SerializeField]
		private float _distance = 15f;

		[SerializeField]
		private int _cycle;

		[SerializeField]
		private CustomFloat _firingInterval;

		[SerializeField]
		private Projectile _enhancedProjectile;

		private NonAllocOverlapper _overlapper;

		private int _fireCount;

		private float _remainCooldownTime;

		private Character _owner;

		private CoroutineReference _coroutineReference;

		Character IAbilityInstance.owner => _owner;

		public IAbility ability => this;

		public float remainTime { get; set; }

		public bool attached => true;

		public Sprite icon
		{
			get
			{
				if (!(_remainCooldownTime > 0f))
				{
					return null;
				}
				return _icon;
			}
		}

		public float iconFillAmount => 1f - _remainCooldownTime / _interval;

		public bool iconFillInversed => true;

		public bool iconFillFlipped => true;

		public int iconStacks
		{
			get
			{
				if (!artifact.keyword.isMaxStep)
				{
					return 0;
				}
				return _fireCount;
			}
		}

		public bool expired => false;

		public float duration { get; set; }

		public int iconPriority => 0;

		public bool removeOnSwapWeapon => false;

		public Artifact artifact { get; set; }

		public void Attach()
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			_coroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)_owner, CStartAttackLoop());
		}

		public IAbilityInstance CreateInstance(Character owner)
		{
			//IL_000d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Expected O, but got Unknown
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			_owner = owner;
			_overlapper = new NonAllocOverlapper(128);
			((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
			return this;
		}

		public void Detach()
		{
			((CoroutineReference)(ref _coroutineReference)).Stop();
		}

		public void Initialize()
		{
		}

		public void UpdateTime(float deltaTime)
		{
		}

		public void Refresh()
		{
		}

		private IEnumerator CStartAttackLoop()
		{
			yield return null;
			while (true)
			{
				_remainCooldownTime = _interval;
				while (_remainCooldownTime > 0f)
				{
					if (_constraints.Pass())
					{
						_remainCooldownTime -= ((ChronometerBase)_owner.chronometer.master).deltaTime;
					}
					yield return null;
				}
				if (artifact.keyword.isMaxStep)
				{
					_fireCount++;
				}
				while (true)
				{
					bool flag = _fireCount == _cycle && artifact.keyword.isMaxStep;
					if (Fire((!flag) ? 1 : _projectileCount))
					{
						break;
					}
					yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)_owner.chronometer.master, 0.3f);
				}
			}
		}

		private bool Fire(int count)
		{
			UsingCollider val = default(UsingCollider);
			((UsingCollider)(ref val))._002Ector(_range, true);
			try
			{
				_overlapper.OverlapCollider(_range);
			}
			finally
			{
				((IDisposable)(UsingCollider)(ref val)).Dispose();
			}
			IEnumerable<Collider2D> source = ((IEnumerable<Collider2D>)_overlapper.results).Where(delegate(Collider2D result)
			{
				Target component = ((Component)result).GetComponent<Target>();
				if ((Object)(object)component == (Object)null)
				{
					return false;
				}
				Character character = component.character;
				if ((Object)(object)character == (Object)null)
				{
					return false;
				}
				return !character.health.dead && ((EnumArray<Character.Type, bool>)_targetTypes)[character.type];
			});
			if (source.Count() == 0)
			{
				return false;
			}
			Collider2D[] array = source.ToArray();
			ExtensionMethods.PseudoShuffle<Collider2D>((IList<Collider2D>)array);
			((MonoBehaviour)_owner).StartCoroutine(CFireWithDelay(count, array));
			return true;
		}

		private IEnumerator CFireWithDelay(int count, Collider2D[] targets)
		{
			int remain = count;
			int index = 0;
			while (remain > 0)
			{
				int num = (MMMaths.RandomBool() ? Random.Range(45, 55) : Random.Range(125, 135));
				Vector2 val = GetAdditionalVector(Vector2.right, num) * _distance;
				Character character = ((Component)targets[index]).GetComponent<Target>().character;
				Vector2 val2 = Vector2.op_Implicit(((Component)character).transform.position) + val;
				Collider2D collider = character.movement.controller.collisionState.lastStandingCollider;
				if (character.movement.configs.Count > 0 && (character.movement.config.type == Movement.Config.Type.Static || character.movement.config.type == Movement.Config.Type.Flying || character.movement.config.type == Movement.Config.Type.AcceleratingFlying))
				{
					character.movement.TryGetClosestBelowCollider(out collider, Layers.footholdMask);
				}
				if ((Object)(object)collider != (Object)null)
				{
					float x = ((Component)character).transform.position.x;
					Bounds bounds = collider.bounds;
					val2 = new Vector2(x, ((Bounds)(ref bounds)).max.y) + val;
				}
				float direction = num + 180;
				Projectile projectile = _projectile;
				if (artifact.keyword.isMaxStep && _fireCount == _cycle && remain == 1)
				{
					projectile = _enhancedProjectile;
					_fireCount = 0;
				}
				((Component)projectile.reusable.Spawn(Vector2.op_Implicit(val2), true)).GetComponent<Projectile>().Fire(_owner, _attackDamage.amount, direction);
				index = (index + 1) % targets.Length;
				remain--;
				yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)character.chronometer.master, _firingInterval.value);
			}
		}

		private Vector2 GetAdditionalVector(Vector2 vec, float angle)
		{
			//IL_0000: Unknown result type (might be due to invalid IL or missing references)
			//IL_0006: Unknown result type (might be due to invalid IL or missing references)
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			float num = Mathf.Atan2(vec.y, vec.x) + angle * ((float)Math.PI / 180f);
			return new Vector2(Mathf.Cos(num), Mathf.Sin(num));
		}
	}

	private const float _checkInterval = 0.3f;

	[SerializeField]
	private double[] _statBonusByStep;

	[SerializeField]
	[Header("Buff")]
	private Buff _buff;

	protected override double[] statBonusByStep => _statBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.PercentPoint;

	protected override Stat.Kind statKind => Stat.Kind.MagicAttackDamage;

	protected override void Initialize()
	{
		base.Initialize();
		_buff.artifact = this;
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
		if (keyword.step >= 2)
		{
			if (!base.character.ability.Contains(_buff))
			{
				base.character.ability.Add(_buff);
			}
		}
		else
		{
			base.character.ability.Remove(_buff);
		}
	}

	public override void Detach()
	{
		base.Detach();
		base.character.ability.Remove(_buff);
	}
}
