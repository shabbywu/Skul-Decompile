using Characters.Abilities.Essences;
using Level;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Essences;

public class AttachKirizGlobal : CharacterOperation
{
	[Subcomponent(typeof(KirizComponent))]
	[SerializeField]
	private KirizComponent _abilityComponent;

	public override void Initialize()
	{
		_abilityComponent.Initialize();
	}

	public override void Run(Character target)
	{
		_abilityComponent.SetAttacker(target);
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
		return ExtensionMethods.GetAutoName((object)this);
	}
}
