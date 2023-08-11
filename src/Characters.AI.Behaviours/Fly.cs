using System.Collections;
using UnityEngine;

namespace Characters.AI.Behaviours;

public class Fly : Move
{
	[MinMaxSlider(0f, 10f)]
	[SerializeField]
	private Vector2 _duration;

	[SerializeField]
	private Collider2D _flyableZoneCollider;

	private Bounds _flyableZone;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_flyableZone = _flyableZoneCollider.bounds;
	}

	public override IEnumerator CRun(AIController controller)
	{
		Character character = controller.character;
		base.result = Result.Doing;
		((MonoBehaviour)this).StartCoroutine(CExpire(controller, _duration));
		if (wander)
		{
			direction = Vector2.op_Implicit(Random.insideUnitSphere);
		}
		while (base.result == Result.Doing)
		{
			yield return null;
			character.movement.move = direction;
			if (Mathf.Abs(((Bounds)(ref _flyableZone)).min.x - ((Component)character).transform.position.x) < 1f && direction.x < 0f)
			{
				((Vector2)(ref direction)).Set(0f - direction.x, direction.y);
			}
			if (Mathf.Abs(((Bounds)(ref _flyableZone)).max.x - ((Component)character).transform.position.x) < 1f && direction.x > 0f)
			{
				((Vector2)(ref direction)).Set(0f - direction.x, direction.y);
			}
			if (Mathf.Abs(((Bounds)(ref _flyableZone)).min.y - ((Component)character).transform.position.y) < 1f && direction.y < 0f)
			{
				((Vector2)(ref direction)).Set(direction.x, 0f - direction.y);
			}
			if (Mathf.Abs(((Bounds)(ref _flyableZone)).max.y - ((Component)character).transform.position.y) < 1f && direction.y > 0f)
			{
				((Vector2)(ref direction)).Set(direction.x, 0f - direction.y);
			}
		}
		idle.CRun(controller);
	}
}
