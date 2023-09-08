using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskCategory("Physics")]
[TaskDescription("Returns success when a collision ends. This task will only receive the physics callback if it is being reevaluated (with a conditional abort or under a parallel task).")]
public class HasExitedCollision : Conditional
{
	[Tooltip("The tag of the GameObject to check for a collision against")]
	public SharedString tag = "";

	[Tooltip("The object that exited the collision")]
	public SharedGameObject collidedGameObject;

	private bool exitedCollision;

	public override TaskStatus OnUpdate()
	{
		if (exitedCollision)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	public override void OnEnd()
	{
		exitedCollision = false;
	}

	public override void OnCollisionExit2D(Collision2D collision)
	{
		if (string.IsNullOrEmpty(((SharedVariable<string>)tag).Value) || collision.gameObject.CompareTag(((SharedVariable<string>)tag).Value))
		{
			((SharedVariable<GameObject>)collidedGameObject).Value = collision.gameObject;
			exitedCollision = true;
		}
	}

	public override void OnReset()
	{
		collidedGameObject = null;
	}
}
