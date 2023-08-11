using System;
using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class BehaviourInfo : MonoBehaviour
{
	[Serializable]
	internal class Subcomponents : SubcomponentArray<BehaviourInfo>
	{
	}

	[Behaviour.Subcomponent(true)]
	[SerializeField]
	private Behaviour _behaviour;

	[SerializeField]
	private string _tag;

	public Behaviour.Result result => _behaviour.result;

	public IEnumerator CRun(AIController controller)
	{
		yield return _behaviour.CRun(controller);
	}

	public override string ToString()
	{
		if (_tag != null && _tag.Length != 0)
		{
			return _tag;
		}
		return ExtensionMethods.GetAutoName((object)this);
	}
}
