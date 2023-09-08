using System;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Decorator;

public class SeedRandom : CharacterOperation
{
	[Subcomponent]
	[SerializeField]
	private Subcomponents _toRandom;

	private const int _randomSeed = 1020464638;

	private System.Random _random;

	public override void Initialize()
	{
		_toRandom.Initialize();
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new System.Random(GameData.Save.instance.randomSeed + 1020464638 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
	}

	public override void Run(Character owner)
	{
		CharacterOperation[] components = _toRandom.components;
		if (components.Length != 0)
		{
			components.PseudoShuffle(_random);
			if (!((Object)(object)components[0] == (Object)null))
			{
				components[0].Run(owner);
			}
		}
	}

	public override void Stop()
	{
		_toRandom.Stop();
	}
}
