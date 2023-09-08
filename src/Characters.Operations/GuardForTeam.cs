using System.Collections;
using System.Collections.Generic;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations;

public class GuardForTeam : CharacterOperation
{
	[SerializeField]
	private Collider2D _guardRange;

	[SerializeField]
	private Collider2D _guardBuffRange;

	[SerializeField]
	private float _duration;

	[Subcomponent]
	[SerializeField]
	private Subcomponents _onHitToOwner;

	[SerializeField]
	private ChronoInfo _onHitToOwnerChronoInfo;

	[SerializeField]
	[Subcomponent]
	private Subcomponents _onHitToOwnerFromRangeAttack;

	[SerializeField]
	[Subcomponent]
	private Subcomponents _onHitToTarget;

	private Character _owner;

	private static readonly NonAllocOverlapper _teamOverlapper;

	private bool _running;

	private List<Character> _teamCached;

	static GuardForTeam()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		_teamOverlapper = new NonAllocOverlapper(6);
		((ContactFilter2D)(ref _teamOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	private bool Block(ref Damage damage)
	{
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_0093: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		Attacker attacker = damage.attacker;
		if (damage.attackType == Damage.AttackType.Additional)
		{
			return false;
		}
		if (damage.attackType == Damage.AttackType.Ranged)
		{
			return false;
		}
		_ = ((Component)this).transform.position;
		Vector3 position = ((Component)damage.attacker.character).transform.position;
		Bounds bounds;
		if (_owner.lookingDirection == Character.LookingDirection.Right)
		{
			bounds = _guardRange.bounds;
			if (((Bounds)(ref bounds)).max.x < position.x)
			{
				GiveGuardEffect(ref damage, attacker.character);
				return true;
			}
		}
		else
		{
			bounds = _guardRange.bounds;
			if (((Bounds)(ref bounds)).min.x > position.x)
			{
				GiveGuardEffect(ref damage, attacker.character);
				return true;
			}
		}
		return false;
	}

	public override void Run(Character owner)
	{
		_owner = owner;
		_running = true;
		_teamCached = new List<Character>();
		if (_duration > 0f)
		{
			((MonoBehaviour)this).StartCoroutine(CExpire());
		}
	}

	private void Update()
	{
		if (_running)
		{
			GiveGuardBuff();
		}
	}

	private void GiveGuardEffect(ref Damage damage, Character attacker)
	{
		damage.stoppingPower = 0f;
		if (damage.attackType == Damage.AttackType.Melee)
		{
			_onHitToOwnerChronoInfo.ApplyGlobe();
			if (_onHitToOwner.components.Length != 0)
			{
				for (int i = 0; i < _onHitToOwner.components.Length; i++)
				{
					_onHitToOwner.components[i].Run(_owner);
				}
			}
			if (_onHitToTarget.components.Length != 0)
			{
				for (int j = 0; j < _onHitToTarget.components.Length; j++)
				{
					_onHitToTarget.components[j].Run(attacker);
				}
			}
		}
		else if ((damage.attackType == Damage.AttackType.Ranged || damage.attackType == Damage.AttackType.Projectile) && _onHitToOwnerFromRangeAttack.components.Length != 0)
		{
			for (int k = 0; k < _onHitToOwnerFromRangeAttack.components.Length; k++)
			{
				_onHitToOwnerFromRangeAttack.components[k].Run(_owner);
			}
		}
	}

	private IEnumerator CExpire()
	{
		_running = true;
		yield return _owner.chronometer.master.WaitForSeconds(_duration);
		_running = false;
		Stop();
	}

	public override void Stop()
	{
		_running = false;
		if (_teamCached != null)
		{
			foreach (Character item in _teamCached)
			{
				item.health.onTakeDamage.Remove(Block);
			}
		}
		_owner?.health.onTakeDamage.Remove(Block);
	}

	private List<Character> FindTeamBody(Collider2D collider)
	{
		((Behaviour)collider).enabled = true;
		List<Character> components = _teamOverlapper.OverlapCollider(collider).GetComponents<Character>(true);
		if (components.Count == 0)
		{
			((Behaviour)collider).enabled = false;
			return null;
		}
		return components;
	}

	private void GiveGuardBuff()
	{
		List<Character> list = FindTeamBody(_guardBuffRange);
		if (list == null)
		{
			foreach (Character item in _teamCached)
			{
				item.health.onTakeDamage.Remove(Block);
			}
		}
		foreach (Character item2 in _teamCached)
		{
			if (!list.Contains(item2))
			{
				item2.health.onTakeDamage.Remove(Block);
			}
		}
		foreach (Character item3 in list)
		{
			if (!item3.health.onTakeDamage.Contains(Block))
			{
				item3.health.onTakeDamage.Add(int.MinValue, Block);
			}
		}
		_teamCached.Clear();
		_teamCached.AddRange(list);
	}
}
