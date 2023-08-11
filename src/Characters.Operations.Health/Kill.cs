namespace Characters.Operations.Health;

public sealed class Kill : CharacterOperation
{
	public override void Run(Character owner)
	{
		Run(owner, owner);
	}

	public override void Run(Character owner, Character target)
	{
		target.health.Kill();
	}
}
