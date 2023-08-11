using System;
using BehaviorDesigner.Runtime;
using UnityEngine;

namespace Characters.Operations.Summon;

[Serializable]
public class LookAtOwnerLookingDirection : IBDCharacterSetting
{
	[SerializeField]
	private Character _owenr;

	[SerializeField]
	private string _variableName;

	public void ApplyTo(Character character)
	{
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		((Component)character).GetComponent<BehaviorDesignerCommunicator>().SetVariable<SharedVector2>(_variableName, (object)((_owenr.lookingDirection == Character.LookingDirection.Left) ? Vector2.left : Vector2.right));
	}
}
