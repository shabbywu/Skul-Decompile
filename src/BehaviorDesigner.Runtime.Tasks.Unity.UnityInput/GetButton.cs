using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityInput;

[TaskCategory("Unity/Input")]
[TaskDescription("Stores the state of the specified button.")]
public class GetButton : Action
{
	[Tooltip("The name of the button")]
	public SharedString buttonName;

	[Tooltip("The stored result")]
	[RequiredField]
	public SharedBool storeResult;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<bool>)storeResult).Value = Input.GetButton(((SharedVariable<string>)buttonName).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		buttonName = "Fire1";
		storeResult = false;
	}
}
