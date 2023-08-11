using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.DarkQuartzGolem;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class DarkQuartzGolem : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(Wander))]
	[SerializeField]
	private Wander _wander;

	[Subcomponent(typeof(Chase))]
	[SerializeField]
	private Chase _chase;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _idle;

	[Subcomponent(true, typeof(SimpleAction))]
	[SerializeField]
	private SimpleAction _summonAction;

	[SerializeField]
	[Subcomponent(typeof(Melee))]
	private Melee _melee;

	[Subcomponent(typeof(Rush))]
	[SerializeField]
	private Rush _rush;

	[Subcomponent(typeof(Range))]
	[SerializeField]
	private Range _range;

	[Subcomponent(typeof(Targeting))]
	[SerializeField]
	private Targeting _targeting;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _chase, _melee, _rush, _range, _targeting, _idle };
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return CIntro();
		yield return _idle.CRun(this);
		yield return _rush.CRun(this);
		yield return _idle.CRun(this);
		((MonoBehaviour)this).StartCoroutine(CChangeStopTrigger());
		while (!base.dead)
		{
			yield return Combat();
		}
	}

	private IEnumerator Combat()
	{
		while (!base.dead)
		{
			if ((Object)(object)base.target == (Object)null)
			{
				yield return null;
			}
			else if (base.stuned)
			{
				yield return null;
			}
			else if (_targeting.CanUse(this))
			{
				yield return _targeting.CRun(this);
				yield return _idle.CRun(this);
			}
			else if (_rush.CanUse(this))
			{
				yield return _rush.CRun(this);
				yield return _idle.CRun(this);
			}
			else if (_melee.CanUse(this))
			{
				if (MMMaths.RandomBool())
				{
					yield return _range.CRun(this);
				}
				else
				{
					yield return _melee.CRun(this);
				}
			}
			else if (_range.CanUse(this))
			{
				yield return _range.CRun(this);
			}
			else
			{
				yield return _targeting.CRun(this);
				yield return _idle.CRun(this);
			}
		}
	}

	private IEnumerator CIntro()
	{
		_summonAction.TryStart();
		while (_summonAction.running)
		{
			yield return null;
		}
	}

	private IEnumerator CChangeStopTrigger()
	{
		while (!base.dead)
		{
			if (_rush.CanUse(this))
			{
				stopTrigger = _range.trigger;
			}
			else
			{
				stopTrigger = _melee.trigger;
			}
			yield return null;
		}
	}
}
