using System;
using System.Collections.Generic;
using UnityEngine;
using Utils;

namespace Characters.Operations.FindOptions;

[Serializable]
public class InGrabBoard : IScope
{
	[SerializeField]
	private GrabBoard _grabBoard;

	public List<Character> GetEnemyList()
	{
		List<Character> list = new List<Character>();
		foreach (Target target in _grabBoard.targets)
		{
			list.Add(target.character);
		}
		return list;
	}
}
