using System.Collections;
using Characters.Controllers;
using Data;
using GameResources;
using InControl;
using Level;
using Platforms;
using Scenes;
using Services;
using Singletons;
using UnityEngine;
using UserInput;

namespace Characters.Player;

public class HandlePlayerDeath : MonoBehaviour
{
	private Character _player;

	private GameBase _gameBase;

	private CoroutineReference _slowMotionReference;

	private void Awake()
	{
		_player = ((Component)this).GetComponent<Character>();
		_gameBase = Scene<GameBase>.instance;
		_player.health.onDiedTryCatch += OnPlayerDied;
	}

	private void OnPlayerDied()
	{
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0118: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		GameData.Progress.deaths++;
		GameData.Save.instance.hasSave = false;
		GameData.Progress.SaveAll();
		GameData.Save.instance.SaveAll();
		GameData.Currency.SaveAll();
		PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
		PlayerDieHeadParts component = ((Component)CommonResource.instance.playerDieHeadParts.parts.poolObject.Spawn(true)).GetComponent<PlayerDieHeadParts>();
		((Component)component).transform.parent = ((Component)Map.Instance).transform;
		DroppedParts parts = component.parts;
		((Component)component).transform.position = ((Component)_player).transform.position;
		component.sprite = _player.playerComponents.inventory.weapon.polymorphOrCurrent.icon;
		parts.Initialize(_player.movement.push);
		_gameBase.cameraController.StartTrack(((Component)component).transform);
		_gameBase.cameraController.Zoom(0.8f);
		((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(CWaitForGameResult());
		_slowMotionReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)CoroutineProxy.instance, CSlowMotion());
	}

	private IEnumerator CWaitForGameResult()
	{
		_gameBase.uiManager.pauseEventSystem.PushEmpty();
		PlayerInput.blocked.Attach((object)this);
		yield return (object)new WaitForSecondsRealtime(2f);
		_gameBase.uiManager.gameResult.Show();
		while (!_gameBase.uiManager.gameResult.animationFinished || (!((OneAxisInputControl)KeyMapper.Map.Attack).WasPressed && !((OneAxisInputControl)KeyMapper.Map.Submit).WasPressed))
		{
			yield return null;
		}
		yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
		_gameBase.uiManager.gameResult.Hide();
		PlayerInput.blocked.Detach((object)this);
		_gameBase.uiManager.pauseEventSystem.PopEvent();
		((CoroutineReference)(ref _slowMotionReference)).Stop();
		((ChronometerBase)Chronometer.global).DetachTimeScale((object)this);
		if (GameData.HardmodeProgress.hardmode)
		{
			ExtensionMethods.Set((Type)63);
		}
		Singleton<Service>.Instance.levelManager.ResetGame();
	}

	private IEnumerator CSlowMotion()
	{
		((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, 0.1f);
		yield return Chronometer.global.WaitForSeconds(0.2f);
		((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, 0.3f);
		yield return Chronometer.global.WaitForSeconds(0.2f);
		((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, 0.5f);
		yield return Chronometer.global.WaitForSeconds(0.2f);
		((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, 0.7f);
		yield return Chronometer.global.WaitForSeconds(0.2f);
		((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, 0.9f);
		yield return Chronometer.global.WaitForSeconds(0.2f);
		((ChronometerBase)Chronometer.global).DetachTimeScale((object)this);
	}
}
