using UnityEngine;

namespace Characters.Projectiles.Operations;

public sealed class ApplyStatus : CharacterHitOperation
{
	[SerializeField]
	private CharacterStatus.ApplyInfo _status;

	[Range(1f, 100f)]
	[SerializeField]
	private int _chance = 100;

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit, Character target)
	{
		if (MMMaths.PercentChance(_chance))
		{
			projectile.owner.GiveStatus(target, _status);
		}
	}
}
