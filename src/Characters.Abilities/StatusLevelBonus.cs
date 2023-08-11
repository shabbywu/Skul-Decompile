using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class StatusLevelBonus : Ability
{
	public class Instance : AbilityInstance<StatusLevelBonus>
	{
		private CharacterStatus _characterStatus;

		public Instance(Character owner, StatusLevelBonus ability)
			: base(owner, ability)
		{
			_characterStatus = ((Component)owner).GetComponent<CharacterStatus>();
		}

		protected override void OnAttach()
		{
			_characterStatus.gradeBonuses.Add((object)this, ability._level);
		}

		protected override void OnDetach()
		{
			((Sum<int>)(object)_characterStatus.gradeBonuses).Remove((object)this);
		}
	}

	[SerializeField]
	private int _level = 1;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
