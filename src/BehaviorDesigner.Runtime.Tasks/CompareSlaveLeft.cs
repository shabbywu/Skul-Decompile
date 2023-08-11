using Characters;
using Characters.AI;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Character의 Master컴퍼넌트에 접근해 남은 Slave 숫자를 비교합니다.")]
public class CompareSlaveLeft : Conditional
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

	[Tooltip("master 컴퍼넌트를 자식으로 가지는 Character입니다. Master 컴퍼넌트가 없을 경우 Failure를 반환합니다.")]
	[SerializeField]
	private SharedCharacter _owner;

	public Operation operation;

	[Tooltip("Slave 갯수와 비교되는 대상입니다.")]
	public SharedInt integer;

	private Master _master;

	public override TaskStatus OnUpdate()
	{
		if (_owner == null)
		{
			return (TaskStatus)1;
		}
		Master componentInChildren = ((Component)((SharedVariable<Character>)_owner).Value).GetComponentInChildren<Master>();
		if ((Object)(object)componentInChildren == (Object)null)
		{
			return (TaskStatus)1;
		}
		switch (operation)
		{
		case Operation.LessThan:
			if (componentInChildren.GetSlavesLeft() < ((SharedVariable<int>)integer).Value)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		case Operation.LessThanOrEqualTo:
			if (componentInChildren.GetSlavesLeft() <= ((SharedVariable<int>)integer).Value)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		case Operation.EqualTo:
			if (componentInChildren.GetSlavesLeft() == ((SharedVariable<int>)integer).Value)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		case Operation.NotEqualTo:
			if (componentInChildren.GetSlavesLeft() != ((SharedVariable<int>)integer).Value)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		case Operation.GreaterThanOrEqualTo:
			if (componentInChildren.GetSlavesLeft() >= ((SharedVariable<int>)integer).Value)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		case Operation.GreaterThan:
			if (componentInChildren.GetSlavesLeft() > ((SharedVariable<int>)integer).Value)
			{
				return (TaskStatus)2;
			}
			return (TaskStatus)1;
		default:
			return (TaskStatus)1;
		}
	}

	public override void OnReset()
	{
		operation = Operation.LessThan;
		((SharedVariable<int>)integer).Value = 0;
	}
}
