using UnityEngine;
using Utils;

namespace Characters.Operations.SetPosition;

public class ToOwner : Policy
{
	[SerializeField]
	private PositionInfo _positionInfo = new PositionInfo(attach: false, layerOnly: false, 0, PositionInfo.Pivot.Bottom);

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		return _positionInfo.GetPosition(owner);
	}

	public override Vector2 GetPosition()
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		Debug.LogError((object)"Invalid onwer");
		return Vector2.op_Implicit(((Component)this).transform.position);
	}
}
