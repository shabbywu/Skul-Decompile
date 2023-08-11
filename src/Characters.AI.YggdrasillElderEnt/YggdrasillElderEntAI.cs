using System.Collections;
using Characters.AI.Behaviours;
using Runnables;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt;

public class YggdrasillElderEntAI : AIController
{
	[Behaviour.Subcomponent(true)]
	[SerializeField]
	private Behaviour _behaviours;

	[Behaviour.Subcomponent(true)]
	[SerializeField]
	private Behaviour _phase2Sequence;

	[SerializeField]
	private Runnable _onPhase2;

	private void Awake()
	{
		((MonoBehaviour)this).StartCoroutine(CProcess());
		character.health.onDie += OnDie;
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return _behaviours.CRun(this);
	}

	private void OnDie()
	{
		character.health.onDie -= OnDie;
		character.status.unstoppable.Attach((object)this);
		character.health.Heal(0.009999999776482582, notify: false);
		_onPhase2.Run();
		StopAllCoroutinesWithBehaviour();
		((MonoBehaviour)this).StartCoroutine(CReserveCleansing());
		((MonoBehaviour)this).StartCoroutine(_phase2Sequence.CRun(this));
	}

	private IEnumerator CReserveCleansing()
	{
		yield return null;
		character.status.RemoveAllStatus();
		character.status.unstoppable.Detach((object)this);
	}

	public void Update2PhaseFreezeEffect()
	{
	}
}
