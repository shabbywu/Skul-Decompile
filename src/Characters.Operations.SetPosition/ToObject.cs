using Characters.Utils;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToObject : Policy
{
	[SerializeField]
	private LayerMask _groundMask = Layers.groundMask;

	[SerializeField]
	private GameObject _object;

	[SerializeField]
	private bool _onPlatform;

	private static NonAllocCaster _belowCaster;

	static ToObject()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_belowCaster = new NonAllocCaster(1);
	}

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	public override Vector2 GetPosition()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (!_onPlatform)
		{
			return Vector2.op_Implicit(_object.transform.position);
		}
		return PlatformUtils.GetProjectionPointToPlatform(Vector2.op_Implicit(_object.transform.position), Vector2.down, _belowCaster, _groundMask);
	}
}
