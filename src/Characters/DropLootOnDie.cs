using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Gear;
using Characters.Movements;
using Level;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters;

public class DropLootOnDie : MonoBehaviour
{
	[Serializable]
	public class DarkQuartzPossibility
	{
		[Serializable]
		public class Reorderable : ReorderableArray<DarkQuartzPossibility>
		{
			public int Take()
			{
				if (base.values.Length == 0)
				{
					return 0;
				}
				int num = base.values.Sum((DarkQuartzPossibility v) => v.weight);
				int num2 = Random.Range(0, num) + 1;
				for (int i = 0; i < base.values.Length; i++)
				{
					num2 -= base.values[i].weight;
					if (num2 <= 0)
					{
						return (int)base.values[i].amount.value;
					}
				}
				return 0;
			}

			public float GetAverage()
			{
				float num = base.values.Sum((DarkQuartzPossibility v) => v.weight);
				float num2 = 0f;
				DarkQuartzPossibility[] values = base.values;
				foreach (DarkQuartzPossibility darkQuartzPossibility in values)
				{
					num2 += (float)((int)darkQuartzPossibility.amount.value * darkQuartzPossibility.weight) / num;
				}
				return num2;
			}
		}

		[Range(0f, 100f)]
		public int weight;

		public CustomFloat amount;
	}

	[Serializable]
	private class GearInfo
	{
		[SerializeField]
		private Characters.Gear.Gear _gear;

		[Range(1f, 100f)]
		[SerializeField]
		private int _weight;
	}

	private const float _droppedGearHorizontalInterval = 1.5f;

	private const float _droppedGearHorizontalSpeed = 2f;

	[GetComponent]
	[SerializeField]
	private Character _character;

	[FormerlySerializedAs("_amount")]
	[SerializeField]
	private int _gold;

	[SerializeField]
	private DarkQuartzPossibility.Reorderable _darkQuartzes;

	[SerializeField]
	private int _count;

	[SerializeField]
	private Characters.Gear.Gear _gear;

	[Range(0f, 100f)]
	[SerializeField]
	private int _gearChance;

	[SerializeField]
	private PotionPossibilities _potionPossibilities;

	public int gold => _gold;

	public DarkQuartzPossibility.Reorderable darkQuartzes => _darkQuartzes;

	private void Awake()
	{
		_character.health.onDie += OnDie;
	}

	private void OnDie()
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ba: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_004e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f6: Unknown result type (might be due to invalid IL or missing references)
		if (!_character.health.dead)
		{
			LevelManager levelManager = Singleton<Service>.Instance.levelManager;
			Push push = _character.movement?.push;
			Vector2 force = Vector2.zero;
			if (push != null && !push.expired)
			{
				force = push.direction * push.totalForce;
			}
			levelManager.DropGold(_gold, _count, ((Component)this).transform.position, force);
			levelManager.DropDarkQuartz(_darkQuartzes.Take(), ((Component)this).transform.position, force);
			List<DropMovement> list = new List<DropMovement>();
			Potion potion = _potionPossibilities.Get();
			if ((Object)(object)potion != (Object)null)
			{
				Potion potion2 = levelManager.DropPotion(potion, ((Component)this).transform.position);
				list.Add(potion2.dropMovement);
			}
			if (MMMaths.PercentChance(_gearChance))
			{
				Characters.Gear.Gear gear = Singleton<Service>.Instance.levelManager.DropGear(_gear, ((Component)this).transform.position);
				list.Add(gear.dropped.dropMovement);
			}
			DropMovement.SetMultiDropHorizontalInterval(list);
		}
	}
}
