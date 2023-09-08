using System;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class Glutton : Ability
{
	public sealed class Instance : AbilityInstance<Glutton>
	{
		private Characters.Shield.Instance _shieldInstance;

		public Instance(Character owner, Glutton ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onHealed += HandleOnHealed;
		}

		private void HandleOnHealed(double healed, double overHealed)
		{
			float num = (float)(overHealed * (double)ability._shieldRatio * 0.009999999776482582);
			if (_shieldInstance != null)
			{
				num = Mathf.Min((float)_shieldInstance.amount + num, (float)ability._maxShieldAmount);
				_shieldInstance.amount = (int)num;
			}
			else
			{
				num = (int)Mathf.Min(num, (float)ability._maxShieldAmount);
				_shieldInstance = owner.health.shield.Add(ability, num);
			}
		}

		protected override void OnDetach()
		{
			owner.health.onHealed -= HandleOnHealed;
			if (owner.health.shield.Remove(ability))
			{
				_shieldInstance = null;
			}
		}
	}

	[SerializeField]
	private int _maxShieldAmount;

	[SerializeField]
	[Range(0f, 100f)]
	private int _shieldRatio;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
