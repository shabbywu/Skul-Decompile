using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Runnables.Triggers;

public class HasBDVariable : Trigger
{
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string _variableName = "Target";

	protected override bool Check()
	{
		if (_communicator.GetVariable(_variableName).GetValue() == null)
		{
			return false;
		}
		return true;
	}
}
