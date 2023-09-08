using System;
using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.FindOptions;

[Serializable]
public class InBDVariable : IScope
{
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string _variableName = "Target";

	private List<Character> _enemise = new List<Character>();

	public List<Character> GetEnemyList()
	{
		_enemise.Clear();
		Character value = ((SharedVariable<Character>)_communicator.GetVariable<SharedCharacter>(_variableName)).Value;
		if ((Object)(object)value != (Object)null)
		{
			_enemise.Add(value);
		}
		return _enemise;
	}
}
