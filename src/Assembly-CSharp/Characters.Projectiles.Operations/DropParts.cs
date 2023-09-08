using UnityEngine;

namespace Characters.Projectiles.Operations;

public class DropParts : HitOperation
{
	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private ParticleEffectInfo _particleEffectInfo;

	[SerializeField]
	private Vector2 _direction;

	[SerializeField]
	private float _power = 3f;

	[SerializeField]
	private bool _interpolation;

	private void OnDestroy()
	{
		_particleEffectInfo = null;
	}

	public override void Run(IProjectile projectile, RaycastHit2D raycastHit)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		_particleEffectInfo.Emit(((RaycastHit2D)(ref raycastHit)).point, _range.bounds, _direction * _power, _interpolation);
	}
}
