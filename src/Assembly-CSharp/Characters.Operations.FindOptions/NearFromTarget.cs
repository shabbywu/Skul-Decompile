using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.FindOptions;

[Serializable]
public class NearFromTarget : ICondition
{
	[SerializeField]
	private BehaviorDesignerCommunicator _communicator;

	[SerializeField]
	private string _targetName = "Target";

	[SerializeField]
	private CustomFloat _maxDistance;

	public bool Satisfied(Character character)
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		Character value = ((SharedVariable<Character>)_communicator.GetVariable<SharedCharacter>(_targetName)).Value;
		if ((Object)(object)value == (Object)null)
		{
			return false;
		}
		float num = Vector2.Distance(Vector2.op_Implicit(((Component)value).transform.position), Vector2.op_Implicit(((Component)character).transform.position));
		return _maxDistance.value > num;
	}
}
