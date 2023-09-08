using BehaviorDesigner.Runtime;
using Characters.Utils;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public class ToBDTransform : Policy
{
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string _transformName = "Destination";

	[SerializeField]
	private bool _onPlatform;

	[SerializeField]
	private LayerMask _groundMask = Layers.groundMask;

	private static NonAllocCaster _belowCaster;

	public override Vector2 GetPosition(Character owner)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		return GetPosition();
	}

	static ToBDTransform()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_belowCaster = new NonAllocCaster(1);
	}

	public override Vector2 GetPosition()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		Transform value = ((SharedVariable<Transform>)_communicator.GetVariable<SharedTransform>(_transformName)).Value;
		if ((Object)(object)value == (Object)null)
		{
			return Vector2.op_Implicit(((Component)this).transform.position);
		}
		if (!_onPlatform)
		{
			return Vector2.op_Implicit(value.position);
		}
		return PlatformUtils.GetProjectionPointToPlatform(Vector2.op_Implicit(value.position), Vector2.down, _belowCaster, _groundMask);
	}
}
