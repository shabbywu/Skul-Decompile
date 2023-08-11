using Characters.Abilities;
using UnityEngine;

namespace Characters.Operations.Decorator;

public class ByAbility : CharacterOperation
{
	[SerializeField]
	private AbilityComponent _ability;

	[SerializeField]
	[Subcomponent]
	private Subcomponents _onAttached;

	[Subcomponent]
	[SerializeField]
	private Subcomponents _onDetached;

	public override void Run(Character owner)
	{
		((!owner.ability.Contains(_ability.ability)) ? _onDetached : _onAttached)?.Run(owner);
	}
}
