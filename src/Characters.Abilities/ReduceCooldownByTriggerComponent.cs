using Characters.Abilities.Triggers;
using UnityEngine;

namespace Characters.Abilities;

public class ReduceCooldownByTriggerComponent : AbilityComponent<ReduceCooldownByTrigger>
{
	[TriggerComponent.Subcomponent]
	[SerializeField]
	private TriggerComponent _triggerComponent;

	public override void Initialize()
	{
		_ability.trigger = _triggerComponent;
		base.Initialize();
	}
}
