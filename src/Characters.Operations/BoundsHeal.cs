using PhysicsUtils;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations;

public class BoundsHeal : CharacterOperation
{
	private enum Type
	{
		Percent,
		Constnat
	}

	[SerializeField]
	private Type _type;

	[SerializeField]
	private CustomFloat _amount;

	[SerializeField]
	private Collider2D _range;

	[UnityEditor.Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _ToTargetOperations;

	private static readonly NonAllocOverlapper _targetOverlapper;

	static BoundsHeal()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Expected O, but got Unknown
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		_targetOverlapper = new NonAllocOverlapper(15);
		((ContactFilter2D)(ref _targetOverlapper.contactFilter)).SetLayerMask(LayerMask.op_Implicit(1024));
	}

	private void Awake()
	{
		_ToTargetOperations.Initialize();
	}

	public override void Run(Character owner)
	{
		foreach (Character component in _targetOverlapper.OverlapCollider(_range).GetComponents<Character>(true))
		{
			component.health.Heal(GetAmount(component));
			_ToTargetOperations.Run(component);
		}
	}

	private double GetAmount(Character target)
	{
		return _type switch
		{
			Type.Percent => (double)_amount.value * target.health.maximumHealth * 0.01, 
			Type.Constnat => _amount.value, 
			_ => 0.0, 
		};
	}
}
