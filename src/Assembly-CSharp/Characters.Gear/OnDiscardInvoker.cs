using Runnables;
using Runnables.Triggers;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Gear;

public sealed class OnDiscardInvoker : MonoBehaviour
{
	[SerializeField]
	private Gear _gear;

	[SerializeField]
	[Trigger.Subcomponent]
	private Trigger _trigger;

	[Runnable.Subcomponent]
	[SerializeField]
	private Runnable _execute;

	private void Start()
	{
		_gear.onDiscard += OnDiscard;
		Singleton<Service>.Instance.levelManager.onMapLoaded += OnMapLoaded;
	}

	private void OnMapLoaded()
	{
		if ((Object)(object)_gear == (Object)null)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
	}

	private void OnDiscard(Gear gear)
	{
		if (_trigger.IsSatisfied())
		{
			_execute.Run();
		}
	}
}
