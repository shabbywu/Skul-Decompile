using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Characters.Movements;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations.BehaviorDesigner;

public class OverrideBDVariableInRange : Operation
{
	private static NonAllocOverlapper _overlapper = new NonAllocOverlapper(31);

	[SerializeField]
	[GetComponentInParent(false)]
	private Character _owner;

	[SerializeField]
	private Collider2D _notifyRange;

	[SerializeField]
	private TargetLayer _targetLayer;

	[SerializeField]
	private string _variableName;

	private Collider2D _ownerPlatform;

	public override void Run()
	{
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_owner == (Object)null)
		{
			_owner = ((Component)this).GetComponentInParent<Character>();
		}
		SharedVariable variable = ((Component)_owner).GetComponent<BehaviorDesignerCommunicator>().GetVariable(_variableName);
		if (variable == null)
		{
			return;
		}
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_targetLayer.Evaluate(((Component)_owner).gameObject));
		List<Character> components = _overlapper.OverlapCollider(_notifyRange).GetComponents<Character>(true);
		_ownerPlatform = _owner.movement.controller.collisionState.lastStandingCollider;
		foreach (Character item in components)
		{
			if (IsEqaulPlatform(item))
			{
				BehaviorDesignerCommunicator component = ((Component)item).GetComponent<BehaviorDesignerCommunicator>();
				if ((Object)(object)component != (Object)null)
				{
					component.SetVariable(_variableName, variable);
				}
			}
		}
	}

	private bool IsEqaulPlatform(Character target)
	{
		if (_owner.movement.config.type == Characters.Movements.Movement.Config.Type.Static)
		{
			return true;
		}
		if (target.movement.config.type == Characters.Movements.Movement.Config.Type.Static)
		{
			return true;
		}
		Collider2D lastStandingCollider = target.movement.controller.collisionState.lastStandingCollider;
		if ((Object)(object)lastStandingCollider != (Object)null && (Object)(object)_ownerPlatform != (Object)null && (Object)(object)_ownerPlatform == (Object)(object)lastStandingCollider)
		{
			return true;
		}
		return false;
	}
}
