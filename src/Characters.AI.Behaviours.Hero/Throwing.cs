using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public class Throwing : Behaviour
{
	[UnityEditor.Subcomponent(typeof(ChainAction))]
	[SerializeField]
	private Action _attackAction;

	public override IEnumerator CRun(AIController controller)
	{
		_attackAction.TryStart();
		while (_attackAction.running)
		{
			yield return null;
		}
	}
}
