using System.Collections.Generic;
using Characters.AI.Hero.LightSwords;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public sealed class TripleDanceMove : LightMove
{
	[SerializeField]
	private TripleDanceHelper _helper;

	private Queue<LightSword> _swords;

	private void Awake()
	{
		_swords = new Queue<LightSword>(3);
	}

	protected override LightSword GetDestination()
	{
		if (_swords.Count <= 0)
		{
			UpdateSwords();
		}
		return _swords.Dequeue();
	}

	private void UpdateSwords()
	{
		var (item, item2, item3) = _helper.GetStuck();
		_swords.Enqueue(item);
		_swords.Enqueue(item3);
		_swords.Enqueue(item2);
	}
}
