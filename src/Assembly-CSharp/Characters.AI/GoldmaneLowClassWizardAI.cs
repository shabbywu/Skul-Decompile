using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class GoldmaneLowClassWizardAI : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	private CheckWithinSight _checkWithinSight;

	[SerializeField]
	[Subcomponent(typeof(CircularProjectileAttack))]
	private CircularProjectileAttack _attack;

	[SerializeField]
	[Subcomponent(typeof(ChaseTeleport))]
	private ChaseTeleport _chaseTeleport;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _essentialIdleAfterAttack;

	[SerializeField]
	private Collider2D _attackCollider;

	[SerializeField]
	private bool _noEscape;

	private void Awake()
	{
		base.behaviours = new List<Behaviour> { _checkWithinSight, _attack, _essentialIdleAfterAttack, _chaseTeleport };
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
			if (!((Object)(object)base.target == (Object)null) && character.movement.controller.isGrounded && _chaseTeleport.result != Behaviour.Result.Doing)
			{
				if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_attackCollider)))
				{
					yield return _attack.CRun(this);
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
