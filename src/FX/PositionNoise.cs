using System;
using UnityEngine;

namespace FX;

[Serializable]
public class PositionNoise
{
	public enum Method
	{
		None,
		InsideCircle,
		Horizontal,
		Vertical
	}

	[SerializeField]
	private Method _method;

	[SerializeField]
	private float _value;

	public Vector3 Evaluate()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		return (Vector3)(_method switch
		{
			Method.InsideCircle => Vector2.op_Implicit(Random.insideUnitCircle * _value), 
			Method.Horizontal => new Vector3(Random.Range(0f - _value, _value), 0f), 
			Method.Vertical => new Vector3(0f, Random.Range(0f - _value, _value)), 
			_ => Vector3.zero, 
		});
	}

	public Vector2 EvaluateAsVector2()
	{
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		return (Vector2)(_method switch
		{
			Method.InsideCircle => Random.insideUnitCircle * _value, 
			Method.Horizontal => new Vector2(Random.Range(0f - _value, _value), 0f), 
			Method.Vertical => new Vector2(0f, Random.Range(0f - _value, _value)), 
			_ => Vector2.zero, 
		});
	}
}
