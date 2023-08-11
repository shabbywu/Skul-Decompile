using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.BehaviorDesigner;

public sealed class SetBehaviorTreeVariable : Operation
{
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string variableName;

	[SerializeReference]
	[SubclassSelector]
	private SharedVariable _variable;

	public override void Run()
	{
		_communicator.SetVariable(variableName, _variable);
	}
}
