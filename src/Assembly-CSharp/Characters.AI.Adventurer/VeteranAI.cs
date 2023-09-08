using System.Collections;
using Characters.AI.Behaviours;
using UnityEngine;

namespace Characters.AI.Adventurer;

public sealed class VeteranAI : AIController
{
	[Behaviour.Subcomponent(true)]
	[SerializeField]
	private Behaviour _behaviours;

	public void StartCombat()
	{
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return _behaviours.CRun(this);
	}
}
