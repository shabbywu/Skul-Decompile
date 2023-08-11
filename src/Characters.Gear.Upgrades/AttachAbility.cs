using Characters.Abilities;
using UnityEngine;

namespace Characters.Gear.Upgrades;

public sealed class AttachAbility : UpgradeAbility
{
	[AbilityAttacher.Subcomponent]
	[SerializeField]
	private AbilityAttacher.Subcomponents _abilityAttacher;

	private Character _target;

	public override void Attach(Character target)
	{
		if ((Object)(object)target == (Object)null)
		{
			Debug.LogError((object)"Player is null");
			return;
		}
		_target = target;
		_abilityAttacher.Initialize(_target);
		_abilityAttacher.StartAttach();
	}

	public override void Detach()
	{
		_abilityAttacher.StopAttach();
	}
}
