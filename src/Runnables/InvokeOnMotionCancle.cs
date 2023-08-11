using Characters.Actions;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables;

public class InvokeOnMotionCancle : MonoBehaviour
{
	[SerializeField]
	[Trigger.Subcomponent]
	private Trigger _trigger;

	[SerializeField]
	private Motion _motion;

	[SerializeField]
	private Runnable _execute;

	private void Awake()
	{
		if (!((Object)(object)_motion == (Object)null))
		{
			_motion.onCancel += ExecuteRunnable;
		}
	}

	private void OnDestroy()
	{
		if (!((Object)(object)_motion == (Object)null))
		{
			_motion.onCancel -= ExecuteRunnable;
		}
	}

	private void ExecuteRunnable()
	{
		if (!((Object)(object)_motion == (Object)null) && _trigger.IsSatisfied())
		{
			_execute.Run();
		}
	}
}
