using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.BehaviorDesigner;

public class IncreaseSharedIntVariable : CharacterOperation
{
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string _variableName;

	[SerializeField]
	private int _value;

	public override void Run(Character owner)
	{
		if ((Object)(object)_communicator == (Object)null)
		{
			_communicator = ((Component)owner).GetComponent<BehaviorDesignerCommunicator>();
		}
		SharedInt variable = _communicator.GetVariable<SharedInt>(_variableName);
		((SharedVariable)variable).SetValue((object)(((SharedVariable<int>)variable).Value + _value));
	}
}
