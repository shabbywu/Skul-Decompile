using UnityEngine;

namespace Characters.Operations.ObjectTransform;

public sealed class SetRotationByDirection : Operation
{
	[SerializeField]
	private CustomFloat _baseDegree;

	[SerializeField]
	private Transform _from;

	[SerializeField]
	private Transform _to;

	[SerializeField]
	private Transform _target;

	public override void Run()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = MMMaths.Vector3ToVector2(((Component)_to).transform.position) - MMMaths.Vector3ToVector2(((Component)_from).transform.position);
		float num = Mathf.Atan2(val.y, val.x) * 57.29578f;
		((Component)_target).transform.localRotation = Quaternion.Euler(0f, 0f, _baseDegree.value + num);
	}
}
