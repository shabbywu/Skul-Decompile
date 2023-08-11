using System;
using UnityEngine;

namespace Characters.Operations.Summon.SummonInRange.Policy;

[Serializable]
public class IntervalRegularly : ISummonPosition
{
	[SerializeField]
	private bool _revese;

	private float _interval;

	public Vector2 GetPosition(Vector2 originPosition, float rangeX, int totalIndex, int currentIndex)
	{
		//IL_0004: Unknown result type (might be due to invalid IL or missing references)
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		if (totalIndex == 1)
		{
			return originPosition;
		}
		if (_interval == 0f)
		{
			_interval = rangeX / (float)(totalIndex - 1);
		}
		Vector2 result = originPosition;
		if (!_revese)
		{
			result.x += _interval * (float)currentIndex - rangeX / 2f;
		}
		else
		{
			result.x += rangeX / 2f - _interval * (float)currentIndex;
		}
		return result;
	}
}
