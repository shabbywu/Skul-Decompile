using System.Collections;
using Characters.AI.Behaviours;
using UnityEngine;

namespace Characters.AI.Hero;

public class FirstHeroAIPhase1 : AIController
{
	[SerializeField]
	[Behaviour.Subcomponent(true)]
	private Behaviour _behaviours;

	private new void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return Chronometer.global.WaitForSeconds(1f);
		yield return _behaviours.CRun(this);
	}
}
