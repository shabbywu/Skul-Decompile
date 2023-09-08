using UnityEngine;

namespace Characters.Operations;

public class TranslateDestination : CharacterOperation
{
	[SerializeField]
	private Transform _target;

	[SerializeField]
	private Transform _destination;

	public override void Run(Character owner)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_target.position = _destination.position;
	}
}
