using System;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI;

public class StoneMonkeyBunsin : MonoBehaviour
{
	[Serializable]
	private class MotionMap
	{
		[SerializeField]
		private string _motionKey;

		[SerializeField]
		private Motion _targetMotion;

		public string motionKey => _motionKey;

		public Motion targetMotion => _targetMotion;
	}

	[SerializeField]
	private Minion _minion;

	[SerializeField]
	private MotionMap[] _motionMap;

	private void Awake()
	{
		_minion.onSummon += OnSummon;
		_minion.onUnsummon += OnUnsummon;
	}

	private void OnSummon(Character owner, Character summoned)
	{
		_minion.leader.player.onStartMotion += OnStartMotion;
	}

	private void OnUnsummon(Character owner, Character summoned)
	{
		_minion.leader.player.onStartMotion -= OnStartMotion;
	}

	private void OnStartMotion(Motion motion, float runSpeed)
	{
		if (_minion.state == Minion.State.Summoned)
		{
			MotionMap motionMap = Array.Find(_motionMap, (MotionMap map) => map.motionKey.Equals(motion.key, StringComparison.OrdinalIgnoreCase));
			if (motionMap != null)
			{
				_minion.character.DoMotion(motionMap.targetMotion, runSpeed);
			}
		}
	}
}
