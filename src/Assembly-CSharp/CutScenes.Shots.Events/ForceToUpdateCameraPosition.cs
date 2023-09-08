using Scenes;

namespace CutScenes.Shots.Events;

public class ForceToUpdateCameraPosition : Event
{
	public override void Run()
	{
		Scene<GameBase>.instance.cameraController.UpdateCameraPosition();
	}
}
