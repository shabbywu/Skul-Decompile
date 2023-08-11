using System;
using Characters.Actions;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class BigBlood : Ability
{
	public class Instance : AbilityInstance<BigBlood>
	{
		private bool _canUse;

		public Instance(Character owner, BigBlood ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.onStartAction += OnStartAction;
			owner.status.onApplyBleed += AdditionalAttack;
		}

		private void OnStartAction(Characters.Actions.Action action)
		{
			if (action.type == Characters.Actions.Action.Type.Swap)
			{
				_canUse = true;
			}
			else if (_canUse && action.type == Characters.Actions.Action.Type.Skill)
			{
				_canUse = false;
				Attack();
			}
		}

		protected override void OnDetach()
		{
			owner.onStartAction -= OnStartAction;
			owner.status.onApplyBleed -= AdditionalAttack;
		}

		public void Attack()
		{
			((MonoBehaviour)owner).StartCoroutine(ability._operations.CRun(owner));
		}

		public void AdditionalAttack(Character giver, Character target)
		{
			//IL_0053: Unknown result type (might be due to invalid IL or missing references)
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			if (MMMaths.PercentChance(ability._additionalAttackChance) && !target.health.dead)
			{
				double @base = target.health.maximumHealth * (double)ability._additionalAttackHealthPercent / 100.0;
				Damage damage = new Damage(giver, @base, MMMaths.RandomPointWithinBounds(((Collider2D)target.collider).bounds), Damage.Attribute.Fixed, Damage.AttackType.Additional, Damage.MotionType.Item, 1.0, 0f, 0.0, 1.0, 1.0, canCritical: false, @null: false, 0.0, 0.0, 0);
				giver.Attack(target, ref damage);
				giver.GiveStatus(target, new CharacterStatus.ApplyInfo(CharacterStatus.Kind.Stun));
			}
		}
	}

	[SerializeField]
	[Range(0f, 100f)]
	private int _additionalAttackChance;

	[SerializeField]
	[Range(0f, 100f)]
	private int _additionalAttackHealthPercent;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
