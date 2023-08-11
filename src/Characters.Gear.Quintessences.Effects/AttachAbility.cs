using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Quintessences.Effects;

public sealed class AttachAbility : QuintessenceEffect
{
	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _abilityComponent;

	private Character _owner;

	private void Awake()
	{
		_abilityComponent.Initialize();
	}

	protected override void OnInvoke(Quintessence quintessence)
	{
		Character owner = quintessence.owner;
		owner.ability.Add(_abilityComponent.ability);
		if ((Object)(object)owner != (Object)(object)_owner)
		{
			quintessence.onDropped += Detach;
		}
		_owner = owner;
	}

	private void Detach()
	{
		_owner.ability.Remove(_abilityComponent.ability);
	}
}
