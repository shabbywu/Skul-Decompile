using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;

[TaskDescription("Stores the forward vector of the Transform. Returns Success.")]
[TaskCategory("Unity/Transform")]
public class GetForwardVector : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[RequiredField]
	[Tooltip("The position of the Transform")]
	public SharedVector3 storeValue;

	private Transform targetTransform;

	private GameObject prevGameObject;

	public override void OnStart()
	{
		GameObject defaultGameObject = ((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value);
		if ((Object)(object)defaultGameObject != (Object)(object)prevGameObject)
		{
			targetTransform = defaultGameObject.GetComponent<Transform>();
			prevGameObject = defaultGameObject;
		}
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)targetTransform == (Object)null)
		{
			Debug.LogWarning((object)"Transform is null");
			return (TaskStatus)1;
		}
		((SharedVariable<Vector3>)storeValue).Value = targetTransform.forward;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		targetGameObject = null;
		storeValue = Vector3.zero;
	}
}
