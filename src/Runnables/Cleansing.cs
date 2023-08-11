using Characters.Abilities;
using UnityEngine;

namespace Runnables;

public class Cleansing : Runnable
{
	[SerializeField]
	private Target _target;

	public override void Run()
	{
		_target.character.playerComponents.savableAbilityManager.Remove(SavableAbilityManager.Name.Curse);
	}
}
