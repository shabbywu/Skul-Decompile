using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.Summon;

[Serializable]
public class SetVariableToCharacter : IBDCharacterSetting
{
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string _variableName;

	public void ApplyTo(Character character)
	{
		_communicator.SetVariable<SharedCharacter>(_variableName, (object)character);
	}
}
