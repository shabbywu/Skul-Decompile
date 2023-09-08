using Characters.Actions;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("Assets/Behavior Designer/Icon/CanUseAction.png")]
public class CanUseAction : Conditional
{
	[SerializeField]
	private SharedCharacterAction _action;

	private Action _actionValue;

	public override void OnAwake()
	{
		_actionValue = ((SharedVariable<Action>)_action).Value;
	}

	public override TaskStatus OnUpdate()
	{
		if (_actionValue.canUse)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
