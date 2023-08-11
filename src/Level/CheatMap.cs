using System;
using Characters;
using Characters.Abilities;
using UnityEngine;

namespace Level;

public sealed class CheatMap : MonoBehaviour
{
	[Serializable]
	private class ChefFoods
	{
		[SerializeField]
		private AbilityBuffList _abilityBuffList;

		[SerializeField]
		private Transform _chefFoodsDropPoint;

		[SerializeField]
		private float _distance;

		[SerializeField]
		private int _maxCount;

		public void Spawn()
		{
			//IL_0058: Unknown result type (might be due to invalid IL or missing references)
			//IL_0072: Unknown result type (might be due to invalid IL or missing references)
			//IL_007c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0081: Unknown result type (might be due to invalid IL or missing references)
			int num = ((_maxCount == 0) ? int.MaxValue : _maxCount);
			for (int i = 0; i < num; i++)
			{
				AbilityBuff abilityBuff = _abilityBuffList.abilityBuff[i];
				AbilityBuff abilityBuff2 = Object.Instantiate<AbilityBuff>(abilityBuff, _chefFoodsDropPoint);
				((Object)abilityBuff2).name = ((Object)abilityBuff).name;
				abilityBuff2.price = 0;
				abilityBuff2.Initialize();
				((Component)abilityBuff2).transform.position = Vector2.op_Implicit(new Vector2(_chefFoodsDropPoint.position.x + _distance * (float)i, _chefFoodsDropPoint.position.y));
			}
		}
	}

	[Serializable]
	private class Combat
	{
		[SerializeField]
		private Character _enemy;

		[SerializeField]
		private Transform _spawnPoint;

		public void SummonCombatTarget()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			Object.Instantiate<Character>(_enemy, _spawnPoint.position, Quaternion.identity, ((Component)Map.Instance).transform);
		}
	}

	[SerializeField]
	private ChefFoods _chefFoods;

	[SerializeField]
	private Combat _combat;

	private void Awake()
	{
		_chefFoods.Spawn();
	}

	public void SummonCombatTarget()
	{
		_combat.SummonCombatTarget();
	}
}
