using System;
using Characters.Abilities.Constraints;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters.Abilities;

[Serializable]
public class PeriodicHeal : Ability
{
	public class Instance : AbilityInstance<PeriodicHeal>
	{
		private float _remainPeriod;

		public override float iconFillAmount => 1f - _remainPeriod / ability._period;

		public Instance(Character owner, PeriodicHeal ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
		}

		protected override void OnDetach()
		{
		}

		public override void UpdateTime(float deltaTime)
		{
			if (!ability._constraints.Pass())
			{
				return;
			}
			base.UpdateTime(deltaTime);
			_remainPeriod -= deltaTime;
			if (_remainPeriod <= 0f)
			{
				_remainPeriod += ability._period;
				float num = (float)ability._healPercent * 0.01f;
				if (num > 0f)
				{
					owner.health.PercentHeal(num);
				}
				num = ability._healConstant.value;
				if (num > 0f)
				{
					owner.health.Heal(num);
				}
			}
		}
	}

	[SerializeField]
	private float _period;

	[FormerlySerializedAs("_amount")]
	[SerializeField]
	private int _healPercent;

	[SerializeField]
	private CustomFloat _healConstant;

	[Constraint.Subcomponent]
	[SerializeField]
	[Space]
	private Constraint.Subcomponents _constraints;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
