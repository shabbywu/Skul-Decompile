using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.BehaviorDesigner;

public sealed class SetBDVariableToTarget : TargetedCharacterOperation
{
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string variableName;

	public override void Run(Character owner, Character target)
	{
		if ((Object)(object)_communicator == (Object)null)
		{
			_communicator = ((Component)owner).GetComponent<BehaviorDesignerCommunicator>();
		}
		_communicator.SetVariable<SharedCharacter>(variableName, (object)target);
	}
}
