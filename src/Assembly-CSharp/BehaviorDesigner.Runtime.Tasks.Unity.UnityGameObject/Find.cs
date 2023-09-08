using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityGameObject;

[TaskDescription("Finds a GameObject by name. Returns success if an object is found.")]
[TaskCategory("Unity/GameObject")]
public class Find : Action
{
	[Tooltip("The GameObject name to find")]
	public SharedString gameObjectName;

	[RequiredField]
	[Tooltip("The object found by name")]
	public SharedGameObject storeValue;

	public override TaskStatus OnUpdate()
	{
		((SharedVariable<GameObject>)storeValue).Value = GameObject.Find(((SharedVariable<string>)gameObjectName).Value);
		if ((Object)(object)((SharedVariable<GameObject>)storeValue).Value != (Object)null)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnReset()
	{
		gameObjectName = null;
		storeValue = null;
	}
}
