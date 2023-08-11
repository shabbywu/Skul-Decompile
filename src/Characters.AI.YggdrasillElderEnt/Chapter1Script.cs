using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Characters.AI.YggdrasillElderEnt;

public class Chapter1Script : MonoBehaviour
{
	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			LetterBox.instance.Disappear();
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
			if (!((Object)(object)Scene<GameBase>.instance.cameraController == (Object)null) && !((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null))
			{
				Scene<GameBase>.instance.cameraController.StartTrack(((Component)Singleton<Service>.Instance.levelManager.player).transform);
			}
		}
	}
}
