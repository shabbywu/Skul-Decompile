using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Actions;
using Characters.Operations.Fx;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Temp;

public class FirstHeroAI : AIController
{
	[SerializeField]
	private LayerMask _adventLayerMask;

	[SerializeField]
	private Action _adventIdle;

	[SerializeField]
	private MotionTrail _motionTrail;

	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _advent;

	[Subcomponent(typeof(MoveToDestinationWithFly))]
	[SerializeField]
	private MoveToDestinationWithFly _dash;

	[SerializeField]
	[Subcomponent(typeof(TeleportInRangeWithFly))]
	private TeleportInRangeWithFly _teleportInRangeWithFly;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _commboAttack1;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _commboAttack2;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _commboAttack3;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _energyBlast;

	[SerializeField]
	[Subcomponent(typeof(CircularProjectileAttack))]
	private CircularProjectileAttack _circularProjectileAttack;

	[SerializeField]
	private Character _aura;

	[SerializeField]
	private Action _auraAction;

	[SerializeField]
	private GameObject _auraEffect;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _advent, _dash, _energyBlast };
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
		yield return DoAdvent();
		_auraAction.Initialize(_aura);
		((MonoBehaviour)this).StartCoroutine(DoRangeContinuousAttack());
		_auraEffect.SetActive(true);
		while (!base.dead)
		{
			if ((Object)(object)base.target == (Object)null)
			{
				yield return null;
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
		yield return DoDash();
		yield return _commboAttack1.CRun(this);
		yield return DoDash();
		yield return _commboAttack2.CRun(this);
		yield return DoDash();
		yield return _commboAttack3.CRun(this);
	}

	private IEnumerator DoAdvent()
	{
		if (base.target.movement.TryBelowRayCast(_adventLayerMask, out var point, 20f))
		{
			((Component)character).transform.position = Vector2.op_Implicit(((RaycastHit2D)(ref point)).point);
		}
		else
		{
			((Component)character).transform.position = ((Component)base.target).transform.position;
		}
		yield return _advent.CRun(this);
		_adventIdle.TryStart();
		while (_adventIdle.running)
		{
			yield return null;
		}
	}

	private IEnumerator DoDash()
	{
		yield return _teleportInRangeWithFly.CRun(this);
		if (((Component)character).transform.position.x > ((Component)base.target).transform.position.x)
		{
			character.lookingDirection = Character.LookingDirection.Left;
		}
		else
		{
			character.lookingDirection = Character.LookingDirection.Right;
		}
		base.destination = Vector2.op_Implicit(((Component)base.target).transform.position);
	}

	private IEnumerator DoEnergyBlast()
	{
		yield return DoDash();
		yield return _circularProjectileAttack.CRun(this);
	}

	private IEnumerator DoRangeContinuousAttack()
	{
		while (!base.dead)
		{
			if (_teleportInRangeWithFly.result == Behaviour.Result.Doing)
			{
				yield return null;
				continue;
			}
			yield return Chronometer.global.WaitForSeconds(0.5f);
			if (((Component)_auraAction).gameObject.activeSelf)
			{
				_auraAction.TryStart();
			}
		}
	}
}
