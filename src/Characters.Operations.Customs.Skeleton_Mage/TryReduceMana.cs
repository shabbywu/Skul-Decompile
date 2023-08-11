using Characters.Abilities.Weapons.Wizard;
using UnityEngine;

namespace Characters.Operations.Customs.Skeleton_Mage;

public sealed class TryReduceMana : CharacterOperation
{
	[SerializeField]
	private WizardPassiveComponent _passive;

	[SerializeField]
	private float _value;

	public override void Run(Character owner)
	{
		_passive.TryReduceMana(_value);
	}
}
