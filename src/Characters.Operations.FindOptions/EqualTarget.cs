using System;
using UnityEngine;

namespace Characters.Operations.FindOptions;

[Serializable]
public class EqualTarget : ICondition
{
	[SerializeField]
	private Character _target;

	public bool Satisfied(Character character)
	{
		return (Object)(object)_target == (Object)(object)character;
	}
}
