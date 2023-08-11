using System.Collections;
using UnityEngine;

namespace Characters.Projectiles.Movements;

public class ReadyAndFire : Movement
{
	[SerializeField]
	private float _readyTime;

	[SerializeField]
	private float _speed;

	private Character.LookingDirection _initialLookingDirection;

	public override void Initialize(IProjectile projectile, float direction)
	{
		base.Initialize(projectile, direction);
		_initialLookingDirection = projectile.owner.lookingDirection;
		projectile.transform.SetParent(projectile.owner.attachWithFlip.transform);
		((MonoBehaviour)this).StartCoroutine(CReadyAndFire());
	}

	public override (Vector2 direction, float speed) GetSpeed(float time, float deltaTime)
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		if (time < _readyTime)
		{
			return (base.directionVector, 0f);
		}
		return (base.directionVector, _speed * base.projectile.speedMultiplier);
	}

	private IEnumerator CReadyAndFire()
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)base.projectile.owner.chronometer.master, _readyTime);
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(0f - base.directionVector.x, base.directionVector.y);
		base.directionVector = ((base.projectile.owner.lookingDirection == _initialLookingDirection) ? base.directionVector : val);
		base.projectile.transform.SetParent((Transform)null);
	}
}
