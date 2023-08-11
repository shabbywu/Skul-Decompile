using Characters;
using Characters.Operations;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("Assets/Behavior Designer/Icon/RunOperation.png")]
public sealed class RunOperations : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedOperations _operations;

	private Character _ownerValue;

	private OperationInfos _operationsValue;

	public override void OnAwake()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
		_operationsValue = ((SharedVariable<OperationInfos>)_operations).Value;
		_operationsValue.Initialize();
	}

	public override void OnStart()
	{
		if (!((Component)_operationsValue).gameObject.activeSelf)
		{
			((Component)_operationsValue).gameObject.SetActive(true);
		}
		_operationsValue.Run(_ownerValue);
	}

	public override TaskStatus OnUpdate()
	{
		if (!_operationsValue.running)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)3;
	}
}
