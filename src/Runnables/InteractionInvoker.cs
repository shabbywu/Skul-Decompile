using Characters;
using Runnables.Triggers;
using Singletons;
using UnityEngine;

namespace Runnables;

public class InteractionInvoker : InteractiveObject
{
	[Trigger.Subcomponent]
	[SerializeField]
	private Trigger _trigger;

	[SerializeField]
	private Runnable _execute;

	[SerializeField]
	private bool _once = true;

	private bool _used;

	public override void InteractWith(Character character)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		if ((!_once || !_used) && _trigger.IsSatisfied())
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
			_used = true;
			_execute.Run();
			if (_once)
			{
				Deactivate();
			}
		}
	}
}
