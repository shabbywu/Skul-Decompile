using Characters.Actions;
using UnityEngine;

namespace Characters;

[RequireComponent(typeof(Minion))]
public sealed class Bunshin : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private Minion _minion;

	[SerializeField]
	private Action _onBasicAttack;

	private void OnEnable()
	{
		_minion.onSummon += AttachEvent;
		_minion.onUnsummon += DetachEvent;
	}

	private void AttachEvent(Character owner, Character minion)
	{
		owner.onStartAction += StartAction;
	}

	private void DetachEvent(Character owner, Character minion)
	{
		owner.onStartAction -= StartAction;
	}

	private void StartAction(Action action)
	{
		switch (action.type)
		{
		case Action.Type.BasicAttack:
			if ((Object)(object)_onBasicAttack != (Object)null)
			{
				_onBasicAttack.TryStart();
			}
			break;
		case Action.Type.Dash:
		case Action.Type.JumpAttack:
		case Action.Type.Jump:
		case Action.Type.Skill:
		case Action.Type.Swap:
			break;
		}
	}
}
