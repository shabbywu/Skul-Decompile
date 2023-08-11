using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityTransform;

[TaskCategory("Unity/Transform")]
[TaskDescription("Finds a transform by name. Returns success if an object is found.")]
public class Find : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The transform name to find")]
	public SharedString transformName;

	[RequiredField]
	[Tooltip("The object found by name")]
	public SharedTransform storeValue;

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
		if ((Object)(object)targetTransform == (Object)null)
		{
			Debug.LogWarning((object)"Transform is null");
			return (TaskStatus)1;
		}
		((SharedVariable<Transform>)storeValue).Value = targetTransform.Find(((SharedVariable<string>)transformName).Value);
		if ((Object)(object)((SharedVariable<Transform>)storeValue).Value != (Object)null)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnReset()
	{
		targetGameObject = null;
		transformName = null;
		storeValue = null;
	}
}
