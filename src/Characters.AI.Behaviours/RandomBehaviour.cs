using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class RandomBehaviour : Behaviour
{
	[Subcomponent(typeof(BehaviourInfo))]
	[SerializeField]
	private BehaviourInfo.Subcomponents _behaviours;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		if (_behaviours == null)
		{
			Debug.LogError((object)"Behaviours length is null");
			yield break;
		}
		if (((SubcomponentArray<BehaviourInfo>)_behaviours).components.Length == 0)
		{
			Debug.LogError((object)"Behaviours length is 0");
			yield break;
		}
		BehaviourInfo behaviour = ExtensionMethods.Random<BehaviourInfo>((IEnumerable<BehaviourInfo>)((SubcomponentArray<BehaviourInfo>)_behaviours).components);
		yield return behaviour.CRun(controller);
		base.result = behaviour.result;
	}
}
