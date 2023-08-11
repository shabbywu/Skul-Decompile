using Characters;
using Runnables.Triggers;
using UnityEngine;

namespace Runnables;

public class InvokeOnCharacterDied : MonoBehaviour
{
	[SerializeField]
	[Trigger.Subcomponent]
	private Trigger _trigger;

	[SerializeField]
	private Runnable _execute;

	[SerializeField]
	private Character _character;

	private void Awake()
	{
		if (!((Object)(object)_character == (Object)null))
		{
			_character.health.onDied += OnDied;
		}
	}

	private void OnDied()
	{
		if (!((Object)(object)_character == (Object)null))
		{
			_character.health.onDied -= OnDied;
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
			_character.health.onDied -= OnDied;
		}
	}
}
