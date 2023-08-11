using Characters;
using Hardmode;
using Level.Npc;
using Runnables;
using Singletons;
using UnityEngine;

namespace Level;

public sealed class HardmodeMagicMirror : InteractiveObject
{
	[SerializeField]
	private Dwarf _dwarf;

	[SerializeField]
	private Runnable _execute;

	private void Update()
	{
		if (_dwarf.tryLevel > Singleton<HardmodeManager>.Instance.clearedLevel + 1 && base.activated)
		{
			Deactivate();
		}
		else if (Singleton<HardmodeManager>.Instance.currentLevel <= Singleton<HardmodeManager>.Instance.clearedLevel + 1 && !base.activated)
		{
			Activate();
		}
	}

	public override void InteractWith(Character character)
	{
		base.InteractWith(character);
		_execute.Run();
	}
}
