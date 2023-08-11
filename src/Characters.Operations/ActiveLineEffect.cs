using FX;
using UnityEngine;

namespace Characters.Operations;

public class ActiveLineEffect : CharacterOperation
{
	[SerializeField]
	private LineEffect _lineEffect;

	[SerializeField]
	private Transform _startPoint;

	[SerializeField]
	private Transform _endPoint;

	public override void Run(Character owner)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		_lineEffect.startPoint = Vector2.op_Implicit(_startPoint.position);
		_lineEffect.endPoint = Vector2.op_Implicit(_endPoint.position);
		_lineEffect.Run();
	}

	public override void Stop()
	{
		_lineEffect.Hide();
	}
}
