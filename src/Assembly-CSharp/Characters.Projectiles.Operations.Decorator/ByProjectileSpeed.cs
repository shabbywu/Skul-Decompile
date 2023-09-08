using System.Collections;
using UnityEngine;

namespace Characters.Projectiles.Operations.Decorator;

public sealed class ByProjectileSpeed : Operation
{
	private enum Comparer
	{
		NotUsed,
		GreaterThanOrEqual,
		LessThan
	}

	[SerializeField]
	private float _checkDuration;

	[SerializeField]
	[Header("조건")]
	private float _horizontal;

	[SerializeField]
	private Comparer _horizontalComparer;

	[SerializeField]
	private float _vertical;

	[SerializeField]
	private Comparer _verticalComparer;

	[SerializeField]
	[Subcomponent]
	private Subcomponents _operations;

	private CoroutineReference _coroutineReference;

	public override void Run(IProjectile projectile)
	{
		if (_checkDuration == 0f)
		{
			if (CheckHorizontal(projectile) && CheckVertical(projectile))
			{
				_operations.Run(projectile);
			}
		}
		else
		{
			_coroutineReference.Stop();
			_coroutineReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CRun(projectile));
		}
	}

	private IEnumerator CRun(IProjectile projectile)
	{
		float remainTime = _checkDuration;
		yield return null;
		Debug.Log((object)"Run");
		while (remainTime > 0f)
		{
			if (CheckHorizontal(projectile) && CheckVertical(projectile))
			{
				_operations.Run(projectile);
				yield break;
			}
			remainTime -= Chronometer.global.deltaTime;
			yield return null;
		}
		Debug.Log((object)"End");
	}

	private bool CheckHorizontal(IProjectile projectile)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		switch (_horizontalComparer)
		{
		case Comparer.NotUsed:
			return true;
		case Comparer.GreaterThanOrEqual:
			if (projectile.firedDirection.x * projectile.speed >= _horizontal)
			{
				return true;
			}
			break;
		case Comparer.LessThan:
			if (projectile.firedDirection.x * projectile.speed <= _horizontal)
			{
				return true;
			}
			break;
		}
		Debug.Log((object)$"Horizontal : {projectile.firedDirection.x * projectile.speed}");
		return false;
	}

	private bool CheckVertical(IProjectile projectile)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		switch (_verticalComparer)
		{
		case Comparer.NotUsed:
			return true;
		case Comparer.GreaterThanOrEqual:
			if (projectile.firedDirection.y * projectile.speed >= _vertical)
			{
				return true;
			}
			break;
		case Comparer.LessThan:
			if (projectile.firedDirection.y * projectile.speed <= _vertical)
			{
				return true;
			}
			break;
		}
		return false;
	}
}
