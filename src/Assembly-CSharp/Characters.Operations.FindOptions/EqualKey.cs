using System;
using UnityEngine;

namespace Characters.Operations.FindOptions;

[Serializable]
public class EqualKey : ICondition
{
	[SerializeField]
	private Key _key;

	public bool Satisfied(Character character)
	{
		return _key == character.key;
	}
}
