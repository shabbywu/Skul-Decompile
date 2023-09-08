using System;
using System.Collections.Generic;
using Characters.Operations;
using FX;
using FX.BoundsAttackVisualEffect;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class CrownOfThorns : Ability
{
	public sealed class Instance : AbilityInstance<CrownOfThorns>
	{
		private float _remainRefreshTime;

		private float _remainCooldownTime;

		private float _multiplier = 1f;

		private int _stack;

		private List<Target> _targets;

		public override Sprite icon
		{
			get
			{
				if (!(_remainRefreshTime > 0f))
				{
					return null;
				}
				return base.icon;
			}
		}

		public override int iconStacks => _stack;

		public override float iconFillAmount => _remainRefreshTime / ability._refreshCooldownTime;

		internal Instance(Character owner, CrownOfThorns ability)
			: base(owner, ability)
		{
			_targets = new List<Target>(128);
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
			_stack = 0;
		}

		public override void UpdateTime(float deltaTime)
		{
			_remainRefreshTime -= deltaTime;
			_remainCooldownTime -= deltaTime;
			if (_remainRefreshTime <= 0f)
			{
				_stack = 0;
				UpdateStack();
			}
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		}

		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			//IL_0092: Unknown result type (might be due to invalid IL or missing references)
			//IL_0236: Unknown result type (might be due to invalid IL or missing references)
			//IL_023b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0277: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00cd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ee: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
			//IL_00fb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_010a: Unknown result type (might be due to invalid IL or missing references)
			//IL_010f: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_01a9: Unknown result type (might be due to invalid IL or missing references)
			if (_remainCooldownTime > 0f || target.character.health.dead || !((Component)target.transform).gameObject.activeSelf || (ability._needCritical && !tookDamage.critical) || !ability._attackTypes[tookDamage.motionType] || !ability._damageTypes[tookDamage.attackType])
			{
				return;
			}
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._attackSoundInfo, ((Component)owner).transform.position);
			if (ability._enhanced)
			{
				TargetFinder.FindTargetInRange(Vector2.op_Implicit(target.transform.position), ability._range, LayerMask.op_Implicit(1024), _targets);
				Transform targetPoint = ability._targetPoint;
				Bounds bounds = target.collider.bounds;
				targetPoint.position = ((Bounds)(ref bounds)).center + MMMaths.Vector2ToVector3(Random.insideUnitCircle * 0.3f);
				foreach (Target target2 in _targets)
				{
					if ((Object)(object)owner == (Object)null)
					{
						return;
					}
					if (!((Object)(object)target2.character == (Object)null) && !target2.character.health.dead && !((Object)(object)target2.character == (Object)(object)owner))
					{
						float num = ability._additionalDamageAmount * _multiplier;
						Damage damage = owner.stat.GetDamage(num, MMMaths.RandomPointWithinBounds(target2.collider.bounds), ability._additionalHit);
						owner.Attack(target2, ref damage);
						((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(ability._onHit.CRun(target2.character));
					}
				}
			}
			else
			{
				float num2 = ability._additionalDamageAmount * _multiplier;
				Damage damage2 = owner.stat.GetDamage(num2, MMMaths.RandomPointWithinBounds(target.collider.bounds), ability._additionalHit);
				owner.Attack(target, ref damage2);
				ability._hitEffect.Spawn(owner, target.collider.bounds, in damage2, target);
				((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(ability._onHit.CRun(target.character));
			}
			if (_stack < ability._maxStack)
			{
				_stack++;
				UpdateStack();
			}
			_remainCooldownTime = ability._cooldownTime;
			_remainRefreshTime = ability._refreshCooldownTime;
		}

		private void UpdateStack()
		{
			_multiplier = 1f + ability._mutiplierPerStack * (float)_stack;
		}
	}

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _onHit;

	[Header("강화")]
	[SerializeField]
	private float _range;

	[SerializeField]
	private bool _enhanced;

	[Header("연타 스택")]
	[SerializeField]
	private int _maxStack;

	[SerializeField]
	private float _mutiplierPerStack;

	[SerializeField]
	private float _refreshCooldownTime;

	[Header("추가 데미지")]
	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private float _additionalDamageAmount;

	[SerializeField]
	private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	private bool _needCritical;

	[SerializeField]
	private Transform _targetPoint;

	[SerializeField]
	private MotionTypeBoolArray _attackTypes;

	[SerializeField]
	private AttackTypeBoolArray _damageTypes;

	[SerializeField]
	[BoundsAttackVisualEffect.Subcomponent]
	private BoundsAttackVisualEffect.Subcomponents _hitEffect;

	[SerializeField]
	private SoundInfo _attackSoundInfo;

	public override void Initialize()
	{
		base.Initialize();
		_onHit.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
