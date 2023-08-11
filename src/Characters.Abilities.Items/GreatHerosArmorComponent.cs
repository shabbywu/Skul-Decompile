namespace Characters.Abilities.Items;

public sealed class GreatHerosArmorComponent : AbilityComponent<GreatHerosArmor>, IAttackDamage
{
	public float amount { get; set; }
}
