using Characters;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables;

public class InvokeOnCharacterDiedTryCatch : MonoBehaviour
{
	[Trigger.Subcomponent]
	[SerializeField]
	private Trigger _trigger;

	[SerializeField]
	private Runnable _execute;

	[SerializeField]
	private Character _character;

	private void Awake()
	{
		if (!((Object)(object)_character == (Object)null))
		{
			_character.health.onDiedTryCatch += OnDiedTryCatch;
		}
	}

	private void OnDiedTryCatch()
	{
		if (!((Object)(object)_character == (Object)null))
		{
			_character.health.onDiedTryCatch -= OnDiedTryCatch;
			if (_trigger.IsSatisfied())
			{
				_execute.Run();
			}
		}
	}

	private void OnDestroy()
	{
		if (!((Object)(object)_character == (Object)null))
		{
			_character.health.onDiedTryCatch -= OnDiedTryCatch;
		}
	}
}
