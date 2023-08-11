using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class AddAirJumpCount : Ability
{
	public class Instance : AbilityInstance<AddAirJumpCount>
	{
		public Instance(Character owner, AddAirJumpCount ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.movement.airJumpCount.Add((object)this, ability._count);
		}

		protected override void OnDetach()
		{
			((Sum<int>)(object)owner.movement.airJumpCount).Remove((object)this);
		}
	}

	[SerializeField]
	private int _count = 1;

	public AddAirJumpCount()
	{
	}

	public AddAirJumpCount(int count)
	{
		_count = count;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
