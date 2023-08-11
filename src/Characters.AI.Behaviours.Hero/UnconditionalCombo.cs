using System.Collections;
using Characters.AI.Hero;
using Characters.Actions;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Behaviours.Hero;

public abstract class UnconditionalCombo : Behaviour, IComboable, IEntryCombo
{
	[SerializeField]
	[Subcomponent(typeof(ChainAction))]
	private Action _startAction;

	public IEnumerator CTryContinuedCombo(AIController controller, ComboSystem comboSystem)
	{
		yield return CRun(controller);
		yield return comboSystem.CNext(controller);
	}

	public IEnumerator CTryEntryCombo(AIController controller, ComboSystem comboSystem)
	{
		_startAction.TryStart();
		while (_startAction.running)
		{
			yield return null;
		}
		comboSystem.Start();
		yield return CTryContinuedCombo(controller, comboSystem);
	}
}
