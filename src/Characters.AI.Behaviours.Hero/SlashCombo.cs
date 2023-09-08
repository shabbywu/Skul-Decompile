using System.Collections;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public class SlashCombo : Behaviour
{
	[UnityEditor.Subcomponent(typeof(ChainAction))]
	[SerializeField]
	private Action _readyAction;

	[UnityEditor.Subcomponent(typeof(ChainAction))]
	[SerializeField]
	private Action _attackAction;

	[UnityEditor.Subcomponent(typeof(ChainAction))]
	[SerializeField]
	private Action _fall;

	[UnityEditor.Subcomponent(typeof(ChainAction))]
	[SerializeField]
	private Action _endAction;

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
		_fall.TryStart();
		while (_fall.running)
		{
			yield return null;
		}
		_endAction.TryStart();
		while (_endAction.running)
		{
			yield return null;
		}
	}
}
