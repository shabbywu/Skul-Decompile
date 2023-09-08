using Services;
using Singletons;
using UnityEngine;

namespace Characters.Operations.Customs;

public class AimPlayer : CharacterOperation
{
	[SerializeField]
	private Transform _centerAxis;

	[SerializeField]
	private bool _platform;

	public override void Run(Character owner)
	{
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		//IL_007f: Unknown result type (might be due to invalid IL or missing references)
		//IL_008f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0094: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_009a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		Character player = Singleton<Service>.Instance.levelManager.player;
		Bounds bounds;
		float num;
		if (_platform)
		{
			bounds = player.movement.controller.collisionState.lastStandingCollider.bounds;
			num = ((Bounds)(ref bounds)).max.y;
		}
		else
		{
			float y = ((Component)player).transform.position.y;
			bounds = ((Collider2D)player.collider).bounds;
			num = y + ((Bounds)(ref bounds)).extents.y;
		}
		Vector3 val = new Vector3(((Component)player).transform.position.x, num) - ((Component)_centerAxis).transform.position;
		float num2 = Mathf.Atan2(val.y, val.x) * 57.29578f;
		_centerAxis.rotation = Quaternion.Euler(0f, 0f, num2);
	}
}
