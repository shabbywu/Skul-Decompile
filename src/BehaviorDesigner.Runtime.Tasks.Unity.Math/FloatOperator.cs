using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.Math;

[TaskCategory("Unity/Math")]
[TaskDescription("Performs a math operation on two floats: Add, Subtract, Multiply, Divide, Min, or Max.")]
public class FloatOperator : Action
{
	public enum Operation
	{
		Add,
		Subtract,
		Multiply,
		Divide,
		Min,
		Max,
		Modulo
	}

	[Tooltip("The operation to perform")]
	public Operation operation;

	[Tooltip("The first float")]
	public SharedFloat float1;

	[Tooltip("The second float")]
	public SharedFloat float2;

	[Tooltip("The variable to store the result")]
	public SharedFloat storeResult;

	public override TaskStatus OnUpdate()
	{
		switch (operation)
		{
		case Operation.Add:
			((SharedVariable<float>)storeResult).Value = ((SharedVariable<float>)float1).Value + ((SharedVariable<float>)float2).Value;
			break;
		case Operation.Subtract:
			((SharedVariable<float>)storeResult).Value = ((SharedVariable<float>)float1).Value - ((SharedVariable<float>)float2).Value;
			break;
		case Operation.Multiply:
			((SharedVariable<float>)storeResult).Value = ((SharedVariable<float>)float1).Value * ((SharedVariable<float>)float2).Value;
			break;
		case Operation.Divide:
			((SharedVariable<float>)storeResult).Value = ((SharedVariable<float>)float1).Value / ((SharedVariable<float>)float2).Value;
			break;
		case Operation.Min:
			((SharedVariable<float>)storeResult).Value = Mathf.Min(((SharedVariable<float>)float1).Value, ((SharedVariable<float>)float2).Value);
			break;
		case Operation.Max:
			((SharedVariable<float>)storeResult).Value = Mathf.Max(((SharedVariable<float>)float1).Value, ((SharedVariable<float>)float2).Value);
			break;
		case Operation.Modulo:
			((SharedVariable<float>)storeResult).Value = ((SharedVariable<float>)float1).Value % ((SharedVariable<float>)float2).Value;
			break;
		}
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		operation = Operation.Add;
		float1 = 0f;
		float2 = 0f;
		storeResult = 0f;
	}
}
