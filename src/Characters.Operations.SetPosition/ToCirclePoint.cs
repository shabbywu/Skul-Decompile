using System;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public sealed class ToCirclePoint : Policy
{
	[SerializeField]
	private Transform _center;

	[SerializeField]
	private CustomAngle _angleRange;

	[SerializeField]
	private float _radius;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		float num = _angleRange.value * ((float)Math.PI / 180f);
		Vector2 val = new Vector2(Mathf.Cos(num), Mathf.Sin(num)) * _radius;
		return Vector2.op_Implicit(((Component)_center).transform.position) + val;
	}
}
