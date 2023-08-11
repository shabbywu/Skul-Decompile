namespace Characters.Abilities.Weapons.GrimReaper;

public class GrimReaperPassiveComponent : AbilityComponent<GrimReaperPassive>, IStackable
{
	public float stack
	{
		get
		{
			return base.baseAbility.stack;
		}
		set
		{
			base.baseAbility.stack = (int)value;
			base.baseAbility.SetEnhanceSkill();
		}
	}

	public void AddStack()
	{
		base.baseAbility.AddStack();
	}
}
