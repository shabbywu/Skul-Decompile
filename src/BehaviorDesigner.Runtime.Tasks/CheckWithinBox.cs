using Characters;
using PhysicsUtils;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("Allows multiple action tasks to be added to a single node.")]
[TaskIcon("Assets/Behavior Designer/Icon/CheckWithinSight.png")]
public sealed class CheckWithinBox : Conditional
{
	private static NonAllocOverlapper _overlapper;

	[SerializeField]
	private SharedCharacter _owner;

	[SerializeField]
	private SharedCharacter _target;

	[SerializeField]
	private Vector2 _boxOffset;

	[SerializeField]
	private Vector2 _boxSize;

	[SerializeField]
	private float _boxAngle;

	[SerializeField]
	private TargetLayer _targetLayer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	private Character _ownerValue;

	static CheckWithinBox()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		_overlapper = new NonAllocOverlapper(15);
	}

	public override void OnAwake()
	{
		_ownerValue = ((SharedVariable<Character>)_owner).Value;
	}

	public override TaskStatus OnUpdate()
	{
		Character character = FindTarget();
		if (!((Object)(object)character == (Object)null))
		{
			((SharedVariable)_target).SetValue((object)character);
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}

	private Character FindTarget()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_targetLayer.Evaluate(((Component)_ownerValue).gameObject));
		return TargetFinder.FindClosestTarget(_overlapper, Vector2.op_Implicit(((Component)_ownerValue).transform.position) + _boxOffset, _boxSize, _boxAngle);
	}
}
