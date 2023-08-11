using UnityEngine;

namespace Runnables;

public sealed class CharacterSetPositionTo : Runnable
{
	[SerializeField]
	private Target _target;

	[SerializeField]
	private Transform _point;

	public override void Run()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		((Component)_target.character).transform.position = ((Component)_point).transform.position;
	}
}
