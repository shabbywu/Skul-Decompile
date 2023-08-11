using Characters.Gear.Quintessences;
using UnityEngine;

namespace Characters.Abilities.Essences;

public sealed class CharmComponent : AbilityComponent<Charm>
{
	[SerializeField]
	private Quintessence _essece;

	private void Awake()
	{
		_ability.SetAttacker(_essece.owner);
	}
}
