using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityCircleCollider2D;

[TaskDescription("Sets the offset of the CircleCollider2D. Returns Success.")]
[TaskCategory("Unity/CircleCollider2D")]
public class SetOffset : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The offset of the CircleCollider2D")]
	public SharedVector3 offset;

	private CircleCollider2D circleCollider2D;

	private GameObject prevGameObject;

	public override void OnStart()
	{
		GameObject defaultGameObject = ((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value);
		if ((Object)(object)defaultGameObject != (Object)(object)prevGameObject)
		{
			circleCollider2D = defaultGameObject.GetComponent<CircleCollider2D>();
			prevGameObject = defaultGameObject;
		}
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)circleCollider2D == (Object)null)
		{
			Debug.LogWarning((object)"CircleCollider2D is null");
			return (TaskStatus)1;
		}
		((Collider2D)circleCollider2D).offset = Vector2.op_Implicit(((SharedVariable<Vector3>)offset).Value);
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		targetGameObject = null;
		offset = Vector3.zero;
	}
}
