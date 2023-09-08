namespace Characters.Operations.Customs.GlacialSkull;

public class AddFreezeRemainHitStack : CharacterOperation
{
	public override void Run(Character owner)
	{
		owner.status.freeze.AddRemainHitStack();
	}
}
