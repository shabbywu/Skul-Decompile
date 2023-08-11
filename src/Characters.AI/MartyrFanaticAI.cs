using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public class MartyrFanaticAI : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[Header("Behaviours")]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(Wander))]
	[SerializeField]
	private Wander _wander;

	[SerializeField]
	[Subcomponent(typeof(Chase))]
	private Chase _chase;

	[SerializeField]
	[Subcomponent(typeof(MoveToDestination))]
	private MoveToDestination _moveForSuicide;

	[SerializeField]
	[Attack.Subcomponent(true)]
	private Attack _suicide;

	[Space]
	[SerializeField]
	[Header("Tools")]
	private Collider2D _attackTrigger;

	[SerializeField]
	private AttachAbility _speedBonus;

	[SerializeField]
	private CharacterAnimation _characterAnimation;

	[SerializeField]
	private AnimationClip _idleClipAfterWander;

	[SerializeField]
	private AnimationClip _walkClipAfterWander;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _wander, _chase, _suicide };
		_speedBonus.Initialize();
	}

	private void OnDestroy()
	{
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
			if ((Object)(object)FindClosestPlayerBody(_attackTrigger) != (Object)null)
			{
				yield return CSuicide();
				continue;
			}
			yield return _chase.CRun(this);
			if (_chase.result == Behaviour.Result.Success)
			{
				yield return CSuicide();
			}
		}
	}

	private IEnumerator CSuicide()
	{
		base.destination = Vector2.op_Implicit(((Component)base.target).transform.position);
		_speedBonus.Run(character);
		yield return _moveForSuicide.CRun(this);
		_speedBonus.Stop();
		yield return _suicide.CRun(this);
	}
}
