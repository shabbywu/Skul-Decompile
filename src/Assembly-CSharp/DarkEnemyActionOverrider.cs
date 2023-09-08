using System;
using BehaviorDesigner.Runtime;
using Characters.Actions;
using UnityEngine;

public class DarkEnemyActionOverrider : MonoBehaviour
{
	[Serializable]
	private class DarkEnemyAction
	{
		[SerializeField]
		private string _variableName;

		[SerializeField]
		private Characters.Actions.Action _action;

		public string variableName => _variableName;

		public Characters.Actions.Action action => _action;
	}

	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private DarkEnemyAction[] _darkEnemyActions;

	private void Start()
	{
		DarkEnemyAction[] darkEnemyActions = _darkEnemyActions;
		foreach (DarkEnemyAction darkEnemyAction in darkEnemyActions)
		{
			_communicator.SetVariable<SharedCharacterAction>(darkEnemyAction.variableName, (object)darkEnemyAction.action);
		}
	}
}
