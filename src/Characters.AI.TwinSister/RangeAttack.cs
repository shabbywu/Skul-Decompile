using System;
using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Hardmode;
using Singletons;
using UnityEngine;

namespace Characters.AI.TwinSister;

public class RangeAttack : MonoBehaviour
{
	[SerializeField]
	private Transform[] _attackPositions;

	[SerializeField]
	private Characters.Actions.Action _action;

	[MinMaxSlider(15f, 90f)]
	[SerializeField]
	private Vector2Int _angleOfMeteorInAir;

	[SerializeField]
	private float _distanceFromCenter = 6.5f;

	private Vector2 GetRightPosition(int minAngle, int maxAngle)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		float angle = Random.Range(minAngle, maxAngle);
		return RotateVector(Vector2.right, angle) * _distanceFromCenter;
	}

	private Vector2 GetLeftPosition(int minAngle, int maxAngle)
	{
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		float num = Random.Range(minAngle, maxAngle);
		return RotateVector(Vector2.right, num + 90f) * _distanceFromCenter;
	}

	private Vector2 RotateVector(Vector2 v, float angle)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		float num = angle * ((float)Math.PI / 180f);
		float num2 = v.x * Mathf.Cos(num) - v.y * Mathf.Sin(num);
		float num3 = v.x * Mathf.Sin(num) + v.y * Mathf.Cos(num);
		return new Vector2(num2, num3);
	}

	private void SetSpawnPosition()
	{
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ef: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0108: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_013f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0124: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0084: Unknown result type (might be due to invalid IL or missing references)
		//IL_0157: Unknown result type (might be due to invalid IL or missing references)
		//IL_015c: Unknown result type (might be due to invalid IL or missing references)
		bool num = MMMaths.RandomBool();
		List<Vector2> list = new List<Vector2>(3);
		if (num)
		{
			list.Add(GetLeftPosition(((Vector2Int)(ref _angleOfMeteorInAir)).x, ((Vector2Int)(ref _angleOfMeteorInAir)).y));
			Vector2 rightPosition = GetRightPosition(((Vector2Int)(ref _angleOfMeteorInAir)).x, ((Vector2Int)(ref _angleOfMeteorInAir)).y);
			float num2 = Mathf.Atan2(rightPosition.y, rightPosition.x) * 57.29578f;
			list.Add(rightPosition);
			if (num2 >= 45f)
			{
				list.Add(GetRightPosition(((Vector2Int)(ref _angleOfMeteorInAir)).x, 40));
			}
			else
			{
				list.Add(GetRightPosition(50, ((Vector2Int)(ref _angleOfMeteorInAir)).y));
			}
		}
		else
		{
			list.Add(GetRightPosition(((Vector2Int)(ref _angleOfMeteorInAir)).x, ((Vector2Int)(ref _angleOfMeteorInAir)).y));
			Vector2 leftPosition = GetLeftPosition(((Vector2Int)(ref _angleOfMeteorInAir)).x, ((Vector2Int)(ref _angleOfMeteorInAir)).y);
			float num3 = Mathf.Atan2(leftPosition.y, leftPosition.x) * 57.29578f;
			list.Add(leftPosition);
			if (num3 >= 45f)
			{
				list.Add(GetLeftPosition(((Vector2Int)(ref _angleOfMeteorInAir)).x, 40));
			}
			else
			{
				list.Add(GetLeftPosition(50, ((Vector2Int)(ref _angleOfMeteorInAir)).y));
			}
		}
		for (int i = 0; i < list.Count; i++)
		{
			_attackPositions[i].position = Vector2.op_Implicit(list[i]);
		}
	}

	private void SetHardmodeSpawnPosition()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		int num = 36;
		int num2 = 0;
		int num3 = num;
		for (int i = 0; i < _attackPositions.Length; i++)
		{
			_attackPositions[i].position = Vector2.op_Implicit(GetRightPosition(num2, num3));
			num2 += num;
			num3 += num;
		}
	}

	public IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		if (Singleton<HardmodeManager>.Instance.hardmode)
		{
			SetHardmodeSpawnPosition();
		}
		else
		{
			SetSpawnPosition();
		}
		_action.TryStart();
		while (_action.running && !character.health.dead)
		{
			yield return null;
		}
	}
}
