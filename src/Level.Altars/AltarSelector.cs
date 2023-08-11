using System;
using System.Linq;
using Data;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Altars;

public class AltarSelector : MonoBehaviour
{
	[Serializable]
	private class Altars : ReorderableArray<Altars.Property>
	{
		[Serializable]
		internal class Property
		{
			[SerializeField]
			private float _weight;

			[SerializeField]
			private Prop _altar;

			public float weight => _weight;

			public Prop altar => _altar;
		}
	}

	private const int _randomSeed = 898776742;

	[SerializeField]
	private Altars _altars;

	private void Awake()
	{
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		Random random = new Random(GameData.Save.instance.randomSeed + 898776742 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		Prop prop = null;
		Altars.Property[] values = ((ReorderableArray<Altars.Property>)_altars).values;
		double num = random.NextDouble() * (double)values.Sum((Altars.Property a) => a.weight);
		for (int i = 0; i < values.Length; i++)
		{
			num -= (double)values[i].weight;
			if (num <= 0.0)
			{
				prop = values[i].altar;
				break;
			}
		}
		if ((Object)(object)prop != (Object)null)
		{
			prop = Object.Instantiate<Prop>(prop, ((Component)this).transform.parent);
			((Component)prop).transform.position = ((Component)this).transform.position;
		}
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}
}
