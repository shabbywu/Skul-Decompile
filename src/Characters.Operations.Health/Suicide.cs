using UnityEngine;

namespace Characters.Operations.Health;

public class Suicide : CharacterOperation
{
	[SerializeField]
	private bool _force = true;

	public override void Run(Character owner)
	{
		if (_force)
		{
			owner.health.Kill();
		}
		else
		{
			owner.health.TryKill();
		}
	}
}
