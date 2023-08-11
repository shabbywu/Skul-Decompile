using Characters;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

public class CameraTargetChanger : MonoBehaviour
{
	[SerializeField]
	private Transform _cameraTarget;

	[SerializeField]
	private float _cameraTrackSpeed = 3f;

	[GetComponent]
	[SerializeField]
	private BoxCollider2D _startTrigger;

	private float _trackSpeedCached;

	private Coroutine _returnTrackSpeedModifier;

	private void Awake()
	{
		_trackSpeedCached = Scene<GameBase>.instance.cameraController.trackSpeed;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		Character component = ((Component)collision).GetComponent<Character>();
		if (!((Object)(object)component == (Object)null) && component.type == Character.Type.Player)
		{
			EnableCameraZone();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Character component = ((Component)collision).GetComponent<Character>();
		if (!((Object)(object)component == (Object)null) && component.type == Character.Type.Player)
		{
			DisableCameraZone();
		}
	}

	private void EnableCameraZone()
	{
		Scene<GameBase>.instance.cameraController.trackSpeed = _cameraTrackSpeed;
		Scene<GameBase>.instance.cameraController.StartTrack(_cameraTarget);
	}

	private void DisableCameraZone()
	{
		Scene<GameBase>.instance.cameraController.trackSpeed = _trackSpeedCached;
		Scene<GameBase>.instance.cameraController.StartTrack(((Component)Singleton<Service>.Instance.levelManager.player).transform);
	}

	private void OnDestroy()
	{
		if (Object.op_Implicit((Object)(object)Scene<GameBase>.instance))
		{
			DisableCameraZone();
		}
	}
}
