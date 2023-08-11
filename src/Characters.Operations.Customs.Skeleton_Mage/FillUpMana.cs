using Characters.Abilities.Weapons.Wizard;

namespace Characters.Operations.Customs.Skeleton_Mage;

public sealed class FillUpMana : CharacterOperation
{
	public override void Run(Character owner)
	{
		owner.ability.GetInstanceByInstanceType<WizardPassive.Instance>()?.FillUp();
	}
}
