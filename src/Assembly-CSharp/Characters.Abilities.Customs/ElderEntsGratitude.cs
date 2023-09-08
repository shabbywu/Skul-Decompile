using System;
using Characters.Gear.Items;
using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class ElderEntsGratitude : Ability
{
	public class Instance : AbilityInstance<ElderEntsGratitude>
	{
		private Characters.Shield.Instance _shieldInstance;

		public override int iconStacks => (int)_shieldInstance.amount;

		public Instance(Character owner, ElderEntsGratitude ability)
			: base(owner, ability)
		{
		}

		public override void Refresh()
		{
			base.Refresh();
			_shieldInstance.amount = ability.component.stack;
		}

		private void OnShieldBroke()
		{
			ChangeItem();
			owner.ability.Remove(this);
		}

		protected override void OnAttach()
		{
			_shieldInstance = owner.health.shield.Add(ability, ability.component.savedShieldAmount, OnShieldBroke);
			owner.stat.AttachValues(ability._stat);
		}

		protected override void OnDetach()
		{
			owner.stat.DetachValues(ability._stat);
			if (_shieldInstance != null && _shieldInstance.amount > 0.0)
			{
				ability.component.savedShieldAmount = (float)_shieldInstance.amount;
			}
			if (owner.health.shield.Remove(ability))
			{
				_shieldInstance = null;
			}
		}

		private void ChangeItem()
		{
			if (owner.playerComponents != null)
			{
				ability._operationsOnChange.Run(owner);
				owner.playerComponents.inventory.item.Change(ability._elderEntsGratitudeItem, ability._toChangeItemOnShieldBroken);
			}
		}

		public float GetShieldAmount()
		{
			return (float)_shieldInstance.amount;
		}

		public void SetShieldAmount(float amount)
		{
			_shieldInstance.amount = amount;
		}
	}

	[SerializeField]
	private Item _elderEntsGratitudeItem;

	[SerializeField]
	private Item _toChangeItemOnShieldBroken;

	[SerializeField]
	[Space]
	private float _amount;

	[SerializeField]
	private Stat.Values _stat;

	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _operationsOnChange;

	private Instance _instance;

	public ElderEntsGratitudeComponent component { get; set; }

	public float amount => _amount;

	public override void Initialize()
	{
		base.Initialize();
		_operationsOnChange.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return _instance = new Instance(owner, this);
	}

	public float GetShieldAmount()
	{
		if (_instance == null)
		{
			return 0f;
		}
		return _instance.GetShieldAmount();
	}

	public void SetShieldAmount(float amount)
	{
		if (_instance != null)
		{
			_instance.SetShieldAmount(amount);
		}
	}
}
