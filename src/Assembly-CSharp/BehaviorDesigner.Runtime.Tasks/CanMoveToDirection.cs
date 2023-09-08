using Characters;
using Characters.AI;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

public class CanMoveToDirection : Conditional
{
	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedVector2 _direction;

	[SerializeField]
	private SharedFloat _minimumDistance = 0.5f;

	private Character _ownerValue;

	private float _minimumDistanceValue;

	public override void OnAwake()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
		_minimumDistanceValue = ((SharedVariable<float>)_minimumDistance).Value;
	}

	public override TaskStatus OnUpdate()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		Vector2 value = ((SharedVariable<Vector2>)_direction).Value;
		if (Precondition.CanMoveToDirection(_ownerValue, value, _minimumDistanceValue))
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
