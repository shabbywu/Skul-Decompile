using Characters.Abilities.Triggers;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

public sealed class StatBonusAndTimeBonusByTriggerComponent : AbilityComponent<StatBonusAndTimeBonusByTrigger>
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
