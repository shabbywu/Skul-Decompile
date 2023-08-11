using System;
using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Level;

[Serializable]
public class DarkEnemyCandidate
{
	[SerializeField]
	private Character[] _lowTargets;

	[SerializeField]
	private Character[] _middleTargets;

	[SerializeField]
	private Character[] _highTargets;

	public Character Get(Tier tier, Random random)
	{
		switch (tier)
		{
		case Tier.Low:
			if (_lowTargets.Length == 0)
			{
				Debug.LogError((object)"target count is 0");
			}
			return ExtensionMethods.Random<Character>((IEnumerable<Character>)_lowTargets, random);
		case Tier.Middle:
			if (_middleTargets.Length == 0)
			{
				Debug.LogError((object)"target count is 0");
			}
			return ExtensionMethods.Random<Character>((IEnumerable<Character>)_middleTargets, random);
		case Tier.High:
			if (_highTargets.Length == 0)
			{
				Debug.LogError((object)"target count is 0");
			}
			return ExtensionMethods.Random<Character>((IEnumerable<Character>)_highTargets, random);
		default:
			return null;
		}
	}
}
