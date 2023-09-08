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
		categoryCounts[_category1]++;
		categoryCounts[_category2]++;
		Weapon[] weapons = base.owner.playerComponents.inventory.weapon.weapons;
		foreach (Weapon weapon in weapons)
		{
			if (!((Object)(object)weapon == (Object)null))
			{
				categoryCounts[weapon.category]--;
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
			for (int j = 0; j < categoryCounts.Keys.Count; j++)
			{
				if (categoryCounts.Array[j] != 0)
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
		return this.GetAutoName();
	}
}
