using System.Collections;
using Characters;
using Characters.Gear;
using GameResources;
using Runnables;
using Services;
using Singletons;
using UnityEngine;

namespace CutScenes.SpecialMap;

public class MysticalRuin : MonoBehaviour
{
	[SerializeField]
	private Runnable _runnable;

	[SerializeField]
	[Range(0f, 100f)]
	private float _weaponWeight;

	[Range(0f, 100f)]
	[SerializeField]
	private float _itemWeight;

	[Range(0f, 100f)]
	[SerializeField]
	private float _quintessenceWeight;

	[SerializeField]
	private RarityPossibilities _rarityPossibilities;

	private WeightedRandomizer<Gear.Type> _weightedRandomizer;

	private Gear _gear;

	private void Awake()
	{
		_weightedRandomizer = WeightedRandomizer.From<Gear.Type>((Gear.Type.Item, _itemWeight), (Gear.Type.Weapon, _weaponWeight), (Gear.Type.Quintessence, _quintessenceWeight));
		DropGear();
	}

	private Gear.Type EvaluateGearType()
	{
		return _weightedRandomizer.TakeOne();
	}

	private void DropGear()
	{
		switch (EvaluateGearType())
		{
		case Gear.Type.Item:
			DropItem();
			break;
		case Gear.Type.Weapon:
			DropWeapon();
			break;
		case Gear.Type.Quintessence:
			DropQuintessence();
			break;
		}
	}

	private void DropItem()
	{
		ItemReference itemToTake;
		do
		{
			Rarity rarity = _rarityPossibilities.Evaluate();
			itemToTake = Singleton<Service>.Instance.gearManager.GetItemToTake(rarity);
		}
		while (itemToTake == null);
		ItemRequest request = itemToTake.LoadAsync();
		((MonoBehaviour)this).StartCoroutine(CDrop());
		IEnumerator CDrop()
		{
			while (!request.isDone)
			{
				yield return null;
			}
			_gear = Singleton<Service>.Instance.levelManager.DropItem(request, ((Component)this).transform.position);
			_gear.dropped.onLoot += Run;
			_gear.dropped.onDestroy += Run;
		}
	}

	private void DropWeapon()
	{
		ItemReference itemToTake;
		do
		{
			Rarity rarity = _rarityPossibilities.Evaluate();
			itemToTake = Singleton<Service>.Instance.gearManager.GetItemToTake(rarity);
		}
		while (itemToTake == null);
		ItemRequest request = itemToTake.LoadAsync();
		((MonoBehaviour)this).StartCoroutine(CDrop());
		IEnumerator CDrop()
		{
			while (!request.isDone)
			{
				yield return null;
			}
			_gear = Singleton<Service>.Instance.levelManager.DropItem(request, ((Component)this).transform.position);
			_gear.dropped.onLoot += Run;
			_gear.dropped.onDestroy += Run;
		}
	}

	private void DropQuintessence()
	{
		EssenceReference quintessenceToTake;
		do
		{
			Rarity rarity = _rarityPossibilities.Evaluate();
			quintessenceToTake = Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(rarity);
		}
		while (quintessenceToTake == null);
		EssenceRequest request = quintessenceToTake.LoadAsync();
		((MonoBehaviour)this).StartCoroutine(CDrop());
		IEnumerator CDrop()
		{
			while (!request.isDone)
			{
				yield return null;
			}
			_gear = Singleton<Service>.Instance.levelManager.DropQuintessence(request, ((Component)this).transform.position);
			_gear.dropped.onLoot += Run;
			_gear.dropped.onDestroy += Run;
		}
	}

	private void Run(Character character)
	{
		_gear.dropped.onLoot -= Run;
		_gear.dropped.onDestroy -= Run;
		_runnable.Run();
	}
}
