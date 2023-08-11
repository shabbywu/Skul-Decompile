using Characters;
using Services;
using Singletons;
using UnityEngine;

namespace BT.Conditions;

public class PlayerInRange : Condition
{
	[SerializeField]
	private float _distance;

	protected override bool Check(Context context)
	{
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		Transform val = context.Get<Transform>(Key.OwnerTransform);
		Character player = Singleton<Service>.Instance.levelManager.player;
		if ((Object)(object)val == (Object)null || (Object)(object)player == (Object)null)
		{
			return false;
		}
		return Vector2.Distance(Vector2.op_Implicit(((Component)player).transform.position), Vector2.op_Implicit(val.position)) < _distance;
	}
}
