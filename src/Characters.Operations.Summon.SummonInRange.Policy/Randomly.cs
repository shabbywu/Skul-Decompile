using System;
using UnityEngine;

namespace Characters.Operations.Summon.SummonInRange.Policy;

[Serializable]
public class Randomly : ISummonPosition
{
	private float _prevRandomRange;

	public Vector2 GetPosition(Vector2 originPosition, float rangeX, int totalIndex, int currentIndex)
	{
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		if (_prevRandomRange == 0f)
		{
			_prevRandomRange = rangeX;
		}
		if (_prevRandomRange >= rangeX / 2f)
		{
			_prevRandomRange = Random.Range(0f, _prevRandomRange);
		}
		else
		{
			_prevRandomRange = Random.Range(_prevRandomRange, rangeX);
		}
		originPosition.x += _prevRandomRange - rangeX / 2f;
		return originPosition;
	}
}
