using System;
using Characters.Operations;
using FX;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class AdditionalHitToStatusTaker : Ability
{
	public class Instance : AbilityInstance<AdditionalHitToStatusTaker>
	{
		public Instance(Character owner, AdditionalHitToStatusTaker ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Combine(character.onGaveStatus, new Character.OnGaveStatusDelegate(OnGaveStatus));
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveStatus = (Character.OnGaveStatusDelegate)Delegate.Remove(character.onGaveStatus, new Character.OnGaveStatusDelegate(OnGaveStatus));
		}

		private void OnGaveStatus(Character target, CharacterStatus.ApplyInfo applyInfo, bool result)
		{
			//IL_0067: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0040: Unknown result type (might be due to invalid IL or missing references)
			if (result && ((EnumArray<CharacterStatus.Kind, bool>)ability._statuses)[applyInfo.kind])
			{
				if ((Object)(object)ability._targetPoint != (Object)null)
				{
					ability._targetPoint.position = ((Component)target).transform.position;
				}
				Damage damage = owner.stat.GetDamage(ability._additionalDamageAmount, MMMaths.RandomPointWithinBounds(((Collider2D)target.collider).bounds), ability._additionalHit);
				((MonoBehaviour)owner).StartCoroutine(ability._targetOperationInfo.CRun(owner, target));
				owner.Attack(target, ref damage);
			}
		}
	}

	[SerializeField]
	private CharacterStatusKindBoolArray _statuses;

	[SerializeField]
	private float _additionalDamageAmount;

	[SerializeField]
	private HitInfo _additionalHit = new HitInfo(Damage.AttackType.Additional);

	[SerializeField]
	private Transform _targetPoint;

	[Subcomponent(typeof(TargetedOperationInfo))]
	[SerializeField]
	private TargetedOperationInfo.Subcomponents _targetOperationInfo;

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
