using System.Collections.Generic;
using Characters.AI;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToLowHealthEnemy : Policy
{
	[SerializeField]
	private AIController _controller;

	[SerializeField]
	private Collider2D _findRange;

	[SerializeField]
	private bool _includeSelf = true;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		Character character = GetLowHealthCharacter();
		if ((Object)(object)character == (Object)null)
		{
			character = _controller.character;
		}
		return Vector2.op_Implicit(((Component)character).transform.position);
	}

	private Character GetLowHealthCharacter()
	{
		List<Character> list = _controller.FindEnemiesInRange(_findRange);
		double num = 1.0;
		Character result = null;
		foreach (Character item in list)
		{
			if (item.liveAndActive && (_includeSelf || !((Object)(object)item == (Object)(object)_controller.character)) && item.health.percent < num)
			{
				num = item.health.percent;
				result = item;
			}
		}
		return result;
	}
}
