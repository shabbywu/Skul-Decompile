using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Gear.Synergy.Inscriptions;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Items;

public class KeywordRandomizer : MonoBehaviour
{
	private enum Type
	{
		Normal,
		Clone
	}

	private const int _randomSeed = 716722307;

	[SerializeField]
	private Item _item;

	[SerializeField]
	private Type _type;

	private Random _random;

	private void Awake()
	{
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		_random = new Random(GameData.Save.instance.randomSeed + 716722307 + (int)currentChapter.type * 16 + currentChapter.stageIndex);
		UpdateKeword();
	}

	private void UpdateKeword()
	{
		List<Inscription.Key> list = Inscription.keys.ToList();
		list.Remove(Inscription.Key.None);
		list.Remove(Inscription.Key.SunAndMoon);
		list.Remove(Inscription.Key.Masterpiece);
		list.Remove(Inscription.Key.Omen);
		list.Remove(Inscription.Key.Sin);
		_item.keyword1 = list.Random(_random);
		if (_type == Type.Normal)
		{
			list.Remove(_item.keyword1);
			_item.keyword2 = list.Random(_random);
		}
		else if (_type == Type.Clone)
		{
			_item.keyword2 = _item.keyword1;
		}
	}

	public void UpdateKeword(int count)
	{
		for (int i = 0; i < count; i++)
		{
			UpdateKeword();
		}
	}
}
