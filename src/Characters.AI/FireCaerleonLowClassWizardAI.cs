using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class FireCaerleonLowClassWizardAI : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[Attack.Subcomponent(true)]
	[SerializeField]
	private CircularProjectileAttack _circularProjectileAttack;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _essentialIdleAfterAttack;

	[SerializeField]
	[Subcomponent(typeof(ChaseTeleport))]
	private ChaseTeleport _chaseTeleport;

	[SerializeField]
	[Subcomponent(typeof(Teleport))]
	private Teleport _escapeTeleport;

	[SerializeField]
	private Collider2D _attackCollider;

	[SerializeField]
	private bool _noEscape;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _circularProjectileAttack, _essentialIdleAfterAttack, _chaseTeleport, _escapeTeleport };
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
		yield return Combat();
	}

	private IEnumerator Combat()
	{
		while (!base.dead)
		{
			yield return null;
			if (!((Object)(object)base.target == (Object)null) && _escapeTeleport.result != Behaviour.Result.Doing && _chaseTeleport.result != Behaviour.Result.Doing)
			{
				if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_attackCollider)))
				{
					yield return _circularProjectileAttack.CRun(this);
					yield return _essentialIdleAfterAttack.CRun(this);
				}
				else if (base.target.movement.controller.isGrounded && !_noEscape && _chaseTeleport.CanUse())
				{
					yield return _chaseTeleport.CRun(this);
				}
			}
		}
	}
}
