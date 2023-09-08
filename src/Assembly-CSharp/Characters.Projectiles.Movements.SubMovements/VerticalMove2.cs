using System.Collections;
using UnityEngine;

namespace Characters.Projectiles.Movements.SubMovements;

public class VerticalMove2 : SubMovement
{
	[SerializeField]
	private float _height = 3f;

	[SerializeField]
	private Curve _curve;

	private IProjectile _projectile;

	public override void Move(IProjectile projectile)
	{
		_projectile = projectile;
		((MonoBehaviour)this).StartCoroutine(CMove());
	}

	private void OnEnable()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.localPosition = Vector2.op_Implicit(Vector2.zero);
	}

	private void OnDisable()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.localPosition = Vector2.op_Implicit(Vector2.zero);
		((MonoBehaviour)this).StopAllCoroutines();
	}

	private IEnumerator CMove()
	{
		float elpased = 0f;
		float startY = 0f;
		int lookingDirection = ((_projectile.owner.lookingDirection == Character.LookingDirection.Right) ? 1 : (-1));
		float destinationY = _height * (float)lookingDirection;
		Vector2 val = default(Vector2);
		while (elpased < _curve.duration)
		{
			yield return null;
			elpased += Chronometer.global.deltaTime;
			float num = Mathf.Lerp(startY, destinationY, _curve.Evaluate(elpased));
			((Vector2)(ref val))._002Ector(0f, num);
			Vector2 val2 = _projectile.firedDirection;
			val2 = ((!(elpased >= _curve.duration / 2f)) ? (val2 + Vector2.up * (float)lookingDirection) : (val2 + Vector2.down * (float)lookingDirection));
			_projectile.DetectCollision(Vector2.op_Implicit(((Component)this).transform.position), ((Vector2)(ref val2)).normalized, _projectile.owner.chronometer.projectile.deltaTime);
			((Component)this).transform.localPosition = Vector2.op_Implicit(val);
		}
	}
}
