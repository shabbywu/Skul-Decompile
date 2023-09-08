using Scenes;
using UnityEngine;

namespace CutScenes.Shots.Events;

public class ChangeCameraTrackSpeed : Event
{
	[SerializeField]
	private float _cameraTrackSpeed = 3f;

	public override void Run()
	{
		Scene<GameBase>.instance.cameraController.trackSpeed = _cameraTrackSpeed;
	}
}
