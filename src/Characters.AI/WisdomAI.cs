using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class WisdomAI : AIController
{
	[Subcomponent(typeof(CheckWithinSight))]
	[SerializeField]
	[Header("Behaviours")]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _attack;

	[SerializeField]
	[Space]
	[Header("Tools")]
	private Collider2D _attackTrigger;

	private void Awake()
	{
		character.status.unstoppable.Attach((object)this);
		base.behaviours = new List<Behaviour> { _checkWithinSight, _attack };
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
		while (!base.dead)
		{
			yield return null;
			if (!((Object)(object)base.target == (Object)null) && _attack.CanUse() && (Object)(object)FindClosestPlayerBody(_attackTrigger) != (Object)null)
			{
				yield return _attack.CRun(this);
			}
		}
	}
}
