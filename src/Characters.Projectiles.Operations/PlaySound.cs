using FX;
using Singletons;
using UnityEngine;

namespace Characters.Projectiles.Operations;

public sealed class PlaySound : Operation
{
	[SerializeField]
	private SoundInfo _soundInfo;

	[SerializeField]
	private Transform _position;

	public override void Run(IProjectile projectile)
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		PersistentSingleton<SoundManager>.Instance.PlaySound(_soundInfo, ((Object)(object)_position == (Object)null) ? ((Component)this).transform.position : _position.position);
	}
}
