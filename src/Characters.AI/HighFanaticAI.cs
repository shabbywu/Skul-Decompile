using System.Collections;
using Characters.AI.Behaviours;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public class HighFanaticAI : AIController
{
	[SerializeField]
	[Subcomponent(typeof(CheckWithinSight))]
	[Header("Behaviours")]
	private CheckWithinSight _checkWithinSight;

	[Subcomponent(typeof(KeepDistance))]
	[SerializeField]
	private KeepDistance _keepDistance;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _keepDistanceIdle;

	[SerializeField]
	[Chase.Subcomponent(true)]
	private Chase _chase;

	[SerializeField]
	[Subcomponent(typeof(Wander))]
	private Wander _wander;

	[Header("Fanatic Call", order = 2)]
	[Subcomponent(typeof(FanaticCall))]
	[Space]
	[SerializeField]
	private FanaticCall _fanaticCall;

	[SerializeField]
	[Subcomponent(typeof(Idle))]
	private Idle _fanaticCallIdle;

	[Header("Mass Sacrifice", order = 3)]
	[Subcomponent(typeof(MassSacrifice))]
	[SerializeField]
	private MassSacrifice _massSacrifice;

	[Subcomponent(typeof(Idle))]
	[SerializeField]
	private Idle _massSacrificeIdle;

	[SerializeField]
	[Header("Tools")]
	private Collider2D _fanaticCallTrigger;

	[SerializeField]
	private Collider2D _massSacrificeTrigger;

	[SerializeField]
	private Collider2D _keepDistanceTrigger;

	protected override void OnEnable()
	{
		base.OnEnable();
		((MonoBehaviour)this).StartCoroutine(CProcess());
		((MonoBehaviour)this).StartCoroutine(_checkWithinSight.CRun(this));
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return _wander.CRun(this);
		yield return Combat();
	}

	private IEnumerator Combat()
	{
		while (!base.dead)
		{
			yield return null;
			if (!((Object)(object)base.target == (Object)null))
			{
				if (_fanaticCall.CanUse() && Object.op_Implicit((Object)(object)FindClosestPlayerBody(_fanaticCallTrigger)))
				{
					yield return _fanaticCall.CRun(this);
					yield return _fanaticCallIdle.CRun(this);
				}
				else if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_massSacrificeTrigger)) && _massSacrifice.CanUse(this))
				{
					yield return _massSacrifice.CRun(this);
				}
				else if (Object.op_Implicit((Object)(object)FindClosestPlayerBody(_keepDistanceTrigger)) && _keepDistance.CanUseBackMove())
				{
					yield return _keepDistance.CRun(this);
					yield return _keepDistanceIdle.CRun(this);
				}
				else
				{
					yield return _chase.CRun(this);
				}
			}
		}
	}
}
