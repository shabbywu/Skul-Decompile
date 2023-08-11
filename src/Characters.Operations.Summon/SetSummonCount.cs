using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.Summon;

[Serializable]
public class SetSummonCount : IBDCharacterSetting
{
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string _variableName = "SummonCount";

	public void ApplyTo(Character character)
	{
		SharedInt variable = _communicator.GetVariable<SharedInt>(_variableName);
		int value = ((SharedVariable<int>)variable).Value;
		((SharedVariable<int>)variable).Value = value + 1;
		character.onDie += delegate
		{
			SharedInt variable2 = _communicator.GetVariable<SharedInt>(_variableName);
			int value2 = ((SharedVariable<int>)variable2).Value;
			((SharedVariable<int>)variable2).Value = value2 - 1;
		};
	}
}
