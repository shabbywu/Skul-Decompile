using Characters;
using UnityEngine;

namespace Runnables.Triggers;

public class ByPosition : Trigger
{
	private enum Direction
	{
		Left,
		Right
	}

	[SerializeField]
	private Target _target;

	[SerializeField]
	private Direction _direction;

	[SerializeField]
	private Transform _base;

	protected override bool Check()
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		Character character = _target.character;
		if ((Object)(object)character == (Object)null)
		{
			return false;
		}
		if (_direction == Direction.Left && ((Component)character).transform.position.x < _base.position.x)
		{
			return true;
		}
		if (_direction == Direction.Right && ((Component)character).transform.position.x > _base.position.x)
		{
			return true;
		}
		return false;
	}
}
