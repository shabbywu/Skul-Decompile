using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class IceCaerleonLowClassWizard : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[Attack.Subcomponent(true)]
	[SerializeField]
	private ActionAttack _icestom;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _essentialIdleAfterAttack;

	[Subcomponent(typeof(ChaseTeleport))]
	[SerializeField]
	private ChaseTeleport _chaseTeleport;

	[SerializeField]
	private Collider2D _attackCollider;

	[SerializeField]
	private Collider2D _escapeTrigger;

	[SerializeField]
	private bool _noEscape;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _icestom, _essentialIdleAfterAttack, _chaseTeleport };
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
			if (!((Object)(object)base.target == (Object)null) && character.movement.controller.isGrounded && _chaseTeleport.result != Behaviour.Result.Doing)
			{
				if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_escapeTrigger)))
				{
					yield return _icestom.CRun(this);
				}
				else if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_attackCollider)))
				{
					yield return _icestom.CRun(this);
				}
				else if (base.target.movement.controller.isGrounded && !_noEscape && _chaseTeleport.CanUse())
				{
					yield return _chaseTeleport.CRun(this);
				}
			}
		}
	}
}
