using Characters.Abilities.Weapons.Wizard;
using UnityEngine;

namespace Characters.Operations.Customs.Skeleton_Mage;

public sealed class AddManaChargingSpeedMultiplier : CharacterOperation
{
	[SerializeField]
	private WizardPassiveComponent _passive;

	[SerializeField]
	private float _value;

	public override void Run(Character owner)
	{
		_passive.manaChargingSpeedMultiplier += _value;
	}

	public override void Stop()
	{
		base.Stop();
		_passive.manaChargingSpeedMultiplier -= _value;
	}
}
