using System.Collections;
using Characters.AI.Behaviours;
using UnityEngine;

namespace Characters.AI;

public sealed class SimpleAI : AIController
{
	[Behaviour.Subcomponent(true)]
	[SerializeField]
	private Behaviour _behaviours;

	private new void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CProcess());
	}

	protected override IEnumerator CProcess()
	{
		yield return CPlayStartOption();
		yield return _behaviours.CRun(this);
	}
}
