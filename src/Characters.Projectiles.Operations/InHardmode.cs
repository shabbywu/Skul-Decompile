using Characters.Operations;
using Hardmode;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public sealed class InHardmode : HitOperation
{
	[SerializeField]
	[Subcomponent]
	private CharacterOperation.Subcomponents _inHardmode;

	[Subcomponent]
	[SerializeField]
	private CharacterOperation.Subcomponents _inNormal;

	public void Awake()
	{
		_inHardmode.Initialize();
		_inNormal.Initialize();
	}

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
	{
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			_inHardmode.Run(projectile.owner);
		}
		else
		{
			_inNormal.Run(projectile.owner);
		}
	}
}
