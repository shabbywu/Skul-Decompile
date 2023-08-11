using System;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Upgrades;

public sealed class AssetManagement : UpgradeAbility
{
	private const int _randomSeed = 2028506624;

	[SerializeField]
	[Range(0f, 100f)]
	private int _benefitChance;

	[SerializeField]
	[Range(0f, 100f)]
	private int _benefitPercent;

	[Range(0f, 100f)]
	[SerializeField]
	private int _lossPercent;

	private Random _random;

	private Character _target;

	public override void Attach(Character target)
	{
		_target = target;
		Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn += Settle;
	}

	private void Settle(Map old, Map @new)
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fe: Unknown result type (might be due to invalid IL or missing references)
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 2028506624 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		int amount;
		string colorValue;
		if (MMMaths.PercentChance(_random, _benefitChance))
		{
			amount = (int)((float)(GameData.Currency.gold.balance * _benefitPercent) * 0.01f);
			colorValue = ColorUtility.ToHtmlStringRGB(Color.yellow);
			GameData.Currency.gold.Earn(amount);
		}
		else
		{
			amount = (int)((float)(GameData.Currency.gold.balance * _lossPercent) * 0.01f);
			colorValue = ColorUtility.ToHtmlStringRGB(Color.gray);
			GameData.Currency.gold.Consume(amount);
		}
		Singleton<Service>.Instance.floatingTextSpawner.SpawnBuff(amount.ToString(), Vector2.op_Implicit(MMMaths.RandomPointWithinBounds(((Collider2D)_target.collider).bounds) + Vector2.up * 2f), colorValue);
	}

	public override void Detach()
	{
		Singleton<Service>.Instance.levelManager.onMapChangedAndFadedIn -= Settle;
	}
}
