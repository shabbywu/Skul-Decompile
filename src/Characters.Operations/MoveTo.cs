using UnityEngine;

namespace Characters.Operations;

public class MoveTo : TargetedCharacterOperation
{
	[SerializeField]
	private Transform _position;

	public override void Run(Character owner, Character target)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		((Component)target).transform.position = _position.position;
	}
}
