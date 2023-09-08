using System.Collections;
using System.Collections.Generic;
using FX;
using Level;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations;

public class HealEnemy : CharacterOperation
{
	private enum TargetType
	{
		LowestHealth,
		Random
	}

	private enum HealType
	{
		Percent,
		Constnat
	}

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private TargetType _targetType;

	[SerializeField]
	private HealType _healType;

	[SerializeField]
	private CustomFloat _amount;

	[SerializeField]
	private float _count;

	[SerializeField]
	private float _delay;

	[SerializeField]
	private EffectInfo _background;

	[SerializeField]
	private EffectInfo _effect;

	private EnemyWaveContainer _enemyWaveContainer;

	private static readonly NonAllocOverlapper _enemyOverlapper;

	static HealEnemy()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_enemyOverlapper = new NonAllocOverlapper(31);
		((ContactFilter2D)(ref _enemyOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	private void Start()
	{
		_enemyWaveContainer = Map.Instance.waveContainer;
	}

	public override void Run(Character owner)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		Character character = SelectTarget(owner);
		((Component)_effect.Spawn(((Component)character).transform.position, character)).transform.SetParent(((Component)character).transform);
		((Component)_background.Spawn(((Component)character).transform.position, character)).transform.SetParent(((Component)character).transform);
		((MonoBehaviour)this).StartCoroutine(CRun(owner, character));
	}

	private IEnumerator CRun(Character owner, Character target)
	{
		for (int i = 0; (float)i < _count; i++)
		{
			target.health.Heal(GetAmount(target));
			yield return owner.chronometer.master.WaitForSeconds(_delay);
		}
	}

	private double GetAmount(Character target)
	{
		return _healType switch
		{
			HealType.Percent => (double)_amount.value * target.health.maximumHealth * 0.01, 
			HealType.Constnat => _amount.value, 
			_ => 0.0, 
		};
	}

	private Character SelectTarget(Character owner)
	{
		List<Character> list = FindEnemiesInRange(_range);
		if (list.Count <= 1)
		{
			return owner;
		}
		switch (_targetType)
		{
		case TargetType.Random:
			return list.Random();
		case TargetType.LowestHealth:
		{
			Character character = owner;
			{
				foreach (Character item in list)
				{
					if (((Component)item).gameObject.activeSelf && !item.health.dead && item.health.percent < character.health.percent)
					{
						character = item;
					}
				}
				return character;
			}
		}
		default:
			return null;
		}
	}

	private List<Character> FindEnemiesInRange(Collider2D collider)
	{
		((Behaviour)collider).enabled = true;
		List<Character> components = _enemyOverlapper.OverlapCollider(collider).GetComponents<Character>(true);
		((Behaviour)collider).enabled = false;
		return components;
	}
}
