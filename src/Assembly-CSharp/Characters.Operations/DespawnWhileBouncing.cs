using Characters.Projectiles;
using UnityEngine;

namespace Characters.Operations;

public class DespawnWhileBouncing : CharacterOperation
{
	[SerializeField]
	private BouncyProjectile _bouncyProjectile;

	[SerializeField]
	[Range(0f, 1f)]
	private float _speedReductionRate = 0.3f;

	[SerializeField]
	private float _minimumSpeedMultiplier = 0.2f;

	public override void Run(Character owner)
	{
		_bouncyProjectile.speedMultiplier *= 1f - _speedReductionRate;
		if (_bouncyProjectile.speedMultiplier < _minimumSpeedMultiplier)
		{
			_bouncyProjectile.Despawn();
		}
	}
}
