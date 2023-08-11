using System.Collections;
using System.Collections.Generic;
using Characters.AI.Behaviours;
using Characters.AI.Behaviours.Attacks;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public sealed class BrothersAI : AIController
{
	[SerializeField]
	private float _lifeTime = 30f;

	[SerializeField]
	private bool _idleOnStart;

	[Subcomponent(typeof(CheckWithinSight))]
	[Header("Behaviours")]
	[SerializeField]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _intro;

	[Subcomponent(typeof(ActionAttack))]
	[SerializeField]
	private ActionAttack _outro;

	[Attack.Subcomponent(true)]
	[SerializeField]
	private Attack _attack;

	[SerializeField]
	[Behaviour.Subcomponent(true)]
	private Idle _idle;

	private bool _readyForOutro;

	private void Awake()
	{
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
		((MonoBehaviour)this).StartCoroutine(COutro());
		yield return CCombat();
	}

	private IEnumerator CCombat()
	{
		yield return _intro.CRun(this);
		if (_idleOnStart)
		{
			yield return _idle.CRun(this);
		}
		while (!base.dead)
		{
			if (_readyForOutro)
			{
				yield return _outro.CRun(this);
				Object.Destroy((Object)(object)((Component)character).gameObject);
			}
			if ((Object)(object)base.target == (Object)null)
			{
				yield return null;
			}
			else if (base.stuned)
			{
				yield return null;
			}
			else
			{
				yield return _attack.CRun(this);
			}
		}
	}

	private IEnumerator COutro()
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)character.chronometer.master, _lifeTime);
		_readyForOutro = true;
	}
}
