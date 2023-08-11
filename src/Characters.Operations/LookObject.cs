using UnityEngine;

namespace Characters.Operations;

public class LookObject : CharacterOperation
{
	[SerializeField]
	private GameObject _target;

	public override void Run(Character owner)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		owner.ForceToLookAt(_target.transform.position.x);
	}
}
