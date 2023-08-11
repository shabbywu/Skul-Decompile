using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput;

[TaskDescription("Returns success when the specified mouse button is pressed.")]
[TaskCategory("Unity/Input")]
public class IsMouseDown : Conditional
{
	[Tooltip("The button index")]
	public SharedInt buttonIndex;

	public override TaskStatus OnUpdate()
	{
		if (Input.GetMouseButtonDown(((SharedVariable<int>)buttonIndex).Value))
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnReset()
	{
		buttonIndex = 0;
	}
}
