using System.Collections.Generic;
using System.Linq;
using Characters.Actions;
using GameResources;
using UnityEngine;

namespace Characters.Gear.Weapons;

public class SkillInfo : MonoBehaviour
{
	[SerializeField]
	private string _key;

	[SerializeField]
	private bool _hasAlways;

	[Range(0f, 100f)]
	[SerializeField]
	private int _weight = 1;

	public string key => _key;

	public bool hasAlways => _hasAlways;

	public int weight => _weight;

	public Sprite cachedIcon { get; private set; }

	public string displayName => Localization.GetLocalizedString("skill/" + _key + "/name");

	public string description => Localization.GetLocalizedString("skill/" + _key + "/desc");

	public Action action { get; private set; }

	public static SkillInfo WeightedRandomPop(List<SkillInfo> from)
	{
		int num = from.Sum((SkillInfo s) => s.weight);
		int num2 = Random.Range(0, num) + 1;
		for (int i = 0; i < from.Count; i++)
		{
			SkillInfo skillInfo = from[i];
			num2 -= skillInfo.weight;
			if (num2 <= 0)
			{
				from.RemoveAt(i);
				return skillInfo;
			}
		}
		return from[0];
	}

	public void Initialize()
	{
		action = ((Component)this).GetComponent<Action>();
		cachedIcon = GearResource.instance.GetSkillIcon(_key);
		if ((Object)(object)cachedIcon == (Object)null)
		{
			Debug.LogError((object)$"Couldn't find a skill icon file: {cachedIcon}.png");
		}
	}

	public Sprite GetIcon()
	{
		return GearResource.instance.GetSkillIcon(_key);
	}
}
