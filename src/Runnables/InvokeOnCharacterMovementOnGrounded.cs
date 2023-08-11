using Characters;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables;

public class InvokeOnCharacterMovementOnGrounded : MonoBehaviour
{
	[SerializeField]
	[Trigger.Subcomponent]
	private Trigger _trigger;

	[SerializeField]
	private Character _character;

	[SerializeField]
	private Runnable _execute;

	private void Awake()
	{
		if (!((Object)(object)_character == (Object)null))
		{
			_character.movement.onGrounded += ExecuteRunnable;
		}
	}

	private void OnDestroy()
	{
		if (!((Object)(object)_character == (Object)null))
		{
			_character.movement.onGrounded -= ExecuteRunnable;
		}
	}

	private void ExecuteRunnable()
	{
		if (!((Object)(object)_character == (Object)null) && _trigger.IsSatisfied())
		{
			_execute.Run();
		}
	}
}
