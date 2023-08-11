using System;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class TimeBombGiver : Ability
{
	public sealed class Instance : AbilityInstance<TimeBombGiver>
	{
		private float _remainExplodeTime;

		private float _acc = 0.3f;

		private float _speed;

		private float _alpha;

		private bool _running;

		private List<TimeBomb.Instance> _bombs;

		public override float iconFillAmount => _remainExplodeTime / ability._explodeTime;

		public Instance(Character owner, TimeBombGiver ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_bombs = new List<TimeBomb.Instance>(128);
			_remainExplodeTime = ability._explodeTime;
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)HandleOnGiveDamage);
			_alpha = 0f;
		}

		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			//IL_00af: Unknown result type (might be due to invalid IL or missing references)
			Character character = target.character;
			if ((Object)(object)character == (Object)null)
			{
				return false;
			}
			if (character.type == Character.Type.Dummy || character.type == Character.Type.Trap || character.type == Character.Type.Player || character.type == Character.Type.PlayerMinion)
			{
				return false;
			}
			if (character.ability.GetInstance<TimeBomb>() != null)
			{
				return false;
			}
			if (damage.key.Equals(ability._explodeAttackKey, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			if (_running)
			{
				return false;
			}
			TimeBomb.Instance item = (TimeBomb.Instance)character.ability.Add(ability._timeBomb.ability);
			_bombs.Add(item);
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._attackSoundInfo, target.transform.position);
			return false;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainExplodeTime -= deltaTime;
			float num = _acc;
			if ((double)_remainExplodeTime <= 2.5)
			{
				num *= 20f;
			}
			_speed += deltaTime * num;
			_alpha += deltaTime * _speed;
			for (int num2 = _bombs.Count - 1; num2 >= 0; num2--)
			{
				if (_bombs[num2] == null)
				{
					_bombs.RemoveAt(num2);
				}
			}
			foreach (TimeBomb.Instance bomb in _bombs)
			{
				bomb.UpdateEffect(_alpha % 2f);
			}
			if (!(_remainExplodeTime <= 0f))
			{
				return;
			}
			_speed = 0f;
			_alpha = 0f;
			_remainExplodeTime = ability._explodeTime;
			_running = true;
			foreach (TimeBomb.Instance bomb2 in _bombs)
			{
				if (bomb2 != null && !((Object)(object)bomb2.owner == (Object)null) && !bomb2.owner.health.dead)
				{
					bomb2.Explode();
				}
			}
			_running = false;
			_bombs.Clear();
		}

		protected override void OnDetach()
		{
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)HandleOnGiveDamage);
			foreach (TimeBomb.Instance bomb in _bombs)
			{
				if (bomb != null && !((Object)(object)bomb.owner == (Object)null) && !bomb.owner.health.dead)
				{
					bomb.owner.ability.Remove(bomb);
				}
			}
			_bombs = null;
		}
	}

	[SerializeField]
	private string _explodeAttackKey;

	[SerializeField]
	private float _explodeTime;

	[SerializeField]
	private TimeBombComponent _timeBomb;

	[Header("Explode")]
	[SerializeField]
	private CustomFloat _damageAmount;

	[SerializeField]
	private HitInfo _hitInfo = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	private PositionInfo _positionInfo;

	[SerializeField]
	private Transform _targetPoint;

	[Subcomponent(typeof(TargetedOperationInfo))]
	[SerializeField]
	private TargetedOperationInfo.Subcomponents _targetOperationInfo;

	[SerializeField]
	private EffectInfo _hitEffect;

	[SerializeField]
	private SoundInfo _attackSoundInfo;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _bombOperations;

	private Character _owner;

	public override void Initialize()
	{
		base.Initialize();
		_timeBomb.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		_owner = owner;
		return new Instance(owner, this);
	}

	public void Attack(Character target)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0078: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_targetPoint != (Object)null)
		{
			Bounds bounds = ((Collider2D)target.collider).bounds;
			Vector3 center = ((Bounds)(ref bounds)).center;
			bounds = ((Collider2D)target.collider).bounds;
			Vector3 size = ((Bounds)(ref bounds)).size;
			size.x *= _positionInfo.pivotValue.x;
			size.y *= _positionInfo.pivotValue.y;
			Vector3 position = center + size;
			_targetPoint.position = position;
		}
		((MonoBehaviour)_owner).StartCoroutine(_bombOperations.CRun(_owner));
	}
}
