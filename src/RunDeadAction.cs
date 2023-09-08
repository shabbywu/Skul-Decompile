using System.Collections;
using BehaviorDesigner.Runtime;
using Characters;
using Characters.Actions;
using Characters.Operations;
using Runnables;
using UnityEngine;

public sealed class RunDeadAction : Runnable
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private BehaviorTree _behaviorTree;

	[SerializeField]
	private Action _deadAction;

	[SerializeField]
	private OperationInfos _whenFailToDeadAction;

	[SerializeField]
	private bool _disableOnActionEnd = true;

	public override void Run()
	{
		((Component)_character).gameObject.SetActive(true);
		((Behaviour)_character.collider).enabled = false;
		if ((Object)(object)_behaviorTree != (Object)null)
		{
			((Behaviour)_behaviorTree).enabled = false;
		}
		_character.status.RemoveAllStatus();
		_character.invulnerable.Attach(this);
		_character.CancelAction();
		if (_deadAction.TryStart())
		{
			if (_disableOnActionEnd)
			{
				((MonoBehaviour)this).StartCoroutine(CDisableOnActionEnd());
			}
		}
		else if (!((Object)(object)_whenFailToDeadAction == (Object)null))
		{
			_whenFailToDeadAction.Initialize();
			_whenFailToDeadAction.Run(_character);
		}
	}

	private IEnumerator CDisableOnActionEnd()
	{
		while (_deadAction.running)
		{
			yield return null;
		}
		((Component)_character).gameObject.SetActive(false);
	}
}
