using System.Collections;
using Characters.Gear;
using GameResources;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public class Gauntlet : MonoBehaviour
{
	[SerializeField]
	private Transform _dropPoint;

	[SerializeField]
	private GearPossibilities _gearPossibilities;

	[SerializeField]
	private RarityPossibilities _rarityPossibilities;

	private GearReference _gearReference;

	private GearRequest _gearRequest;

	private Gear _droppedGear;

	public void Unlock()
	{
		_droppedGear.destructible = true;
		_droppedGear.lootable = true;
	}

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine("CDrop");
	}

	private void OnDestroy()
	{
		_gearRequest?.Release();
	}

	private IEnumerator CDrop()
	{
		Load();
		while (!_gearRequest.isDone)
		{
			yield return null;
		}
		_droppedGear = Singleton<Service>.Instance.levelManager.DropGear(_gearRequest, _dropPoint.position);
		_droppedGear.dropped.dropMovement.Stop();
		Lock();
	}

	private void Load()
	{
		Rarity key = _rarityPossibilities.Evaluate();
		Gear.Type? type = _gearPossibilities.Evaluate();
		do
		{
			Rarity rarity = Settings.instance.containerPossibilities[key].Evaluate();
			switch (type)
			{
			case Gear.Type.Weapon:
				_gearReference = Singleton<Service>.Instance.gearManager.GetWeaponToTake(rarity);
				break;
			case Gear.Type.Item:
				_gearReference = Singleton<Service>.Instance.gearManager.GetItemToTake(rarity);
				break;
			case Gear.Type.Quintessence:
				_gearReference = Singleton<Service>.Instance.gearManager.GetQuintessenceToTake(rarity);
				break;
			}
		}
		while (_gearReference == null);
		_gearRequest = _gearReference.LoadAsync();
	}

	private void Lock()
	{
		_droppedGear.destructible = false;
		_droppedGear.lootable = false;
	}
}
