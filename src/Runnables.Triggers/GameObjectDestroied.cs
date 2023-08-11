using UnityEngine;

namespace Runnables.Triggers;

public class GameObjectDestroied : Trigger
{
	[SerializeField]
	private GameObject _target;

	protected override bool Check()
	{
		return (Object)(object)_target == (Object)null;
	}
}
