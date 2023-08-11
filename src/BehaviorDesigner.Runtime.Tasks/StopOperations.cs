using Characters.Operations;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("실행중인 Operation을 취소합니다.")]
public sealed class StopOperations : Action
{
	[SerializeField]
	private SharedOperations _operation;

	private OperationInfos _operationValue;

	public override void OnAwake()
	{
		_operationValue = ((SharedVariable<OperationInfos>)_operation).Value;
	}

	public override TaskStatus OnUpdate()
	{
		if (!((Object)(object)_operationValue == (Object)null))
		{
			if (_operationValue.running)
			{
				_operationValue.Stop();
			}
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
