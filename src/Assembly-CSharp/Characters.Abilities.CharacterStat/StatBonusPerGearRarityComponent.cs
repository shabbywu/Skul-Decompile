namespace Characters.Abilities.CharacterStat;

public class StatBonusPerGearRarityComponent : AbilityComponent<StatBonusPerGearRarity>, IStackable
{
	public bool loaded { get; set; }

	public float stack
	{
		get
		{
			return base.baseAbility.count;
		}
		set
		{
			loaded = false;
			base.baseAbility.count = (int)value;
			loaded = true;
		}
	}
}
