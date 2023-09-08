using Characters.Gear;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables;

public sealed class OnGearDroppedInvoker : MonoBehaviour
{
	[SerializeField]
	private Gear _gear;

	[Trigger.Subcomponent]
	[SerializeField]
	private Trigger _trigger;

	[SerializeField]
	private Runnable _execute;

	private void Awake()
	{
		_gear.onDropped += OnGearDrop;
	}

	private void OnGearDrop()
	{
		if (_trigger.IsSatisfied())
		{
			_execute.Run();
		}
	}
}
