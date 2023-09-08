using Characters;
using Characters.Actions;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("{SkinColor}StackedActionIcon.png")]
[TaskDescription("Allows multiple action tasks to be added to a single node.")]
public sealed class RunCharacterActions : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private Action[] _actions;

	[SerializeField]
	private bool _tryUntilSucceed;

	private bool _running;

	private bool _trying;

	private int _index;

	private Character _ownerValue;

	public override void OnAwake()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
	}

	public override void OnStart()
	{
		if (_actions != null && _actions.Length != 0 && !_ownerValue.stunedOrFreezed)
		{
			_trying = _actions[_index].TryStart();
			_running = true;
		}
	}

	public override TaskStatus OnUpdate()
	{
		Action action = _actions[_index];
		if (!_running)
		{
			if (!_ownerValue.stunedOrFreezed)
			{
				_running = true;
				_index++;
				_trying = action.TryStart();
				return (TaskStatus)3;
			}
			return (TaskStatus)3;
		}
		if (_tryUntilSucceed && !_trying)
		{
			_trying = action.TryStart();
			return (TaskStatus)3;
		}
		if (!action.running)
		{
			if (_index < _actions.Length)
			{
				return (TaskStatus)3;
			}
			return (TaskStatus)2;
		}
		return (TaskStatus)3;
	}

	public override void OnEnd()
	{
		((Task)this).OnEnd();
		_running = false;
		_trying = false;
		_index = 0;
	}
}
