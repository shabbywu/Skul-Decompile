using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D;

[TaskCategory("Unity/Rigidbody2D")]
[TaskDescription("Sets the is kinematic value of the Rigidbody2D. Returns Success.")]
public class SetIsKinematic : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The is kinematic value of the Rigidbody2D")]
	public SharedBool isKinematic;

	private Rigidbody2D rigidbody2D;

	private GameObject prevGameObject;

	public override void OnStart()
	{
		GameObject defaultGameObject = ((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value);
		if ((Object)(object)defaultGameObject != (Object)(object)prevGameObject)
		{
			rigidbody2D = defaultGameObject.GetComponent<Rigidbody2D>();
			prevGameObject = defaultGameObject;
		}
	}

	public override TaskStatus OnUpdate()
	{
		if ((Object)(object)rigidbody2D == (Object)null)
		{
			Debug.LogWarning((object)"Rigidbody2D is null");
			return (TaskStatus)1;
		}
		rigidbody2D.isKinematic = ((SharedVariable<bool>)isKinematic).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetGameObject = null;
		isKinematic = false;
	}
}
