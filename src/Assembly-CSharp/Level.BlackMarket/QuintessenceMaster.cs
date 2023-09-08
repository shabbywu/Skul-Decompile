using System;
using System.Collections;
using Characters;
using Characters.Gear.Quintessences;
using Data;
using GameResources;
using Level.Npc;
using Services;
using Singletons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Level.BlackMarket;

public class QuintessenceMaster : Npc
{
	private const int _randomSeed = 1569003183;

	[SerializeField]
	private TMP_Text _priceDisplay;

	[SerializeField]
	private Transform _slot;

	[SerializeField]
	private NpcLineText _lineText;

	[SerializeField]
	private GameObject _talk;

	private int _price;

	private Random _random;

	public string submitLine => Localization.GetLocalizedStringArray("npc/QuintessenceMeister/submit/line").Random();

	private void Start()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 1569003183 + (int)currentChapter.type * 16 + currentChapter.stageIndex);
		if (MMMaths.PercentChance(_random, currentChapter.currentStage.marketSettings.quintessencePossibility))
		{
			Activate();
		}
		else
		{
			Deactivate();
		}
	}

	private IEnumerator CDropGear()
	{
		SettingsByStage settingsByStage = Singleton<Service>.Instance.levelManager.currentChapter.currentStage.marketSettings;
		GlobalSettings globalSetting = Settings.instance.marketSettings;
		Rarity rarity = settingsByStage.quintessenceMeisterPossibilities.Evaluate(_random);
		EssenceReference quintessenceToTake = Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(_random, rarity);
		EssenceRequest request = quintessenceToTake.LoadAsync();
		while (!request.isDone)
		{
			yield return null;
		}
		LevelManager levelManager = Singleton<Service>.Instance.levelManager;
		Quintessence droppedGear = levelManager.DropQuintessence(request, _slot.position);
		float num = (float)globalSetting.quintessenceMeisterPrices[rarity] * settingsByStage.quintessenceMeisterPriceMultiplier;
		num *= Random.Range(0.95f, 1.05f);
		_price = (int)(num / 10f) * 10;
		droppedGear.dropped.price = _price;
		bool destructible = droppedGear.destructible;
		droppedGear.destructible = false;
		droppedGear.dropped.dropMovement.Stop();
		_priceDisplay.text = _price.ToString();
		droppedGear.dropped.onLoot += OnLoot;
		void OnLoot(Character character)
		{
			droppedGear.destructible = destructible;
			_price = 0;
			_priceDisplay.text = "---";
			_lineText.Run(submitLine);
			droppedGear.dropped.onLoot -= OnLoot;
		}
	}

	protected override void OnActivate()
	{
		((Component)_lineText).gameObject.SetActive(true);
		_talk.SetActive(true);
		((MonoBehaviour)this).StartCoroutine(CDropGear());
	}

	protected override void OnDeactivate()
	{
		((Component)_lineText).gameObject.SetActive(false);
		_priceDisplay.text = string.Empty;
	}

	public void Update()
	{
		//IL_001f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		((Graphic)_priceDisplay).color = (GameData.Currency.gold.Has(_price) ? Color.white : Color.red);
	}
}
