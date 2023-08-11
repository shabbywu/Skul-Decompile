using UnityEditor;
using UnityEngine;

namespace Characters.Operations.Decorator;

public sealed class HealthFilter : CharacterOperation
{
	private enum ValueType
	{
		Percent,
		Constant
	}

	private enum Compare
	{
		GreaterThan,
		LessThan
	}

	[SerializeField]
	private ValueType _valueType;

	[SerializeField]
	private Compare _compare;

	[SerializeField]
	[Information(/*Could not decode attribute arguments.*/)]
	private float _value;

	[SerializeField]
	[Subcomponent(typeof(TargetedOperationInfo))]
	private TargetedOperationInfo.Subcomponents _operations;

	public override void Initialize()
	{
		_operations.Initialize();
	}

	public override void Run(Character owner)
	{
		Run(owner, owner);
	}

	public override void Run(Character owner, Character target)
	{
		bool flag = false;
		switch (_compare)
		{
		case Compare.GreaterThan:
			if (_valueType == ValueType.Constant)
			{
				flag = target.health.currentHealth >= (double)_value;
			}
			else if (_valueType == ValueType.Percent)
			{
				flag = target.health.percent * 100.0 >= (double)_value;
			}
			break;
		case Compare.LessThan:
			if (_valueType == ValueType.Constant)
			{
				flag = target.health.currentHealth <= (double)_value;
			}
			else if (_valueType == ValueType.Percent)
			{
				flag = target.health.percent * 100.0 <= (double)_value;
			}
			break;
		}
		if (flag)
		{
			((MonoBehaviour)this).StartCoroutine(_operations.CRun(owner, target));
		}
	}

	public override void Stop()
	{
		((MonoBehaviour)this).StopAllCoroutines();
	}
}
