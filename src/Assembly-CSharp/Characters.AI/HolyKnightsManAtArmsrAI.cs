using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class HolyKnightsManAtArmsrAI : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	[Header("Behaviours")]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(Wander))]
	private Wander _wander;

	[Subcomponent(typeof(Chase))]
	[SerializeField]
	private Chase _chase;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _attack;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _tackle;

	[SerializeField]
	[Subcomponent(typeof(ContinuousTackle))]
	private ContinuousTackle _trippleTackle;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _holyWord;

	[Space]
	[Header("Holy Word Buff")]
	[SerializeField]
	private Stat.Values _HolyWordBuff;

	[SerializeField]
	private int _maxBuffStack;

	[SerializeField]
	[Space]
	[Header("Tools")]
	private Collider2D _attackTrigger;

	[SerializeField]
	private Collider2D _tackleTrigger;

	private int _buffStack = -1;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _chase, _attack, _tackle, _holyWord };
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(CUpdateStopTrigger());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return CCombat();
	}

	private IEnumerator CCombat()
	{
		yield return _wander.CRun(this);
		while (!base.dead)
		{
			yield return null;
			if ((Object)(object)base.target == (Object)null || base.stuned)
			{
				continue;
			}
			if (_holyWord.CanUse() && _buffStack < _maxBuffStack)
			{
				yield return _holyWord.CRun(this);
				if (_holyWord.result == Behaviour.Result.Success && _buffStack >= 0)
				{
					character.stat.AttachValues(_HolyWordBuff);
				}
				_buffStack++;
			}
			if ((Object)(object)FindClosestPlayerBody(_tackleTrigger) != (Object)null && _tackle.CanUse())
			{
				yield return _tackle.CRun(this);
				yield return _attack.CRun(this);
			}
			else if ((Object)(object)FindClosestPlayerBody(_attackTrigger) != (Object)null)
			{
				yield return _attack.CRun(this);
			}
			else
			{
				yield return _chase.CRun(this);
			}
		}
	}

	private IEnumerator CUpdateStopTrigger()
	{
		while (!base.dead)
		{
			yield return null;
			if (_tackle.CanUse())
			{
				stopTrigger = _tackleTrigger;
			}
			else
			{
				stopTrigger = _attackTrigger;
			}
		}
	}
}
