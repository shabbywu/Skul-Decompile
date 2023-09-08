using Characters.Abilities;
using Level;
using UnityEngine;

namespace Characters.Operations;

public class AttachAbilityGlobal : CharacterOperation
{
	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _abilityComponent;

	public override void Initialize()
	{
		_abilityComponent.Initialize();
	}

	public override void Run(Character target)
	{
		foreach (Character item in Map.Instance.waveContainer)
		{
			if (!((Object)(object)item.ability == (Object)null))
			{
				item.ability.Add(_abilityComponent.ability);
			}
		}
	}

	public override void Stop()
	{
		foreach (Character item in Map.Instance.waveContainer)
		{
			if (!((Object)(object)item.ability == (Object)null))
			{
				item.ability.Remove(_abilityComponent.ability);
			}
		}
	}

	public override string ToString()
	{
		return this.GetAutoName();
	}
}
