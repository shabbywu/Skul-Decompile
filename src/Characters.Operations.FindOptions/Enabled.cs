using System;
using UnityEngine;

namespace Characters.Operations.FindOptions;

[Serializable]
public class Enabled : ICondition
{
	public bool Satisfied(Character character)
	{
		return ((Component)character).gameObject.activeSelf;
	}
}
