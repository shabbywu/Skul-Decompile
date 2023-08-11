using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class FanaticAI : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	[Header("Behaviours")]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(Wander))]
	[SerializeField]
	private Wander _wander;

	[SerializeField]
	[Subcomponent(typeof(Chase))]
	private Chase _chase;

	[Attack.Subcomponent(true)]
	[SerializeField]
	private Attack _attack;

	[Subcomponent(typeof(Sacrifice))]
	[SerializeField]
	private Sacrifice _sacrifice;

	[Header("Tools")]
	[Space]
	[SerializeField]
	private Collider2D _attackTrigger;

	[SerializeField]
	private CharacterAnimation _characterAnimation;

	[SerializeField]
	private AnimationClip _idleClipAfterWander;

	[SerializeField]
	private AnimationClip _walkClipAfterWander;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _chase, _attack, _sacrifice };
	}

	private void OnDestroy()
	{
		_characterAnimation = null;
		_idleClipAfterWander = null;
		_walkClipAfterWander = null;
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
		((MonoBehaviour)this).StartCoroutine(CProcess());
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
		_characterAnimation.SetWalk(_walkClipAfterWander);
		while (!base.dead)
		{
			if (_sacrifice.result.Equals(Behaviour.Result.Doing))
			{
				yield return null;
				continue;
			}
			if ((Object)(object)FindClosestPlayerBody(_attackTrigger) != (Object)null)
			{
				yield return _attack.CRun(this);
				continue;
			}
			yield return _chase.CRun(this);
			if (_chase.result == Behaviour.Result.Success)
			{
				yield return _attack.CRun(this);
			}
		}
	}
}
