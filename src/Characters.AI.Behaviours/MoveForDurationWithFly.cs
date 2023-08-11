using System.Collections;
using Level;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class MoveForDurationWithFly : Move
{
	[MinMaxSlider(0f, 10f)]
	[SerializeField]
	private Vector2 _duration;

	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		base.result = Result.Doing;
		((MonoBehaviour)this).StartCoroutine(CExpire(controller, _duration));
		_ = direction;
		Bounds bounds = Map.Instance.bounds;
		while (base.result == Result.Doing)
		{
			yield return null;
			character.movement.move = direction;
			ChangeDirectionIfBlocked(character, bounds);
		}
		idle.CRun(controller);
	}

	private void ChangeDirectionIfBlocked(Character character, Bounds bounds)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		if (((Component)character).transform.position.x + direction.x < ((Bounds)(ref bounds)).min.x && character.lookingDirection == Character.LookingDirection.Left)
		{
			direction.x = 1f;
		}
		else if (((Component)character).transform.position.x + direction.x > ((Bounds)(ref bounds)).max.x && character.lookingDirection == Character.LookingDirection.Right)
		{
			direction.x = -1f;
		}
		if (((Component)character).transform.position.y + direction.y < ((Bounds)(ref bounds)).min.y)
		{
			direction.y = 1f;
		}
		else if (((Component)character).transform.position.y + direction.y > ((Bounds)(ref bounds)).max.y)
		{
			direction.y = -1f;
		}
	}
}
