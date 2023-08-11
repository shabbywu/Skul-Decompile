using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.LookAtTargets;

public sealed class BDTarget : Target
{
	[SerializeField]
	private BehaviorTree _tree;

	[SerializeField]
	private string _targetValueName = "Target";

	public override Character.LookingDirection GetDirectionFrom(Character character)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		SharedCharacter sharedCharacter = ((Behavior)_tree).GetVariable(_targetValueName) as SharedCharacter;
		if ((Object)(object)((SharedVariable<Character>)sharedCharacter).Value == (Object)null)
		{
			return character.lookingDirection;
		}
		if (!(((Component)character).transform.position.x > ((Component)((SharedVariable<Character>)sharedCharacter).Value).transform.position.x))
		{
			return Character.LookingDirection.Right;
		}
		return Character.LookingDirection.Left;
	}
}
