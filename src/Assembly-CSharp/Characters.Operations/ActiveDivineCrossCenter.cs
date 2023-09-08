using Level.Traps;
using UnityEngine;

namespace Characters.Operations;

public sealed class ActiveDivineCrossCenter : CharacterOperation
{
	[SerializeField]
	private RotateLaser _rotateLaser;

	public override void Run(Character owner)
	{
		_rotateLaser.Activate(owner);
	}
}
