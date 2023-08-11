using System.Collections;
using Characters;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Level.Adventurer;

public class AdventurerCameraZone : MonoBehaviour
{
	[SerializeField]
	private Transform _cameraTarget;

	[SerializeField]
	private float _cameraTrackSpeed = 3f;

	[SerializeField]
	private EnemyWave _enemyWave;

	[SerializeField]
	[GetComponent]
	private BoxCollider2D _startTrigger;

	private float _trackSpeedCached;

	private void Awake()
	{
		_enemyWave.onClear += DisableCameraZone;
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

	private void EnableCameraZone()
	{
		Scene<GameBase>.instance.cameraController.trackSpeed = _cameraTrackSpeed;
		Scene<GameBase>.instance.cameraController.StartTrack(_cameraTarget);
	}

	public void DisableCameraZone()
	{
		Scene<GameBase>.instance.cameraController.trackSpeed = 0.05f;
		Scene<GameBase>.instance.cameraController.StartTrack(((Component)Singleton<Service>.Instance.levelManager.player).transform);
		((MonoBehaviour)this).StartCoroutine(CReturnTrackSpeed());
	}

	private IEnumerator CReturnTrackSpeed()
	{
		yield return Chronometer.global.WaitForSeconds(2f);
		Scene<GameBase>.instance.cameraController.trackSpeed = _trackSpeedCached;
	}

	private void OnDestroy()
	{
		if (!((Object)(object)Scene<GameBase>.instance.cameraController == (Object)null))
		{
			Scene<GameBase>.instance.cameraController.trackSpeed = _trackSpeedCached;
			if (!((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null))
			{
				Scene<GameBase>.instance.cameraController.StartTrack(((Component)Singleton<Service>.Instance.levelManager.player).transform);
			}
		}
	}
}
