using Characters.Controllers;
using InControl;
using Services;
using Singletons;
using UnityEngine;

namespace Runnables.Triggers;

public class InteractionInput : Trigger
{
	private PlayerInput _input;

	private void Awake()
	{
		if ((Object)(object)Singleton<Service>.Instance.levelManager == (Object)null || (Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null)
		{
			Debug.LogError((object)"No levelManager or player character.");
		}
		else
		{
			_input = ((Component)Singleton<Service>.Instance.levelManager.player).GetComponent<PlayerInput>();
		}
	}

	protected override bool Check()
	{
		if ((Object)(object)_input == (Object)null)
		{
			return true;
		}
		return ((OneAxisInputControl)_input.interaction).IsPressed;
	}
}
