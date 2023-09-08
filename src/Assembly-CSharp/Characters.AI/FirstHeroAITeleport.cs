using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Actions;
using Characters.Operations.Fx;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public class FirstHeroAITeleport : AIController
{
	[SerializeField]
	private Action _adventIdle;

	[SerializeField]
	private Action _rangeContinuousAttack;

	[SerializeField]
	private float _rangeContinuousAttackDuration;

	[SerializeField]
	private MotionTrail _motionTrail;

	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _advent;

	[Subcomponent(typeof(TeleportBehind))]
	[SerializeField]
	private TeleportBehind _teleportBehind;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _commboAttack;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _energyBlast;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _advent, _teleportBehind, _commboAttack, _energyBlast };
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return Chronometer.global.WaitForSeconds(1f);
		yield return DoAdvent();
		while (!base.dead)
		{
			if ((Object)(object)base.target == (Object)null)
			{
				yield return null;
				continue;
			}
			yield return DoDash();
			if (MMMaths.RandomBool())
			{
				yield return DoEnergyBlast();
			}
			else
			{
				yield return DoComoboAttack();
			}
		}
	}

	private IEnumerator DoComoboAttack()
	{
		float num = ((Component)base.target).transform.position.x - ((Component)character).transform.position.x;
		character.lookingDirection = ((!(num > 0f)) ? Character.LookingDirection.Left : Character.LookingDirection.Right);
		yield return _commboAttack.CRun(this);
	}

	private IEnumerator DoAdvent()
	{
		((Component)character).transform.position = ((Component)Singleton<Service>.Instance.levelManager.player).transform.position;
		yield return _advent.CRun(this);
		_adventIdle.TryStart();
		while (_adventIdle.running)
		{
			yield return null;
		}
	}

	private IEnumerator DoDash()
	{
		base.destination = Vector2.op_Implicit(((Component)base.target).transform.position);
		_motionTrail.Run(character);
		yield return _teleportBehind.CRun(this);
		_motionTrail.Stop();
	}

	private IEnumerator DoEnergyBlast()
	{
		yield return _energyBlast.CRun(this);
	}

	private IEnumerator DoRangeContinuousAttack()
	{
		while (!base.dead)
		{
			yield return Chronometer.global.WaitForSeconds(_rangeContinuousAttackDuration);
			_rangeContinuousAttack.TryStart();
		}
	}
}
