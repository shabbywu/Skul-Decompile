using BehaviorDesigner.Runtime;
using FX;
using PhysicsUtils;
using UnityEngine;

namespace Characters.Operations;

public sealed class SetBDCharacterVariableInRange : CharacterOperation
{
	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private TargetLayer _targetLayer = new TargetLayer(LayerMask.op_Implicit(0), allyBody: false, foeBody: true, allyProjectile: false, foeProjectile: false);

	[SerializeField]
	private string variableName;

	[SubclassSelector]
	[SerializeReference]
	private SharedVariable _variable;

	[SerializeField]
	private EffectInfo _effect;

	private NonAllocOverlapper _overlapper;

	public override void Run(Character owner)
	{
		//IL_0003: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Expected O, but got Unknown
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		_overlapper = new NonAllocOverlapper(31);
		((Behaviour)_range).enabled = true;
		((ContactFilter2D)(ref _overlapper.contactFilter)).SetLayerMask(_targetLayer.Evaluate(((Component)owner).gameObject));
		foreach (Character component2 in _overlapper.OverlapCollider(_range).GetComponents<Character>(true))
		{
			BehaviorDesignerCommunicator component = ((Component)component2).GetComponent<BehaviorDesignerCommunicator>();
			if ((Object)(object)component != (Object)null)
			{
				if (component.GetVariable(variableName) == null)
				{
					break;
				}
				component.SetVariable(variableName, _variable);
				_effect.Spawn(((Component)component).transform.position);
			}
		}
	}
}
