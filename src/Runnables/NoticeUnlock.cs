using Scenes;
using UnityEngine;

namespace Runnables;

public sealed class NoticeUnlock : Runnable
{
	[SerializeField]
	private Sprite _icon;

	[SerializeField]
	private string _name;

	public override void Run()
	{
		Scene<GameBase>.instance.uiManager.unlockNotice.Show(_icon, _name);
	}
}
