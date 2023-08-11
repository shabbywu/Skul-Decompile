using Characters.Operations;
using UnityEngine;

namespace Characters.Abilities.Customs;

public class ArchlichSoulLootingPassiveComponent : AbilityComponent<ArchlichSoulLootingPassive>
{
	[SerializeField]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _operations;

	public override void Initialize()
	{
		_ability.operationsOnStacked = ((SubcomponentArray<CharacterOperation>)_operations).components;
		base.Initialize();
	}
}
