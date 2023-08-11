using UnityEngine;

namespace Characters.Operations.Summon.SummonInRange.Policy;

public interface ISummonPosition
{
	Vector2 GetPosition(Vector2 originPosition, float rangeX, int totalIndex, int currentIndex);
}
