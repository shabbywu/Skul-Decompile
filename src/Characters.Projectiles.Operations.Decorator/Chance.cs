using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator;

public class Chance : Operation
{
	[Range(0f, 1f)]
	[SerializeField]
	private float _successChance = 0.5f;

	[Subcomponent]
	[SerializeField]
	private Operation _onSuccess;

	[Subcomponent]
	[SerializeField]
	private Operation _onFail;

	public override void Run(IProjectile projectile)
	{
		if (MMMaths.Chance(_successChance))
		{
			if (!((Object)(object)_onSuccess == (Object)null))
			{
				_onSuccess.Run(projectile);
			}
		}
		else if (!((Object)(object)_onFail == (Object)null))
		{
			_onFail.Run(projectile);
		}
	}
}
