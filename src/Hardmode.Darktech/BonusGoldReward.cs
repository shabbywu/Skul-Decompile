using System;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Hardmode.Darktech;

public sealed class BonusGoldReward : MonoBehaviour
{
	private const int _randomSeed = 2028506624;

	[SerializeField]
	private CurrencyBag _pureGoldBar;

	[SerializeField]
	private GoldReward _goldReward;

	private void Awake()
	{
		_goldReward.onLoot += Drop;
	}

	private void Drop()
	{
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0088: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		Random random = new Random(GameData.Save.instance.randomSeed + 2028506624 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		int goldCalculatorCount = Singleton<DarktechManager>.Instance.setting.GetGoldCalculatorCount(random, currentChapter.type, currentChapter.stageIndex);
		for (int i = 0; i < goldCalculatorCount; i++)
		{
			CurrencyBag currencyBag = Object.Instantiate<CurrencyBag>(_pureGoldBar, ((Component)this).transform.position + new Vector3(0f, 0.6f), Quaternion.identity, ((Component)Map.Instance).transform);
			((Object)currencyBag).name = ((Object)_pureGoldBar).name;
			currencyBag.released = true;
			currencyBag.count = Random.Range(20, 30);
			currencyBag.dropMovement.Pause();
			currencyBag.dropMovement.Move(MMMaths.RandomBool() ? Random.Range(-2.5f, -0.2f) : Random.Range(0.2f, 2.5f), Random.Range(14, 17));
		}
	}
}
