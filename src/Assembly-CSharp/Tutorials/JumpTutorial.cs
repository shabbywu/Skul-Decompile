using System.Collections;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Tutorials;

public class JumpTutorial : Tutorial
{
	[SerializeField]
	private Transform _skeletone;

	[SerializeField]
	private Transform _trackPoint;

	public override void Activate()
	{
		base.Activate();
		Scene<GameBase>.instance.cameraController.StartTrack(_trackPoint);
	}

	public override void Deactivate()
	{
		((MonoBehaviour)this).StartCoroutine(CDeactivate());
		IEnumerator CDeactivate()
		{
			base.state = State.Done;
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			yield return LetterBox.instance.CDisappear();
			Scene<GameBase>.instance.cameraController.StartTrack(((Component)Singleton<Service>.Instance.levelManager.player).transform);
		}
	}

	protected override IEnumerator Process()
	{
		yield return Converse();
	}
}
