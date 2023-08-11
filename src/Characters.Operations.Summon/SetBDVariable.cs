using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.Summon;

[Serializable]
public class SetBDVariable : IBDCharacterSetting
{
	[SerializeField]
	private string _variableName;

	[SerializeReference]
	[SubclassSelector]
	private SharedVariable _variable;

	public void ApplyTo(Character character)
	{
		((Component)character).GetComponent<BehaviorDesignerCommunicator>().SetVariable(_variableName, _variable);
	}
}
