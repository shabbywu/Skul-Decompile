using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class HolyKnightsWizardAI : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	[Header("Behaviours")]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(ChaseTeleport))]
	[SerializeField]
	private ChaseTeleport _chaseTeleport;

	[SerializeField]
	[Subcomponent(typeof(CircularProjectileAttack))]
	private CircularProjectileAttack _homingRangeAttack;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _radialRangeAttack;

	[Header("Tools")]
	[Space]
	[SerializeField]
	private Collider2D _attackTrigger;

	[SerializeField]
	[Range(0f, 1f)]
	private float _counterChance;

	private bool _counter;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _chaseTeleport, _homingRangeAttack, _radialRangeAttack };
		character.health.onTookDamage += TryCounterAttack;
	}

	private void TryCounterAttack(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (!_counter && _radialRangeAttack.CanUse())
		{
			if (character.health.dead || base.dead || character.health.percent <= damageDealt)
			{
				_counter = true;
			}
			else if (_radialRangeAttack.result != Behaviour.Result.Doing && MMMaths.Chance(_counterChance))
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
		while (!base.dead)
		{
			yield return null;
			if ((Object)(object)base.target == (Object)null || base.stuned)
			{
				continue;
			}
			if (_counter && character.health.currentHealth > 0.0 && !base.dead)
			{
				yield return _radialRangeAttack.CRun(this);
				_counter = false;
			}
			if ((Object)(object)FindClosestPlayerBody(_attackTrigger) != (Object)null)
			{
				if (!_radialRangeAttack.CanUse())
				{
					yield return _homingRangeAttack.CRun(this);
				}
				else if (MMMaths.RandomBool())
				{
					yield return _homingRangeAttack.CRun(this);
				}
				else
				{
					yield return _radialRangeAttack.CRun(this);
				}
			}
			else
			{
				yield return _chaseTeleport.CRun(this);
			}
		}
	}
}
