using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public class DashSlash : SequentialCombo
{
	[SerializeField]
	[Subcomponent(typeof(ChainAction))]
	private Action _readyAction;

	[Subcomponent(typeof(ChainAction))]
	[SerializeField]
	private Action _attackAction;

	public override IEnumerator CRun(AIController controller)
	{
		_readyAction.TryStart();
		while (_readyAction.running)
		{
			yield return null;
		}
		_attackAction.TryStart();
		while (_attackAction.running)
		{
			yield return null;
		}
	}
}
