using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityLayerMask;

[TaskDescription("Gets the layer of a GameObject.")]
[TaskCategory("Unity/LayerMask")]
public class GetLayer : Action
{
	[Tooltip("The GameObject to set the layer of")]
	public SharedGameObject targetGameObject;

	[Tooltip("The name of the layer to get")]
	[RequiredField]
	public SharedString storeResult;

	public override TaskStatus OnUpdate()
	{
		GameObject defaultGameObject = ((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value);
		((SharedVariable<string>)storeResult).Value = LayerMask.LayerToName(defaultGameObject.layer);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetGameObject = null;
		storeResult = "";
	}
}
