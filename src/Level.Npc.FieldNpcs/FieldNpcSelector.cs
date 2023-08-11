using System;
using System.Linq;
using Data;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Npc.FieldNpcs;

public class FieldNpcSelector : MonoBehaviour
{
	[Serializable]
	private class Npcs : ReorderableArray<Npcs.Property>
	{
		[Serializable]
		internal class Property
		{
			[SerializeField]
			private bool _bigCage;

			[SerializeField]
			private float _weight;

			[SerializeField]
			private FieldNpc _npc;

			public bool bigCage => _bigCage;

			public float weight => _weight;

			public FieldNpc npc => _npc;
		}
	}

	private const int _randomSeed = 699075432;

	[SerializeField]
	private Npcs _npcs;

	[SerializeField]
	private Prop _bigProp;

	[SerializeField]
	private SpriteRenderer _bigPropBehind;

	[SerializeField]
	private Sprite _bigBehindWreck;

	private Cage _cage;

	private void Awake()
	{
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
		Random random = new Random(GameData.Save.instance.randomSeed + 699075432 + (int)currentChapter.type * 256 + currentChapter.stageIndex * 16 + currentChapter.currentStage.pathIndex);
		_cage = ((Component)this).GetComponentInParent<Cage>();
		FieldNpc fieldNpc = null;
		Npcs.Property[] values = ((ReorderableArray<Npcs.Property>)_npcs).values;
		double num = random.NextDouble() * (double)values.Sum((Npcs.Property a) => a.weight);
		foreach (Npcs.Property property in values)
		{
			num -= (double)property.weight;
			if (num <= 0.0 && ((Object)(object)property.npc == (Object)null || !property.npc.encountered))
			{
				fieldNpc = property.npc;
				if (property.bigCage)
				{
					_cage.OverrideProp(_bigProp, _bigPropBehind, _bigBehindWreck);
				}
				break;
			}
		}
		if ((Object)(object)fieldNpc == (Object)null || !Map.TestingTool.fieldNPC)
		{
			_cage.Destroy();
			((Component)_cage).gameObject.SetActive(false);
		}
		else
		{
			fieldNpc = Object.Instantiate<FieldNpc>(fieldNpc, ((Component)this).transform);
			((Component)fieldNpc).transform.position = ((Component)this).transform.position;
			fieldNpc.SetCage(_cage);
		}
		Object.Destroy((Object)(object)this);
	}
}
