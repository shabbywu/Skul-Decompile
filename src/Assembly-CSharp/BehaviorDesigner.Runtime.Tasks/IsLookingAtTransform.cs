using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public sealed class IsLookingAtTransform : Conditional
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedTransform _transform;

	private Character _ownerValue;

	private Transform _transformValue;

	private Vector2 _direction;

	public override TaskStatus OnUpdate()
	{
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
		_transformValue = ((SharedVariable<Transform>)_transform).Value;
		Vector3 val = _transformValue.position - ((Component)_ownerValue).transform.position;
		_direction = Vector2.op_Implicit(((Vector3)(ref val)).normalized);
		if ((!(_direction.x > 0f) || _ownerValue.lookingDirection != Character.LookingDirection.Left) && (!(_direction.x < 0f) || _ownerValue.lookingDirection != 0))
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
