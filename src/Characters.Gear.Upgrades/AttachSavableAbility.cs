using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Upgrades;

public sealed class AttachSavableAbility : UpgradeAbility
{
	[SerializeField]
	private SavableAbilityManager.Name _name;

	[SerializeField]
	private int _stack;

	private Character _target;

	public override void Attach(Character target)
	{
		if ((Object)(object)target == (Object)null)
		{
			Debug.LogError((object)"Player is null");
			return;
		}
		_target = target;
		if (_stack == 0)
		{
			_target.playerComponents.savableAbilityManager.Apply(_name);
		}
		else
		{
			_target.playerComponents.savableAbilityManager.Apply(_name, _stack);
		}
	}

	public override void Detach()
	{
		_target.playerComponents.savableAbilityManager.Remove(_name);
	}
}
