using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public class IsNearByToDestination : Conditional
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedVector2 _destination;

	[SerializeField]
	private float _minimumDistance = 0.5f;

	private Character _ownerValue;

	private Vector2 _destinationValue;

	public override void OnStart()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
		_destinationValue = ((SharedVariable<Vector2>)_destination).Value;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)_ownerValue == (Object)null))
		{
			_ = _destinationValue;
			if (!(Vector2.Distance(Vector2.op_Implicit(((Component)_ownerValue).gameObject.transform.position), _destinationValue) < _minimumDistance))
			{
				return (TaskStatus)1;
			}
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
