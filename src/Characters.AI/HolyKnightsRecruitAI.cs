using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class HolyKnightsRecruitAI : AIController
{
	[Header("Behaviours")]
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(Wander))]
	[SerializeField]
	private Wander _wander;

	[Subcomponent(typeof(Chase))]
	[SerializeField]
	private Chase _chase;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _counterAttack;

	[SerializeField]
	[Subcomponent(typeof(ActionAttack))]
	private ActionAttack _comboAttack;

	[Space]
	[Header("Tools")]
	[SerializeField]
	private CharacterAnimation _characterAnimation;

	[SerializeField]
	private AnimationClip _idleClipAfterWander;

	[SerializeField]
	private Collider2D _comboAttackTrigger;

	[SerializeField]
	[Range(0f, 1f)]
	private float _counterChance;

	private bool _counter;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _chase, _counterAttack, _comboAttack };
		character.health.onTookDamage += TryCounterAttack;
	}

	private void OnDestroy()
	{
		_idleClipAfterWander = null;
	}

	private void TryCounterAttack(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (!_counter && _counterAttack.CanUse())
		{
			if (character.health.dead || base.dead || character.health.percent <= damageDealt)
			{
				_counter = true;
			}
			else if (_comboAttack.result != Behaviour.Result.Doing && MMMaths.Chance(_counterChance))
			{
				StopAllBehaviour();
				_counter = true;
			}
		}
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
		yield return CCombat();
	}

	private IEnumerator CCombat()
	{
		yield return _wander.CRun(this);
		_characterAnimation.SetIdle(_idleClipAfterWander);
		while (!base.dead)
		{
			yield return null;
			if (!((Object)(object)base.target == (Object)null) && !base.stuned)
			{
				if (_counter && character.health.currentHealth > 0.0 && !base.dead)
				{
					yield return _counterAttack.CRun(this);
					_counter = false;
				}
				if ((Object)(object)FindClosestPlayerBody(_comboAttackTrigger) != (Object)null)
				{
					yield return _comboAttack.CRun(this);
				}
				else
				{
					yield return _chase.CRun(this);
				}
			}
		}
	}
}
