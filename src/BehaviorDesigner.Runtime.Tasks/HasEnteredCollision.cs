using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Physics")]
[TaskDescription("Returns success when a collision starts. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
public class HasEnteredCollision : Conditional
{
	[Tooltip("The tag of the GameObject to check for a collision against")]
	public SharedString tag = "";

	[Tooltip("The object that started the collision")]
	public SharedGameObject collidedGameObject;

	private bool enteredCollision;

	public override TaskStatus OnUpdate()
	{
		if (enteredCollision)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnEnd()
	{
		enteredCollision = false;
	}

	public override void OnCollisionEnter2D(Collision2D collision)
	{
		if (string.IsNullOrEmpty(((SharedVariable<string>)tag).Value) || collision.gameObject.CompareTag(((SharedVariable<string>)tag).Value))
		{
			((SharedVariable<GameObject>)collidedGameObject).Value = collision.gameObject;
			enteredCollision = true;
		}
	}

	public override void OnReset()
	{
		tag = "";
		collidedGameObject = null;
	}
}
