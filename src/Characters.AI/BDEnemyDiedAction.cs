using System.Collections;
using BehaviorDesigner.Runtime;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI;

public class BDEnemyDiedAction : MonoBehaviour
{
	[SerializeField]
	private BehaviorTree _behaviorTree;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private Action _dieAction;

	[SerializeField]
	private Action _diedAction;

	private bool _run;

	private void Start()
	{
		if (!((Object)(object)_behaviorTree == (Object)null))
		{
			_character.health.onDiedTryCatch += OnDied;
		}
	}

	private void OnDied()
	{
		if (!_run)
		{
			_run = true;
			ActiveCharacterSprite();
			((MonoBehaviour)_behaviorTree).StopAllCoroutines();
			((Behavior)_behaviorTree).StopAllTaskCoroutines();
			((Behaviour)_behaviorTree).enabled = false;
			if ((Object)(object)_dieAction != (Object)null)
			{
				((MonoBehaviour)this).StartCoroutine(PlayDieAction());
			}
			_character.health.onDiedTryCatch -= OnDied;
		}
	}

	private void ActiveCharacterSprite()
	{
		((Behaviour)_character.collider).enabled = false;
		((Component)_character).gameObject.SetActive(true);
	}

	private IEnumerator PlayDieAction()
	{
		bool flag = _dieAction.TryStart();
		while (!flag)
		{
			yield return null;
			flag = _dieAction.TryStart();
		}
		if ((Object)(object)_diedAction != (Object)null)
		{
			((MonoBehaviour)this).StartCoroutine(PlayDiedAction());
		}
	}

	private IEnumerator PlayDiedAction()
	{
		while (_dieAction.running)
		{
			yield return null;
		}
		_diedAction.TryStart();
	}
}
