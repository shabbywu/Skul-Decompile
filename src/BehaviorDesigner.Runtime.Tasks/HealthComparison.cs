using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public class HealthComparison : Conditional
{
	public enum Operation
	{
		LessThan,
		LessThanOrEqualTo,
		EqualTo,
		NotEqualTo,
		GreaterThanOrEqualTo,
		GreaterThan
	}

	public enum HealthType
	{
		Percent,
		Constant
	}

	[SerializeField]
	private Operation _operation;

	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedFloat _value;

	[SerializeField]
	private HealthType _healthType;

	public override TaskStatus OnUpdate()
	{
		float num = ((_healthType == HealthType.Constant) ? ((float)((SharedVariable<Character>)_owner).Value.health.currentHealth) : ((float)((SharedVariable<Character>)_owner).Value.health.percent * 100f));
		switch (_operation)
		{
		case Operation.LessThan:
			if (num < ((SharedVariable<float>)_value).Value)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		case Operation.LessThanOrEqualTo:
			if (num <= ((SharedVariable<float>)_value).Value)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		case Operation.EqualTo:
			if (Mathf.Approximately(num, ((SharedVariable<float>)_value).Value))
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		case Operation.NotEqualTo:
			if (!Mathf.Approximately(num, ((SharedVariable<float>)_value).Value))
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		case Operation.GreaterThanOrEqualTo:
			if (num >= ((SharedVariable<float>)_value).Value)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		case Operation.GreaterThan:
			if (num > ((SharedVariable<float>)_value).Value)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		default:
			return (TaskStatus)1;
		}
	}
}
