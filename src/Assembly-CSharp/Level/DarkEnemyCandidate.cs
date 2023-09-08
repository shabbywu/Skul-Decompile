using System;
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
			return _lowTargets.Random(random);
		case Tier.Middle:
			if (_middleTargets.Length == 0)
			{
				Debug.LogError((object)"target count is 0");
			}
			return _middleTargets.Random(random);
		case Tier.High:
			if (_highTargets.Length == 0)
			{
				Debug.LogError((object)"target count is 0");
			}
			return _highTargets.Random(random);
		default:
			return null;
		}
	}
}
