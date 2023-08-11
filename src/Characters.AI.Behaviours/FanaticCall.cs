using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.AI.Behaviours;

public sealed class FanaticCall : Behaviour
{
	[SerializeField]
	private Transform _spawnPositionParent;

	[SerializeField]
	private Action _action;

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		SetSpawnPoint(controller.target);
		if (!_action.TryStart())
		{
			base.result = Result.Fail;
			yield break;
		}
		while (_action.running)
		{
			yield return null;
		}
		base.result = Result.Success;
	}

	private void SetSpawnPoint(Character target)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = target.movement.controller.collisionState.lastStandingCollider.bounds;
		foreach (Transform item in _spawnPositionParent)
		{
			float num = Random.Range(((Bounds)(ref bounds)).min.x, ((Bounds)(ref bounds)).max.x);
			float y = ((Bounds)(ref bounds)).max.y;
			item.position = Vector2.op_Implicit(new Vector2(num, y));
		}
	}

	public bool CanUse()
	{
		return _action.canUse;
	}
}
