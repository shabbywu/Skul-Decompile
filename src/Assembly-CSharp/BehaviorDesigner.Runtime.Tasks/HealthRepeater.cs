using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}RepeaterIcon.png")]
[TaskDescription("체력 조건을 만족할 경우 반복 실행")]
public class HealthRepeater : Decorator
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

	[Tooltip("The number of times to repeat the execution of its child task")]
	public SharedInt count = 1;

	[Tooltip("Allows the repeater to repeat forever")]
	public SharedBool repeatForever;

	[Tooltip("Should the task return if the child task returns a failure")]
	public SharedBool endOnFailure;

	private int executionCount;

	private TaskStatus executionStatus;

	public override bool CanExecute()
	{
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Invalid comparison between Unknown and I4
		if (CheackHealthCondition() && (((SharedVariable<bool>)repeatForever).Value || executionCount < ((SharedVariable<int>)count).Value))
		{
			if (((SharedVariable<bool>)endOnFailure).Value)
			{
				if (((SharedVariable<bool>)endOnFailure).Value)
				{
					return (int)executionStatus != 1;
				}
				return false;
			}
			return true;
		}
		return false;
	}

	public override void OnChildExecuted(TaskStatus childStatus)
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		executionCount++;
		executionStatus = childStatus;
	}

	public override void OnEnd()
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		executionCount = 0;
		executionStatus = (TaskStatus)0;
	}

	public override void OnReset()
	{
		count = 0;
		endOnFailure = true;
	}

	private bool CheackHealthCondition()
	{
		float num = ((_healthType == HealthType.Constant) ? ((float)((SharedVariable<Character>)_owner).Value.health.currentHealth) : ((float)((SharedVariable<Character>)_owner).Value.health.percent * 100f));
		return _operation switch
		{
			Operation.LessThan => num < ((SharedVariable<float>)_value).Value, 
			Operation.LessThanOrEqualTo => num <= ((SharedVariable<float>)_value).Value, 
			Operation.EqualTo => Mathf.Approximately(num, ((SharedVariable<float>)_value).Value), 
			Operation.NotEqualTo => !Mathf.Approximately(num, ((SharedVariable<float>)_value).Value), 
			Operation.GreaterThanOrEqualTo => num >= ((SharedVariable<float>)_value).Value, 
			Operation.GreaterThan => num > ((SharedVariable<float>)_value).Value, 
			_ => false, 
		};
	}
}
