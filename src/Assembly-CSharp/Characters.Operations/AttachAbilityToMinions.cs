using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Abilities;
using Unity.Mathematics;
using UnityEngine;

namespace Characters.Operations;

public class AttachAbilityToMinions : CharacterOperation
{
	private enum PriorityPolicy
	{
		NearToFar,
		FarToNear,
		Random
	}

	[SerializeField]
	[Tooltip("적용할 미니언 프리팹, 비워두면 모든 미니언에 적용됨")]
	private Minion _targetMinion;

	[SerializeField]
	private Minion[] _targetMinions;

	[Tooltip("적용할 범위, 비워두면 모든 범위에 적용됨")]
	[SerializeField]
	[Space]
	private Collider2D _range;

	[SerializeField]
	[Tooltip("최대 적용 가능한 미니언 수, 0이면 무제한")]
	private int _maxCount;

	[SerializeField]
	private PriorityPolicy _priorityPolicy;

	[Space]
	[SerializeField]
	[AbilityComponent.Subcomponent]
	private AbilityComponent _abilityComponent;

	private Character _target;

	public override void Initialize()
	{
		_abilityComponent.Initialize();
	}

	public override void Run(Character target)
	{
		//IL_00ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		_target = target;
		if (_target.playerComponents == null)
		{
			return;
		}
		if (_targetMinions != null && _targetMinions.Length != 0)
		{
			ApplyToMinions();
			return;
		}
		IEnumerable<Minion> enumerable = ((!((Object)(object)_targetMinion == (Object)null)) ? _target.playerComponents.minionLeader.GetMinionEnumerable(_targetMinion) : _target.playerComponents.minionLeader.GetMinionEnumerable());
		if (_maxCount == 0)
		{
			foreach (Minion item in enumerable)
			{
				if (!AddAbility(item))
				{
					break;
				}
			}
			return;
		}
		switch (_priorityPolicy)
		{
		case PriorityPolicy.NearToFar:
			AddabilityByDistance(enumerable, ((Component)target).transform.position, ((Minion minion, float distance) a, (Minion minion, float distance) b) => a.distance.CompareTo(b.distance));
			break;
		case PriorityPolicy.FarToNear:
			AddabilityByDistance(enumerable, ((Component)target).transform.position, ((Minion minion, float distance) a, (Minion minion, float distance) b) => b.distance.CompareTo(a.distance));
			break;
		case PriorityPolicy.Random:
			AddAbilityRandom(enumerable);
			break;
		}
	}

	private void ApplyToMinions()
	{
		//IL_00a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
		if (_targetMinions == null || _targetMinions.Length == 0)
		{
			return;
		}
		Minion[] targetMinions = _targetMinions;
		foreach (Minion targetMinion in targetMinions)
		{
			IEnumerable<Minion> minionEnumerable = _target.playerComponents.minionLeader.GetMinionEnumerable(targetMinion);
			if (_maxCount == 0)
			{
				foreach (Minion item in minionEnumerable)
				{
					if (!AddAbility(item))
					{
						return;
					}
				}
				continue;
			}
			switch (_priorityPolicy)
			{
			case PriorityPolicy.NearToFar:
				AddabilityByDistance(minionEnumerable, ((Component)_target).transform.position, ((Minion minion, float distance) a, (Minion minion, float distance) b) => a.distance.CompareTo(b.distance));
				break;
			case PriorityPolicy.FarToNear:
				AddabilityByDistance(minionEnumerable, ((Component)_target).transform.position, ((Minion minion, float distance) a, (Minion minion, float distance) b) => b.distance.CompareTo(a.distance));
				break;
			case PriorityPolicy.Random:
				AddAbilityRandom(minionEnumerable);
				break;
			}
		}
	}

	private void AddabilityByDistance(IEnumerable<Minion> enumerable, Vector3 origin, Comparison<(Minion minion, float distance)> comparison)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		List<(Minion, float)> list = new List<(Minion, float)>();
		foreach (Minion item2 in enumerable)
		{
			list.Add((item2, math.distancesq(float3.op_Implicit(origin), float3.op_Implicit(((Component)item2).transform.position))));
		}
		list.Sort(comparison);
		int num = 0;
		foreach (var item3 in list)
		{
			Minion item = item3.Item1;
			if (!AddAbility(item))
			{
				break;
			}
			num++;
			if (num == _maxCount)
			{
				break;
			}
		}
	}

	private void AddAbilityRandom(IEnumerable<Minion> enumerable)
	{
		int num = 0;
		Minion[] array = enumerable.ToArray();
		array.Shuffle();
		enumerable = array;
		foreach (Minion item in enumerable)
		{
			if (!AddAbility(item))
			{
				break;
			}
			num++;
			if (num == _maxCount)
			{
				break;
			}
		}
	}

	private bool AddAbility(Minion minion)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		Character character = minion.character;
		if ((Object)(object)_range != (Object)null && !_range.OverlapPoint(Vector2.op_Implicit(((Component)character).transform.position)))
		{
			return false;
		}
		character.ability.Add(_abilityComponent.ability);
		minion.onUnsummon -= RemoveAbilityOnUnsummon;
		minion.onUnsummon += RemoveAbilityOnUnsummon;
		return true;
	}

	private void RemoveAbilityOnUnsummon(Character owner, Character summoned)
	{
		summoned.ability.Remove(_abilityComponent.ability);
	}

	public override void Stop()
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_target == (Object)null || _target.playerComponents == null)
		{
			return;
		}
		IEnumerable<Minion> enumerable = ((!((Object)(object)_targetMinion == (Object)null)) ? _target.playerComponents.minionLeader.GetMinionEnumerable(_targetMinion) : _target.playerComponents.minionLeader.GetMinionEnumerable());
		foreach (Minion item in enumerable)
		{
			Character character = item.character;
			if (!((Object)(object)_range != (Object)null) || _range.OverlapPoint(Vector2.op_Implicit(((Component)character).transform.position)))
			{
				character.ability.Remove(_abilityComponent.ability);
				item.onUnsummon -= RemoveAbilityOnUnsummon;
			}
		}
	}

	public override string ToString()
	{
		return this.GetAutoName();
	}
}
