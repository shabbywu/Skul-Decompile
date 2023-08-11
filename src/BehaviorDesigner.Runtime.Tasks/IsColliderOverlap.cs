using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("특정 레이어 마스크의 콜라이더에 오버랩 되는것이 있는지 확인합니다.하나라도 있으면 성공 없으면 실패를 반환합니다.")]
public sealed class IsColliderOverlap : Conditional
{
	[SerializeField]
	private SharedCollider _range;

	[SerializeField]
	private LayerMask _layerMask;

	public override TaskStatus OnUpdate()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		NonAllocOverlapper val = new NonAllocOverlapper(31);
		((ContactFilter2D)(ref val.contactFilter)).SetLayerMask(_layerMask);
		if (val.OverlapCollider(((SharedVariable<Collider2D>)_range).Value).results.Count != 0)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
