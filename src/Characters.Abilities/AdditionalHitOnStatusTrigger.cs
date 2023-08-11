using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class AdditionalHitOnStatusTrigger : Ability
{
	public class Instance : AbilityInstance<AdditionalHitOnStatusTrigger>
	{
		private CharacterStatus.OnTimeDelegate _handle;

		public Instance(Character owner, AdditionalHitOnStatusTrigger ability)
			: base(owner, ability)
		{
			_handle = HandleOnTime;
		}

		protected override void OnAttach()
		{
			foreach (CharacterStatus.Kind value in Enum.GetValues(typeof(CharacterStatus.Kind)))
			{
				if (((EnumArray<CharacterStatus.Kind, bool>)ability._statuses)[value])
				{
					owner.status.Register(value, ability._timing, _handle);
				}
			}
		}

		protected override void OnDetach()
		{
			foreach (CharacterStatus.Kind value in Enum.GetValues(typeof(CharacterStatus.Kind)))
			{
				if (((EnumArray<CharacterStatus.Kind, bool>)ability._statuses)[value])
				{
					owner.status.Unregister(value, ability._timing, _handle);
				}
			}
		}

		private void HandleOnTime(Character attacker, Character target)
		{
			//IL_0024: Unknown result type (might be due to invalid IL or missing references)
			//IL_006e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0073: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)ability._targetPoint != (Object)null)
			{
				ability._targetPoint.position = ((Component)target).transform.position;
			}
			if (ability._adaptiveForce)
			{
				ability._additionalHit.ChangeAdaptiveDamageAttribute(attacker);
			}
			Damage damage = owner.stat.GetDamage(ability._additionalDamageAmount.value, MMMaths.RandomPointWithinBounds(((Collider2D)target.collider).bounds), ability._additionalHit);
			((MonoBehaviour)owner).StartCoroutine(ability._targetOperationInfo.CRun(owner, target));
			owner.Attack(target, ref damage);
		}
	}

	[SerializeField]
	private CharacterStatus.Timing _timing;

	[SerializeField]
	private CharacterStatusKindBoolArray _statuses;

	[SerializeField]
	private CustomFloat _additionalDamageAmount;

	[SerializeField]
	private bool _adaptiveForce;

	[SerializeField]
	private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	private Transform _targetPoint;

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
