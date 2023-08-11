using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityString;

[TaskDescription("Global SharedVariable이 Null 인지 체크")]
[TaskIcon("Assets/Behavior Designer/Icon/IsNullSharedVariable.png")]
public class IsNullSharedVariable : Conditional
{
	[SerializeField]
	private SharedVariable _variable;

	public override TaskStatus OnUpdate()
	{
		if (_variable.GetValue() == null)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
