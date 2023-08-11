using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class HolyKnightsArcherAI : AIController
{
	[Header("Behaviours")]
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(Wander))]
	private Wander _wander;

	[SerializeField]
	[Subcomponent(typeof(Chase))]
	private Chase _chase;

	[Subcomponent(typeof(HorizontalProjectileAttack))]
	[SerializeField]
	private HorizontalProjectileAttack _sniping;

	[SerializeField]
	[Subcomponent(typeof(HorizontalProjectileAttack))]
	private HorizontalProjectileAttack _lightRain;

	[SerializeField]
	[Subcomponent(typeof(HorizontalProjectileAttack))]
	private HorizontalProjectileAttack _backStepShot;

	[SerializeField]
	[Header("Tools")]
	[Space]
	private CharacterAnimation _characterAnimation;

	[SerializeField]
	private AnimationClip _idleClipAfterWander;

	[SerializeField]
	private Collider2D _snipingTrigger;

	[SerializeField]
	private Collider2D _lightRainTrigger;

	[SerializeField]
	private Collider2D _backstepShotTrigger;

	[Range(0f, 1f)]
	[SerializeField]
	private float _counterChance;

	private bool _counter;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _chase, _sniping, _lightRain, _backStepShot };
		character.health.onTookDamage += TryCounterAttack;
	}

	private void OnDestroy()
	{
		_idleClipAfterWander = null;
	}

	private void TryCounterAttack(in Damage originalDamage, in Damage tookDamage, double damageDealt)
	{
		if (!_counter && _backStepShot.CanUse())
		{
			if (character.health.dead || base.dead || character.health.percent <= damageDealt)
			{
				_counter = true;
			}
			else if (_backStepShot.result != Behaviour.Result.Doing && _lightRain.result != Behaviour.Result.Doing && _sniping.result != Behaviour.Result.Doing && MMMaths.Chance(_counterChance))
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
					yield return _backStepShot.CRun(this);
					_counter = false;
				}
				if ((Object)(object)FindClosestPlayerBody(_backstepShotTrigger) != (Object)null && _backStepShot.CanUse())
				{
					yield return _backStepShot.CRun(this);
				}
				else if ((Object)(object)FindClosestPlayerBody(_lightRainTrigger) != (Object)null && _lightRain.CanUse())
				{
					yield return _lightRain.CRun(this);
				}
				else if ((Object)(object)FindClosestPlayerBody(_snipingTrigger) != (Object)null && _sniping.CanUse())
				{
					yield return _sniping.CRun(this);
				}
				else
				{
					yield return _chase.CRun(this);
				}
			}
		}
	}
}
