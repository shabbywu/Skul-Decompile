using UnityEngine;

namespace Characters.Operations.Movement;

public class KnockbackTo : TargetedCharacterOperation
{
	[Header("Destination")]
	[SerializeField]
	private Collider2D _targetPlace;

	[SerializeField]
	private Transform _targetPoint;

	[SerializeField]
	[Header("Force")]
	private Curve _curve;

	[SerializeField]
	private bool _ignoreOtherForce = true;

	[SerializeField]
	private bool _expireOnGround;

	public override void Run(Character owner, Character target)
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		if (!((Object)(object)target == (Object)null) && target.liveAndActive)
		{
			Vector2 val = ((!((Object)(object)_targetPlace != (Object)null)) ? Vector2.op_Implicit(_targetPoint.position) : MMMaths.RandomPointWithinBounds(_targetPlace.bounds));
			Vector2 force = val - Vector2.op_Implicit(((Component)target).transform.position);
			target.movement.push.ApplyKnockback(owner, force, _curve, _ignoreOtherForce, _expireOnGround);
		}
	}
}
