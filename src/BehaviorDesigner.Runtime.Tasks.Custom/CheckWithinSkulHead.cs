using Level;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom;

public sealed class CheckWithinSkulHead : Conditional
{
	[SerializeField]
	private SharedCollider _range;

	private LayerMask _layerMask = LayerMask.op_Implicit(134217728);

	[SerializeField]
	private SharedTransform _storeSkulHead;

	public override TaskStatus OnUpdate()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0008: Expected O, but got Unknown
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		NonAllocOverlapper val = new NonAllocOverlapper(31);
		((ContactFilter2D)(ref val.contactFilter)).SetLayerMask(_layerMask);
		if (val.OverlapCollider(((SharedVariable<Collider2D>)_range).Value).results.Count != 0)
		{
			foreach (Collider2D result in val.OverlapCollider(((SharedVariable<Collider2D>)_range).Value).results)
			{
				if (Object.op_Implicit((Object)(object)((Component)result).GetComponent<DroppedSkulHead>()))
				{
					((SharedVariable)_storeSkulHead).SetValue((object)((Component)result).transform);
					return (TaskStatus)2;
				}
			}
			return (TaskStatus)1;
		}
		return (TaskStatus)1;
	}
}
