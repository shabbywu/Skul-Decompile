using System;
using System.Collections;
using UnityEngine;

namespace Level.Objects.DecorationCharacter.Command;

public class RandomCommandRunner : MonoBehaviour
{
	[Serializable]
	private struct WeightedCommand
	{
		[SerializeReference]
		[SubclassSelector]
		public ICommand command;

		public int weight;
	}

	[SubclassSelector]
	[SerializeReference]
	private ICommand[] _startCommands;

	[SerializeField]
	private WeightedCommand[] _weightedCommands;

	private int _currentIndex;

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine(CRunSequance());
	}

	private IEnumerator CRunSequance()
	{
		ICommand[] startCommands = _startCommands;
		foreach (ICommand command in startCommands)
		{
			yield return command.CRun();
		}
		if (_weightedCommands.Length != 0)
		{
			while (true)
			{
				_currentIndex = GetWeightedRandomIndex();
				yield return _weightedCommands[_currentIndex].command.CRun();
			}
		}
	}

	private int GetWeightedRandomIndex()
	{
		int num = 0;
		WeightedCommand[] weightedCommands = _weightedCommands;
		for (int i = 0; i < weightedCommands.Length; i++)
		{
			WeightedCommand weightedCommand = weightedCommands[i];
			num += weightedCommand.weight;
		}
		if (num == 0)
		{
			return Random.Range(0, _weightedCommands.Length);
		}
		int num2 = Random.Range(0, num);
		for (int j = 0; j < _weightedCommands.Length; j++)
		{
			if (num2 < _weightedCommands[j].weight)
			{
				return j;
			}
			num2 -= _weightedCommands[j].weight;
		}
		return 0;
	}
}
