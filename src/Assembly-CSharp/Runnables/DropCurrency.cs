using System;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables;

public class DropCurrency : Runnable
{
	[SerializeField]
	private CurrencyPossibilities _currencyPossibilities;

	[SerializeField]
	private RarityPossibilities _rarityPossibilities;

	[SerializeField]
	private Transform _dropPoint;

	public override void Run()
	{
		//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c2: Unknown result type (might be due to invalid IL or missing references)
		Rarity rarity = _rarityPossibilities.Evaluate();
		GameData.Currency.Type? type = _currencyPossibilities.Evaluate();
		IStageInfo currentStage = Singleton<Service>.Instance.levelManager.currentChapter.currentStage;
		int num = 0;
		switch (type)
		{
		case GameData.Currency.Type.Gold:
			num = currentStage.goldRangeByRarity.Evaluate(rarity);
			break;
		case GameData.Currency.Type.DarkQuartz:
			num = currentStage.darkQuartzRangeByRarity.Evaluate(rarity);
			break;
		case GameData.Currency.Type.Bone:
			num = currentStage.boneRangeByRarity.Evaluate(rarity);
			break;
		}
		if (num == 0)
		{
			throw new InvalidOperationException("드랍되는 재화의 값이 0입니다.");
		}
		Singleton<Service>.Instance.levelManager.DropCurrency(type.Value, num, (int)Mathf.Pow((float)num, 0.5f), ((Object)(object)_dropPoint == (Object)null) ? ((Component)this).transform.position : _dropPoint.position);
	}
}
