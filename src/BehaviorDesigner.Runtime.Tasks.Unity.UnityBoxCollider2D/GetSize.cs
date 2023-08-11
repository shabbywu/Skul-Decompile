using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Unity.UnityBoxCollider2D;

[TaskDescription("Stores the size of the BoxCollider2D. Returns Success.")]
[TaskCategory("Unity/BoxCollider2D")]
public class GetSize : Action
{
	[Tooltip("The GameObject that the task operates on. If null the task GameObject is used.")]
	public SharedGameObject targetGameObject;

	[RequiredField]
	[Tooltip("The size of the BoxCollider2D")]
	public SharedVector2 storeValue;

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
		((SharedVariable<Vector2>)storeValue).Value = boxCollider2D.size;
		return (TaskStatus)2;
	}

	public override void OnReset()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		targetGameObject = null;
		storeValue = Vector2.zero;
	}
}
