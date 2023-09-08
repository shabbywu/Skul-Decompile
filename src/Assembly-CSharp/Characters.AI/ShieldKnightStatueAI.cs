using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI;

public sealed class ShieldKnightStatueAI : AIController
{
	[Header("Behaviours")]
	[SerializeField]
	[Attack.Subcomponent(true)]
	private ActionAttack _attack;

	[SerializeField]
	[Header("Guard")]
	private Action _guard;

	[SerializeField]
	private Action _guardEnd;

	[SerializeField]
	[Header("Range")]
	private Collider2D _guardTrigger;

	[SerializeField]
	private Collider2D _attackTrigger;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _attack };
	}

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	protected override void OnDisable()
	{
		base.OnDisable();
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		while (!base.dead)
		{
			yield return Combat();
		}
	}

	private IEnumerator Combat()
	{
		while (!base.dead)
		{
			yield return null;
			if (base.stuned || (Object)(object)base.target == (Object)null || !character.movement.controller.isGrounded)
			{
				continue;
			}
			if ((Object)(object)FindClosestPlayerBody(_guardTrigger) != (Object)null)
			{
				yield return Guard();
			}
			else if ((Object)(object)FindClosestPlayerBody(_attackTrigger) != (Object)null)
			{
				if (_guard.running)
				{
					character.CancelAction();
				}
				yield return _attack.CRun(this);
			}
			else if (_guard.running)
			{
				character.CancelAction();
			}
		}
	}

	private IEnumerator Guard()
	{
		_guard.TryStart();
		while (_guard.running && (Object)(object)FindClosestPlayerBody(_guardTrigger) != (Object)null)
		{
			yield return null;
		}
		_guardEnd.TryStart();
		while (_guardEnd.running)
		{
			yield return null;
		}
	}
}
