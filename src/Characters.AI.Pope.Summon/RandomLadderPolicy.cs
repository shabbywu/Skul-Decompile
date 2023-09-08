using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Characters.AI.Pope.Summon;

public class RandomLadderPolicy : LadderPolicy
{
	[SerializeField]
	private Transform _ladderContainer;

	private FanaticLadder[] _ladders;

	private void Awake()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		_ladders = new FanaticLadder[_ladderContainer.childCount];
		int num = 0;
		foreach (Transform item in _ladderContainer)
		{
			Transform val = item;
			_ladders[num++] = ((Component)val).GetComponent<FanaticLadder>();
		}
	}

	protected override void GetLadders(ref List<FanaticLadder> results, int count)
	{
		results.Clear();
		_ladders.Shuffle();
		FanaticLadder[] array = _ladders.Take(count).ToArray();
		for (int i = 0; i < count; i++)
		{
			results.Add(array[i]);
		}
	}
}
