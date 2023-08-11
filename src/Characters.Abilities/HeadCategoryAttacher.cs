using Characters.Gear.Weapons;
using UnityEngine;

namespace Characters.Abilities;

public class HeadCategoryAttacher : AbilityAttacher
{
	[SerializeField]
	private Weapon.Category _category1;

	[SerializeField]
	private Weapon.Category _category2;

	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _abilityComponent;

	private bool _attached;

	public override void OnIntialize()
	{
		_abilityComponent.Initialize();
	}

	public override void StartAttach()
	{
		base.owner.playerComponents.inventory.weapon.onChanged += Check;
		Check(null, null);
	}

	public override void StopAttach()
	{
		if (!((Object)(object)base.owner == (Object)null))
		{
			base.owner.playerComponents.inventory.weapon.onChanged -= Check;
			base.owner.ability.Remove(_abilityComponent.ability);
		}
	}

	private void Check(Weapon old, Weapon @new)
	{
		EnumArray<Weapon.Category, int> categoryCounts = new EnumArray<Weapon.Category, int>();
		EnumArray<Weapon.Category, int> obj = categoryCounts;
		Weapon.Category category = _category1;
		int num = obj[category];
		obj[category] = num + 1;
		EnumArray<Weapon.Category, int> obj2 = categoryCounts;
		category = _category2;
		num = obj2[category];
		obj2[category] = num + 1;
		Weapon[] weapons = base.owner.playerComponents.inventory.weapon.weapons;
		foreach (Weapon weapon in weapons)
		{
			if (!((Object)(object)weapon == (Object)null))
			{
				EnumArray<Weapon.Category, int> obj3 = categoryCounts;
				category = weapon.category;
				int num2 = obj3[category];
				obj3[category] = num2 - 1;
			}
		}
		if (CanAttach())
		{
			Attach();
		}
		else
		{
			Detach();
		}
		bool CanAttach()
		{
			for (int i = 0; i < categoryCounts.Keys.Count; i++)
			{
				if (categoryCounts.Array[i] != 0)
				{
					return false;
				}
			}
			return true;
		}
	}

	private void Attach()
	{
		if (!_attached)
		{
			_attached = true;
			base.owner.ability.Add(_abilityComponent.ability);
		}
	}

	private void Detach()
	{
		if (_attached)
		{
			_attached = false;
			base.owner.ability.Remove(_abilityComponent.ability);
		}
	}

	public override string ToString()
	{
		return ExtensionMethods.GetAutoName((object)this);
	}
}
