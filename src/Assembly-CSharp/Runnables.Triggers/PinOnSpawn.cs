using Characters;
using Level.Waves;
using UnityEngine;

namespace Runnables.Triggers;

public class PinOnSpawn : Trigger
{
	[SerializeField]
	private Pin _pin;

	protected override bool Check()
	{
		bool flag = false;
		if (_pin.characters == null)
		{
			return flag;
		}
		foreach (Character character in _pin.characters)
		{
			flag |= ((Component)character).gameObject.activeInHierarchy;
		}
		return flag;
	}
}
