using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;

[TaskDescription("Returns the component of Type type if the game object has one attached, null if it doesn't. Returns Success.")]
[TaskCategory("Unity/GameObject")]
public class GetComponent : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The type of component")]
	public SharedString type;

	[RequiredField]
	[Tooltip("The component")]
	public SharedObject storeValue;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<Object>)storeValue).Value = (Object)(object)((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value).GetComponent(((SharedVariable<string>)type).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetGameObject = null;
		((SharedVariable<string>)type).Value = "";
		((SharedVariable<Object>)storeValue).Value = null;
	}
}
