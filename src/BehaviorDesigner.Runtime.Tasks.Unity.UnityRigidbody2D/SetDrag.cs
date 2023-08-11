using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityRigidbody2D;

[TaskDescription("Sets the drag of the Rigidbody2D. Returns Success.")]
[TaskCategory("Unity/Rigidbody2D")]
public class SetDrag : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The drag of the Rigidbody2D")]
	public SharedFloat drag;

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
		rigidbody2D.drag = ((SharedVariable<float>)drag).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		targetGameObject = null;
		drag = 0f;
	}
}