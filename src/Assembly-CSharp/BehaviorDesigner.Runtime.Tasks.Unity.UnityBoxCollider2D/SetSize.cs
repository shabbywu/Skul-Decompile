using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBoxCollider2D;

[TaskCategory("Unity/BoxCollider2D")]
[TaskDescription("Sets the size of the BoxCollider2D. Returns Success.")]
public class SetSize : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[Tooltip("The size of the BoxCollider2D")]
	public SharedVector2 size;

	private BoxCollider2D boxCollider2D;

	private GameObject prevGameObject;

	public override void OnStart()
	{
		GameObject defaultGameObject = ((Task)this).GetDefaultGameObject(((SharedVariable<GameObject>)targetGameObject).Value);
		if ((Object)(object)defaultGameObject != (Object)(object)prevGameObject)
		{
			boxCollider2D = defaultGameObject.GetComponent<BoxCollider2D>();
			prevGameObject = defaultGameObject;
		}
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)boxCollider2D == (Object)null)
		{
			Debug.LogWarning((object)"BoxCollider2D is null");
			return (TaskStatus)1;
		}
		boxCollider2D.size = ((SharedVariable<Vector2>)size).Value;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		targetGameObject = null;
		size = Vector2.zero;
	}
}
