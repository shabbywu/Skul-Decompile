namespace Characters.Abilities;

public sealed class DetachAbilityByTriggerComponent : AbilityComponent<DetachAbilityByTrigger>
{
	private void OnDestroy()
	{
		base.baseAbility.Destroy();
	}
}
