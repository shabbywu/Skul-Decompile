using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public class FlyToDestination : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedTransform _destination;

	[SerializeField]
	private float minimumDistanceValue = 0.1f;

	private Character _ownerValue;

	private Transform _destinationValue;

	public override void OnStart()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
		_destinationValue = ((SharedVariable<Transform>)_destination).Value;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0086: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_ownerValue == (Object)null) && !((Object)(object)_destinationValue == (Object)null))
		{
			if (!(Vector2.Distance(Vector2.op_Implicit(_destinationValue.position), Vector2.op_Implicit(((Component)_ownerValue).transform.position)) <= minimumDistanceValue))
			{
				Vector3 val = _destinationValue.position - ((Component)_ownerValue).transform.position;
				Vector3 normalized = ((Vector3)(ref val)).normalized;
				_ownerValue.movement.Move(Vector2.op_Implicit(normalized));
				return (TaskStatus)3;
			}
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
