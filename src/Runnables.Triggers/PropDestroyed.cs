using Level;
using UnityEngine;

namespace Runnables.Triggers;

public class PropDestroyed : Trigger
{
	[SerializeField]
	private Prop _prop;

	private bool _destroyed;

	private void Awake()
	{
		if (!((Object)(object)_prop == (Object)null))
		{
			_prop.onDestroy += delegate
			{
				_destroyed = true;
			};
		}
	}

	protected override bool Check()
	{
		return _destroyed;
	}
}
