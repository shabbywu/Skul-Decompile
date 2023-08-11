using System.Collections;
using Characters.AI.Hero;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public abstract class SequentialCombo : Behaviour, IComboable, IEntryCombo
{
	[SerializeField]
	[Subcomponent(typeof(ChainAction))]
	private Action _startAction;

	[SerializeField]
	[Subcomponent(typeof(ChainAction))]
	private Action _endAction;

	public IEnumerator CTryContinuedCombo(AIController controller, ComboSystem comboSystem)
	{
		yield return CRun(controller);
		if (comboSystem.TryComboAttack())
		{
			yield return comboSystem.CNext(controller);
			yield break;
		}
		comboSystem.Clear();
		if (_endAction.TryStart())
		{
			while (_endAction.running)
			{
				yield return null;
			}
		}
	}

	public IEnumerator CTryEntryCombo(AIController controller, ComboSystem comboSystem)
	{
		_startAction.TryStart();
		while (_startAction.running)
		{
			yield return null;
		}
		yield return CTryContinuedCombo(controller, comboSystem);
	}
}
