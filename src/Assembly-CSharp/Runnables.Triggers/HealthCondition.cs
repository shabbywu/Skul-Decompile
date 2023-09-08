using Characters;
using UnityEngine;

namespace Runnables.Triggers;

public class HealthCondition : Trigger
{
	[SerializeField]
	private Character _character;

	[MinMaxSlider(0f, 1f)]
	[SerializeField]
	private Vector2 _range;

	protected override bool Check()
	{
		if (_character.health.percent >= (double)_range.x)
		{
			return _character.health.percent <= (double)_range.y;
		}
		return false;
	}
}
