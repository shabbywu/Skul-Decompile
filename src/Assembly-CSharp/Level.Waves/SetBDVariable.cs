using System;
using BehaviorDesigner.Runtime;
using Characters;
using UnityEngine;

namespace Level.Waves;

[Serializable]
public class SetBDVariable : IPinEnemyOption
{
	[SerializeField]
	private string _variableName;

	[SerializeReference]
	[SubclassSelector]
	private SharedVariable _variable;

	public void ApplyTo(Character character)
	{
		BehaviorDesignerCommunicator component = ((Component)character).GetComponent<BehaviorDesignerCommunicator>();
		if ((Object)(object)component != (Object)null && component.GetVariable(_variableName) != null)
		{
			component.SetVariable(_variableName, _variable);
		}
	}
}
