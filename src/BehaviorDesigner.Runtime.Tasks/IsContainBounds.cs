using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Collider가 해당 layerMask를 가진 충돌체의 바운드로부터 포함되어있는지")]
public sealed class IsContainBounds : Conditional
{
	[SerializeField]
	private SharedCollider _bounds;

	[SerializeField]
	private LayerMask _layerMask;

	private Collider2D _boundsValue;

	private NonAllocOverlapper _overlapper;

	public override void OnAwake()
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		_overlapper = new NonAllocOverlapper(31);
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_layerMask);
		_boundsValue = ((SharedVariable<Collider2D>)_bounds).Value;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0049: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d3: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e0: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		foreach (Collider2D result in _overlapper.OverlapCollider(_boundsValue).results)
		{
			Bounds bounds = result.bounds;
			float x = ((Bounds)(ref bounds)).min.x;
			bounds = _boundsValue.bounds;
			if (!(x <= ((Bounds)(ref bounds)).min.x))
			{
				continue;
			}
			bounds = result.bounds;
			float y = ((Bounds)(ref bounds)).min.y;
			bounds = _boundsValue.bounds;
			if (!(y <= ((Bounds)(ref bounds)).min.y))
			{
				continue;
			}
			bounds = result.bounds;
			float x2 = ((Bounds)(ref bounds)).max.x;
			bounds = _boundsValue.bounds;
			if (x2 >= ((Bounds)(ref bounds)).max.x)
			{
				bounds = result.bounds;
				float y2 = ((Bounds)(ref bounds)).max.y;
				bounds = _boundsValue.bounds;
				if (y2 >= ((Bounds)(ref bounds)).max.y)
				{
					return (TaskStatus)2;
				}
			}
		}
		return (TaskStatus)1;
	}
}
