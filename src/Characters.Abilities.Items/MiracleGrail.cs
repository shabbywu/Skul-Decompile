using System;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Items;

[Serializable]
public sealed class MiracleGrail : Ability
{
	public sealed class Instance : AbilityInstance<MiracleGrail>
	{
		private int _stack;

		public Instance(Character owner, MiracleGrail ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.status.onApplyBleed += HandleOnApplyBleed;
		}

		private void HandleOnApplyBleed(Character attacker, Character target)
		{
			_stack++;
			if (_stack + 1 >= ability._cycle)
			{
				owner.onGiveDamage.Add(int.MaxValue, HandleOnGiveDamage);
				_stack = -1;
			}
		}

		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			//IL_0026: Unknown result type (might be due to invalid IL or missing references)
			//IL_002b: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if (!damage.key.Equals(CharacterStatus.AttackKeyBleed, StringComparison.OrdinalIgnoreCase))
			{
				return false;
			}
			Transform targetPoint = ability._targetPoint;
			Bounds bounds = target.collider.bounds;
			targetPoint.position = ((Bounds)(ref bounds)).center;
			((MonoBehaviour)owner).StartCoroutine(ability._targetOperationInfo.CRun(owner, target.character));
			damage.percentMultiplier *= ability._multiplier;
			owner.onGiveDamage.Remove(HandleOnGiveDamage);
			return false;
		}

		protected override void OnDetach()
		{
			owner.status.onApplyBleed -= HandleOnApplyBleed;
			owner.onGiveDamage.Remove(HandleOnGiveDamage);
		}
	}

	[SerializeField]
	private int _cycle;

	[Header("Percent Point")]
	[SerializeField]
	private float _multiplier;

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
