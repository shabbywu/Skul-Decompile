using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskIcon("Assets/Behavior Designer/Icon/CheckWithinSight.png")]
public sealed class CheckWithinLookingDirection : Conditional
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private float _rayDistance;

	[SerializeField]
	private LayerMask _layerMask;

	[SerializeField]
	private bool _reverseLookingDirection;

	[SerializeField]
	private float yOffset;

	private float _skinWidth = 0.1f;

	private RayCaster _rayCaster;

	private Character _ownerValue;

	public override void OnAwake()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		//IL_0065: Unknown result type (might be due to invalid IL or missing references)
		//IL_006f: Expected O, but got Unknown
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
		ContactFilter2D contactFilter = default(ContactFilter2D);
		((ContactFilter2D)(ref contactFilter)).SetLayerMask(_layerMask);
		if (_rayDistance <= 0f)
		{
			Bounds bounds = ((Collider2D)_ownerValue.collider).bounds;
			_rayDistance = ((Bounds)(ref bounds)).extents.x + _skinWidth;
		}
		_rayCaster = new RayCaster
		{
			contactFilter = contactFilter
		};
	}

	public override TaskStatus OnUpdate()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_008b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
		((Caster)_rayCaster).origin = Vector2.op_Implicit(((Task)this).transform.position);
		Vector2 offset = ((Collider2D)_ownerValue.collider).offset;
		if (yOffset == 0f)
		{
			((Caster)_rayCaster).origin.y += offset.y;
		}
		else
		{
			((Caster)_rayCaster).origin.y += yOffset;
		}
		if (!_reverseLookingDirection)
		{
			((Caster)_rayCaster).direction = ((_ownerValue.lookingDirection == Character.LookingDirection.Left) ? Vector2.left : Vector2.right);
		}
		else
		{
			((Caster)_rayCaster).direction = ((_ownerValue.lookingDirection == Character.LookingDirection.Left) ? Vector2.right : Vector2.left);
		}
		((Caster)_rayCaster).distance = _rayDistance;
		RaycastHit2D[] array = ((Caster)_rayCaster).Cast();
		for (int i = 0; i < array.Length; i++)
		{
			RaycastHit2D val = array[i];
			if (!((Object)(object)((Component)((RaycastHit2D)(ref val)).collider).gameObject == (Object)(object)((Component)_ownerValue).gameObject))
			{
				return (TaskStatus)2;
			}
		}
		return (TaskStatus)1;
	}
}
