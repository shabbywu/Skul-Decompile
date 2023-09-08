using System;
using Characters.Operations;
using FX;
using FX.BoundsAttackVisualEffect;
using Singletons;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Abilities;

[Serializable]
public class AdditionalHit : Ability
{
	public class Instance : AbilityInstance<AdditionalHit>
	{
		private float _remainCooldownTime;

		private int _remainCount;

		internal Instance(Character owner, AdditionalHit ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			_remainCount = ability._applyCount;
			if (ability._applyCount == 0)
			{
				_remainCount = int.MaxValue;
			}
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainCooldownTime -= deltaTime;
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnOwnerGaveDamage));
		}

		private void OnOwnerGaveDamage(ITarget target, in Damage originalDamage, in Damage tookDamage, double damageDealt)
		{
			//IL_0126: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_01af: Unknown result type (might be due to invalid IL or missing references)
			//IL_0090: Unknown result type (might be due to invalid IL or missing references)
			//IL_0095: Unknown result type (might be due to invalid IL or missing references)
			//IL_0098: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a8: Unknown result type (might be due to invalid IL or missing references)
			//IL_00ab: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0103: Unknown result type (might be due to invalid IL or missing references)
			if (!(_remainCooldownTime > 0f) && !target.character.health.dead && ((Component)target.transform).gameObject.activeSelf && (!ability._needCritical || tookDamage.critical) && ability._attackTypes[tookDamage.motionType] && ability._damageTypes[tookDamage.attackType])
			{
				if ((Object)(object)ability._targetPoint != (Object)null)
				{
					Bounds bounds = target.collider.bounds;
					Vector3 center = ((Bounds)(ref bounds)).center;
					bounds = target.collider.bounds;
					Vector3 size = ((Bounds)(ref bounds)).size;
					size.x *= ability._positionInfo.pivotValue.x;
					size.y *= ability._positionInfo.pivotValue.y;
					Vector3 position = center + size;
					ability._targetPoint.position = position;
				}
				Damage damage = owner.stat.GetDamage(ability._additionalDamageAmount, MMMaths.RandomPointWithinBounds(target.collider.bounds), ability._additionalHit);
				PersistentSingleton<SoundManager>.Instance.PlaySound(ability._attackSoundInfo, target.transform.position);
				((MonoBehaviour)owner).StartCoroutine(ability._targetOperationInfo.CRun(owner, target.character));
				owner.Attack(target, ref damage);
				ability._hitEffect.Spawn(owner, target.collider.bounds, in damage, target);
				_remainCooldownTime = ability._cooldownTime;
				_remainCount--;
				if (_remainCount == 0)
				{
					owner.ability.Remove(this);
				}
			}
		}
	}

	[SerializeField]
	private float _cooldownTime;

	[SerializeField]
	private int _applyCount;

	[SerializeField]
	private float _additionalDamageAmount;

	[SerializeField]
	private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	private bool _needCritical;

	[SerializeField]
	private MotionTypeBoolArray _attackTypes;

	[SerializeField]
	private AttackTypeBoolArray _damageTypes;

	[SerializeField]
	private PositionInfo _positionInfo;

	[SerializeField]
	private Transform _targetPoint;

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _targetOperationInfo;

	[SerializeField]
	[BoundsAttackVisualEffect.Subcomponent]
	private BoundsAttackVisualEffect.Subcomponents _hitEffect;

	[SerializeField]
	private SoundInfo _attackSoundInfo;

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
