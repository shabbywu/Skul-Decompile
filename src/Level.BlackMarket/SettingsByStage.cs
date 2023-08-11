using System;
using UnityEngine;

namespace Level.BlackMarket;

[Serializable]
public class SettingsByStage
{
	[SerializeField]
	[Range(0f, 100f)]
	private int _collectorPossibility;

	[Range(0f, 100f)]
	[SerializeField]
	private int _masterPossibility;

	[Range(0f, 100f)]
	[SerializeField]
	private int _headlessPossibility;

	[SerializeField]
	[Range(0f, 100f)]
	private int _quintessenceMeisterPossibility;

	[Range(0f, 100f)]
	[SerializeField]
	private int _tombRaiderPossibility;

	[SerializeField]
	[Header("Collector")]
	private RarityPossibilities _collectorItemPossibilities;

	[SerializeField]
	private float _collectorItemPriceMultiplier = 1f;

	[SerializeField]
	[Header("Master (Chef)")]
	private RarityPossibilities _masterDishPossibilities;

	[SerializeField]
	private float _masterDishPriceMultiplier = 1f;

	[Header("Headless")]
	[SerializeField]
	private RarityPossibilities _headlessHeadPossibilities;

	[Header("Essence Meister")]
	[SerializeField]
	private RarityPossibilities _quintessenceMeisterPossibilities;

	[SerializeField]
	private float _quintessenceMeisterPriceMultiplier = 1f;

	[SerializeField]
	[Header("Tomb Raider")]
	private RarityPossibilities _tombRaiderGearPossibilities;

	[SerializeField]
	private RarityPrices _tombRaiderUnlockPrices;

	public int collectorPossibility => _collectorPossibility;

	public int masterPossibility => _masterPossibility;

	public int headlessPossibility => _headlessPossibility;

	public int quintessencePossibility => _quintessenceMeisterPossibility;

	public int tombRaiderPossibility => _tombRaiderPossibility;

	public RarityPossibilities collectorItemPossibilities => _collectorItemPossibilities;

	public float collectorItemPriceMultiplier => _collectorItemPriceMultiplier;

	public RarityPossibilities masterDishPossibilities => _masterDishPossibilities;

	public float masterDishPriceMultiplier => _masterDishPriceMultiplier;

	public RarityPossibilities headlessHeadPossibilities => _headlessHeadPossibilities;

	public RarityPossibilities quintessenceMeisterPossibilities => _quintessenceMeisterPossibilities;

	public float quintessenceMeisterPriceMultiplier => _quintessenceMeisterPriceMultiplier;

	public RarityPossibilities tombRaiderGearPossibilities => _tombRaiderGearPossibilities;

	public RarityPrices tombRaiderUnlockPrices => _tombRaiderUnlockPrices;
}
