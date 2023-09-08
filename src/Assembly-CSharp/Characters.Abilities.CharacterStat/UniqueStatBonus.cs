using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public class UniqueStatBonus : Ability
{
	public class Instance : AbilityInstance<UniqueStatBonus>
	{
		public bool attachedStatBonus;

		public Instance(Character owner, UniqueStatBonus ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			if (keys.ContainsKey(ability._key))
			{
				if (keys[ability._key].Count == 0)
				{
					AttachStatBonus();
				}
				keys[ability._key].Add(this);
			}
			else
			{
				keys.Add(ability._key, new List<Instance> { this });
				AttachStatBonus();
			}
		}

		protected override void OnDetach()
		{
			if (!keys.ContainsKey(ability._key))
			{
				owner.stat.DetachValues(ability._stat);
				return;
			}
			List<Instance> list = keys[ability._key];
			list.Remove(this);
			owner.stat.DetachValues(ability._stat);
			attachedStatBonus = false;
			if (list.Count((Instance key) => key.attachedStatBonus) <= 0)
			{
				if (list.Count > 0)
				{
					list[0].AttachStatBonus();
				}
				else
				{
					keys.Remove(ability._key);
				}
			}
		}

		public void AttachStatBonus()
		{
			owner.stat.AttachOrUpdateValues(ability._stat);
			attachedStatBonus = true;
		}
	}

	[SerializeField]
	private string _key;

	[SerializeField]
	private Stat.Values _stat;

	private static readonly IDictionary<string, List<Instance>> keys = new Dictionary<string, List<Instance>>();

	public UniqueStatBonus()
	{
	}

	public UniqueStatBonus(Stat.Values stat)
	{
		_stat = stat;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
