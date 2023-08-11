using System;
using Characters.Operations;
using FX.BoundsAttackVisualEffect;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class AdditionalHitByTargetStatus : Ability
{
	public enum DamageAmountType
	{
		Constant,
		PercentOfOriginalDamage
	}

	public class Instance : AbilityInstance<AdditionalHitByTargetStatus>
	{
		private float _remainCooldownTime;

		private int _remainCount;

		private bool _passPrecondition;

		internal Instance(Character owner, AdditionalHitByTargetStatus ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_remainCount = ability._applyCount;
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)HandleOnGiveDamage);
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		}

		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			_passPrecondition = false;
			if ((Object)(object)target.character.status == (Object)null || !target.character.status.IsApplying(ability._targetStatusFilter))
			{
				return false;
			}
			_passPrecondition = true;
			return false;
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)HandleOnGiveDamage);
		}

		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
			//IL_0124: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_0164: Unknown result type (might be due to invalid IL or missing references)
			if (_passPrecondition && !(_remainCooldownTime > 0f) && !target.character.health.dead && ((Component)target.transform).gameObject.activeSelf && (!ability._needCritical || tookDamage.critical) && ((EnumArray<Damage.MotionType, bool>)ability._attackTypes)[tookDamage.motionType] && ((EnumArray<Damage.AttackType, bool>)ability._damageTypes)[tookDamage.attackType] && !(tookDamage.amount < (double)ability._minDamage))
			{
				if ((Object)(object)ability._targetPoint != (Object)null)
				{
					ability._targetPoint.position = target.transform.position;
				}
				double baseDamage = ((ability._damageAmountType != 0) ? ((double)ability._additionalDamageAmount * 0.01 * originalDamage.amount) : ((double)ability._additionalDamageAmount));
				Damage damage = owner.stat.GetDamage(baseDamage, MMMaths.RandomPointWithinBounds(target.collider.bounds), ability._additionalHit);
				owner.Attack(target, ref damage);
				ability._hitEffect.Spawn(owner, target.collider.bounds, in damage, target);
				_remainCooldownTime = ability._cooldownTime;
				((MonoBehaviour)owner).StartCoroutine(ability._targetOperationInfo.CRun(owner, target.character));
				_remainCount--;
				if (_remainCount == 0)
				{
					owner.ability.Remove(this);
				}
			}
		}
	}

	[SerializeField]
	private Transform _targetPoint;

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private int _applyCount;

	[SerializeField]
	private DamageAmountType _damageAmountType;

	[SerializeField]
	private int _additionalDamageAmount;

	[SerializeField]
	private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	private CharacterStatusKindBoolArray _targetStatusFilter;

	[SerializeField]
	private bool _needCritical;

	[SerializeField]
	private float _minDamage = 1f;

	[SerializeField]
	private MotionTypeBoolArray _attackTypes;

	[SerializeField]
	private AttackTypeBoolArray _damageTypes;

	[SerializeField]
	[BoundsAttackVisualEffect.Subcomponent]
	private BoundsAttackVisualEffect.Subcomponents _hitEffect;

	[Subcomponent(typeof(TargetedOperationInfo))]
	[SerializeField]
	private TargetedOperationInfo.Subcomponents _targetOperationInfo;

	public override void Initialize()
	{
		base.Initialize();
		_targetOperationInfo.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
