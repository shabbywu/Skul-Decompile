using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Characters.AI.Behaviours;
using FX;
using GameResources;
using PhysicsUtils;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.AI;

public abstract class AIController : MonoBehaviour
{
	private class Assets
	{
		internal static readonly EffectInfo effect = new EffectInfo(CommonResource.instance.enemyInSightEffect);
	}

	public enum StartOption
	{
		None,
		IdleUntilFindTarget,
		SetPlayerAsTarget
	}

	private static readonly NonAllocOverlapper _playerOverlapper;

	private static readonly NonAllocOverlapper _groundOverlapper;

	private static readonly NonAllocOverlapper _enemyOverlapper;

	private static readonly NonAllocCaster _reusableCaster;

	public Character character;

	public Collider2D stopTrigger;

	[SerializeField]
	private Collider2D _notifyCollider;

	[SerializeField]
	private Transform _findEffectTransform;

	[SerializeField]
	private bool _hideFindEffect;

	[SerializeField]
	private StartOption _startOption;

	public Character target { get; set; }

	public Character lastAttacker { get; set; }

	public List<Behaviour> behaviours { private get; set; }

	public StartOption startOption
	{
		get
		{
			return _startOption;
		}
		set
		{
			_startOption = value;
		}
	}

	public Vector2 destination { get; set; }

	public bool dead => character.health.dead;

	public bool stuned
	{
		get
		{
			if (!character.status.stuned)
			{
				return character.status.unmovable;
			}
			return true;
		}
	}

	public event Action onFind;

	static AIController()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Expected O, but got Unknown
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Expected O, but got Unknown
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Expected O, but got Unknown
		_playerOverlapper = new NonAllocOverlapper(15);
		((ContactFilter2D)(ref _playerOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(512));
		_groundOverlapper = new NonAllocOverlapper(15);
		((ContactFilter2D)(ref _groundOverlapper.contactFilter)).SetLayerMask(Layers.groundMask);
		_enemyOverlapper = new NonAllocOverlapper(31);
		((ContactFilter2D)(ref _enemyOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
		_reusableCaster = new NonAllocCaster(15);
	}

	public void RunProcess()
	{
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	protected abstract IEnumerator CProcess();

	public void FoundEnemy()
	{
		this.onFind?.Invoke();
		NotifyHitEvent();
		if (!_hideFindEffect)
		{
			SpawnFindTargetEffect();
		}
	}

	private void SpawnFindTargetEffect()
	{
		//IL_004a: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val;
		if ((Object)(object)_findEffectTransform == (Object)null)
		{
			Bounds bounds = ((Collider2D)character.collider).bounds;
			val = Vector2.op_Implicit(new Vector3(((Bounds)(ref bounds)).center.x, ((Bounds)(ref bounds)).max.y));
		}
		else
		{
			val = Vector2.op_Implicit(_findEffectTransform.position);
		}
		Assets.effect.Spawn(Vector2.op_Implicit(val));
	}

	public Character FindClosestPlayerBody(Collider2D collider)
	{
		//IL_006f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)collider).enabled = true;
		List<Target> components = _playerOverlapper.OverlapCollider(collider).GetComponents<Target>(true);
		if (components.Count == 0)
		{
			((Behaviour)collider).enabled = false;
			return null;
		}
		if (components.Count == 1)
		{
			((Behaviour)collider).enabled = false;
			return components[0].character;
		}
		float num = float.MaxValue;
		int index = 0;
		for (int i = 1; i < components.Count; i++)
		{
			ColliderDistance2D val = Physics2D.Distance((Collider2D)(object)components[i].character.collider, (Collider2D)(object)character.collider);
			float distance = ((ColliderDistance2D)(ref val)).distance;
			if (num > distance)
			{
				index = i;
				num = distance;
			}
		}
		((Behaviour)collider).enabled = false;
		return components[index].character;
	}

	public Character FindClosestPlayerBody(Collider2D range, Vector3 origin, LayerMask blockLayerMask)
	{
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0069: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007e: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)range).enabled = true;
		List<Target> components = _playerOverlapper.OverlapCollider(range).GetComponents<Target>(true);
		if (components.Count == 0)
		{
			((Behaviour)range).enabled = false;
			return null;
		}
		float num = float.MaxValue;
		int num2 = -1;
		((ContactFilter2D)(ref _reusableCaster.contactFilter)).SetLayerMask(blockLayerMask);
		for (int i = 0; i < components.Count; i++)
		{
			Character character = components[i].character;
			ColliderDistance2D val = Physics2D.Distance((Collider2D)(object)character.collider, (Collider2D)(object)this.character.collider);
			float distance = ((ColliderDistance2D)(ref val)).distance;
			NonAllocCaster reusableCaster = _reusableCaster;
			Vector2 val2 = Vector2.op_Implicit(origin);
			Bounds bounds = ((Collider2D)character.collider).bounds;
			if (reusableCaster.RayCast(val2, Vector2.op_Implicit(((Bounds)(ref bounds)).center - origin), distance * 1.5f).results.Count <= 0 && num > distance)
			{
				num2 = i;
				num = distance;
			}
		}
		((Behaviour)range).enabled = false;
		if (num2 == -1)
		{
			return null;
		}
		return components[num2].character;
	}

	public List<Character> FindEnemiesInRange(Collider2D collider)
	{
		((Behaviour)collider).enabled = true;
		List<Character> components = _enemyOverlapper.OverlapCollider(collider).GetComponents<Character>(true);
		((Behaviour)collider).enabled = false;
		return components;
	}

	public Collider2D FindClosestGround(Collider2D collider)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)collider).enabled = true;
		ReadonlyBoundedList<Collider2D> results = _groundOverlapper.OverlapCollider(collider).results;
		if (results.Count == 0)
		{
			((Behaviour)collider).enabled = false;
			return null;
		}
		if (results.Count == 1)
		{
			((Behaviour)collider).enabled = false;
			return results[0];
		}
		float num = float.MaxValue;
		int index = 0;
		for (int i = 1; i < results.Count; i++)
		{
			ColliderDistance2D val = Physics2D.Distance(results[i], (Collider2D)(object)character.collider);
			float distance = ((ColliderDistance2D)(ref val)).distance;
			if (num > distance)
			{
				index = i;
				num = distance;
			}
		}
		((Behaviour)collider).enabled = false;
		return results[index];
	}

	public List<Character> FindRandomEnemies(Collider2D collider, Character except, int amount)
	{
		((Behaviour)collider).enabled = true;
		List<Character> components = _enemyOverlapper.OverlapCollider(collider).GetComponents<Character>(true);
		foreach (Character item in components)
		{
			if ((Object)(object)item == (Object)(object)except)
			{
				components.Remove(item);
				break;
			}
		}
		if (components.Count <= 0)
		{
			((Behaviour)collider).enabled = false;
			return null;
		}
		int[] array = Enumerable.Range(0, components.Count).ToArray();
		array.PseudoShuffle();
		IEnumerable<int> enumerable = array.Take(amount);
		List<Character> list = new List<Character>(components.Count);
		foreach (int item2 in enumerable)
		{
			list.Add(components[item2]);
		}
		((Behaviour)collider).enabled = false;
		return list;
	}

	protected virtual void OnEnable()
	{
		if (!((Object)(object)character.health == (Object)null))
		{
			character.health.onTookDamage += onTookDamage;
		}
	}

	protected virtual void OnDisable()
	{
		if (!((Object)(object)character.health == (Object)null))
		{
			character.health.onTookDamage -= onTookDamage;
		}
	}

	protected void Start()
	{
		((MonoBehaviour)this).StartCoroutine(CCheckStun());
	}

	private void onTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (!(damageDealt <= 0.0) && !((Object)(object)originalDamage.attacker.character == (Object)null) && !((Object)(object)originalDamage.attacker.character.collider == (Object)null) && !((Object)(object)originalDamage.attacker.character.health == (Object)null) && ((Component)originalDamage.attacker.character).gameObject.layer == 9)
		{
			if ((Object)(object)target == (Object)null)
			{
				FoundEnemy();
			}
			target = originalDamage.attacker.character;
			lastAttacker = originalDamage.attacker.character;
		}
	}

	private void NotifyHitEvent()
	{
		if ((Object)(object)_notifyCollider == (Object)null)
		{
			return;
		}
		List<Character> list = FindEnemiesInRange(_notifyCollider);
		Collider2D lastStandingCollider = character.movement.controller.collisionState.lastStandingCollider;
		foreach (Character item in list)
		{
			Collider2D lastStandingCollider2 = item.movement.controller.collisionState.lastStandingCollider;
			if (!((Object)(object)lastStandingCollider != (Object)(object)lastStandingCollider2))
			{
				AIController componentInChildren = ((Component)item).GetComponentInChildren<AIController>();
				if (!((Object)(object)componentInChildren == (Object)null))
				{
					componentInChildren.target = target;
				}
			}
		}
	}

	protected IEnumerator CPlayStartOption()
	{
		StartOption startOption = _startOption;
		if (startOption != StartOption.IdleUntilFindTarget)
		{
			if (startOption == StartOption.SetPlayerAsTarget)
			{
				while ((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null)
				{
					yield return null;
				}
				target = Singleton<Service>.Instance.levelManager.player;
			}
		}
		else
		{
			while ((Object)(object)target == (Object)null)
			{
				yield return null;
			}
			FoundEnemy();
		}
	}

	public void StopAllCoroutinesWithBehaviour()
	{
		((MonoBehaviour)this).StopAllCoroutines();
		character.CancelAction();
		if (behaviours == null)
		{
			return;
		}
		foreach (Behaviour behaviour in behaviours)
		{
			behaviour.StopPropagation();
		}
	}

	public void StopAllBehaviour()
	{
		if (behaviours == null)
		{
			return;
		}
		foreach (Behaviour behaviour in behaviours)
		{
			behaviour.StopPropagation();
		}
	}

	protected IEnumerator CCheckStun()
	{
		if ((Object)(object)character.status == (Object)null)
		{
			yield break;
		}
		while (!dead)
		{
			yield return null;
			if (stuned)
			{
				StopAllBehaviour();
			}
		}
	}
}
