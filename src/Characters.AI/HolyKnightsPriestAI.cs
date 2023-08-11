using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class HolyKnightsPriestAI : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	[Header("Behaviours")]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(KeepDistance))]
	[SerializeField]
	private KeepDistance _keepDistance;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _heal;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _holyLight;

	[SerializeField]
	[Space]
	[Header("Tools")]
	private Collider2D _keepDistanceTrigger;

	[Range(0f, 1f)]
	[SerializeField]
	private float _counterChance;

	private bool _counter;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _heal, _holyLight };
		character.health.onTookDamage += TryCounterAttack;
	}

	private void TryCounterAttack(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (!_counter && _holyLight.CanUse())
		{
			if (character.health.dead || base.dead || character.health.percent <= damageDealt)
			{
				_counter = true;
			}
			else if (_holyLight.result != Behaviour.Result.Doing && MMMaths.Chance(_counterChance))
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
			if (!((Object)(object)base.target == (Object)null) && !base.stuned)
			{
				if (_counter && character.health.currentHealth > 0.0 && !base.dead)
				{
					yield return _holyLight.CRun(this);
					_counter = false;
				}
				if ((Object)(object)FindClosestPlayerBody(_keepDistanceTrigger) != (Object)null && _keepDistance.CanUseBackMove())
				{
					yield return _keepDistance.CRun(this);
				}
				else if (_heal.CanUse())
				{
					yield return _heal.CRun(this);
				}
				else if (_holyLight.CanUse())
				{
					yield return _holyLight.CRun(this);
				}
			}
		}
	}
}
