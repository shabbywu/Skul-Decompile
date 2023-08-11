using System.Collections;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class Maid01AI : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(Chase))]
	private Chase _chase;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _attack;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _dashAfterAttack;

	[SerializeField]
	private Collider2D _attackCollider;

	[SerializeField]
	private Collider2D _dashAttackCollider;

	[SerializeField]
	[Subcomponent(typeof(MoveToDestination))]
	private MoveToDestination _dash;

	[SerializeField]
	private Action _dashBuff;

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		((MonoBehaviour)this).StartCoroutine(CChangeStopTrigger());
		yield return CCombat();
	}

	private IEnumerator CCombat()
	{
		while (!base.dead)
		{
			yield return null;
			if ((Object)(object)base.target == (Object)null)
			{
				continue;
			}
			if ((Object)(object)FindClosestPlayerBody(_dashAttackCollider) != (Object)null)
			{
				if ((Object)(object)FindClosestPlayerBody(_attackCollider) != (Object)null)
				{
					yield return _attack.CRun(this);
					continue;
				}
				if (_dashBuff.canUse)
				{
					yield return CDoDash();
					yield return _dashAfterAttack.CRun(this);
					continue;
				}
				yield return _chase.CRun(this);
				if (_chase.result == Behaviour.Result.Success)
				{
					yield return _attack.CRun(this);
				}
			}
			else
			{
				yield return _chase.CRun(this);
				if (_chase.result == Behaviour.Result.Success)
				{
					yield return _attack.CRun(this);
				}
			}
		}
	}

	private IEnumerator CDoDash()
	{
		base.destination = Vector2.op_Implicit(((Component)base.target).transform.position);
		_dashBuff.TryStart();
		((MonoBehaviour)this).StartCoroutine(_dash.CRun(this));
		yield return CStopDash();
		if (!character.hit.action.running)
		{
			character.CancelAction();
		}
	}

	private IEnumerator CStopDash()
	{
		while (!base.dead)
		{
			yield return null;
			if (_dash.result == Behaviour.Result.Doing)
			{
				if (base.stuned)
				{
					_dash.result = Behaviour.Result.Done;
					break;
				}
				continue;
			}
			break;
		}
	}

	private IEnumerator CChangeStopTrigger()
	{
		while (!base.dead)
		{
			yield return null;
			if (_dashBuff.canUse)
			{
				stopTrigger = _dashAttackCollider;
			}
			else
			{
				stopTrigger = _attackCollider;
			}
		}
	}
}
