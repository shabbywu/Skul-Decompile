using System.Collections;
using Characters;
using Characters.Actions;
using Characters.Controllers;
using Level;
using Scenes;
using Services;
using Singletons;
using UnityEngine;

namespace Tutorials;

public class DieTutorial : Tutorial
{
	[SerializeField]
	private EnemyWave _enemyWave;

	[SerializeField]
	private Hero _hero;

	[SerializeField]
	private Character _ogre;

	[SerializeField]
	private GameObject _darkOgre;

	[SerializeField]
	private Character _witch;

	[SerializeField]
	private Transform _plyerConversationPoint;

	[SerializeField]
	private Transform _ogreConversationPoint;

	[SerializeField]
	private Transform _witchConversationPoint;

	[SerializeField]
	private Transform _diePoint;

	[SerializeField]
	private BossNameDisplay _bossNameDisplay;

	[SerializeField]
	private Action _wakeUp;

	[SerializeField]
	private Action _laugh;

	protected override void Start()
	{
		base.Start();
		_player = Singleton<Service>.Instance.levelManager.player;
		_enemyWave.onClear += delegate
		{
			PlayerInput.blocked.Attach((object)this);
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			((MonoBehaviour)this).StartCoroutine(ProcessOgreDie());
		};
		IEnumerator ProcessOgreDie()
		{
			Singleton<Service>.Instance.fadeInOut.SetFadeColor(Color.white);
			yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
			yield return Singleton<Service>.Instance.fadeInOut.CFadeIn();
			Singleton<Service>.Instance.fadeInOut.SetFadeColor(Color.black);
			yield return Chronometer.global.WaitForSeconds(1.7f);
			Activate();
		}
	}

	protected override IEnumerator Process()
	{
		yield return Chronometer.global.WaitForSeconds(2f);
		yield return Singleton<Service>.Instance.fadeInOut.CFadeOut();
		((Component)_player).transform.position = _plyerConversationPoint.position;
		((Component)_ogre).transform.position = _ogreConversationPoint.position;
		((Component)_witch).transform.position = _witchConversationPoint.position;
		Object.Destroy((Object)(object)_darkOgre);
		((Component)_ogre).gameObject.SetActive(true);
		((Component)_ogre).transform.localScale = new Vector3(-1f, 1f, 1f);
		_witch.lookingDirection = Character.LookingDirection.Right;
		_player.lookingDirection = Character.LookingDirection.Right;
		yield return Chronometer.global.WaitForSeconds(2f);
		_wakeUp.TryStart();
		yield return Singleton<Service>.Instance.fadeInOut.CFadeIn();
		while (_wakeUp.running)
		{
			yield return null;
		}
		_npcConversation.Done();
		_laugh.TryStart();
		while (_laugh.running)
		{
			yield return null;
		}
		yield return MoveTo(_diePoint.position);
		Scene<GameBase>.instance.cameraController.Shake(1.3f, 0.5f);
		((Component)((Component)_hero).transform.parent).gameObject.SetActive(true);
		yield return _hero.CAppear();
		_npcConversation.Done();
		yield return _hero.CAttack();
		yield return CDeactivate();
		PlayerInput.blocked.Detach((object)this);
		Singleton<Service>.Instance.levelManager.ResetGame(Chapter.Type.Castle);
	}

	private new IEnumerator MoveTo(Vector3 destination)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		while (true)
		{
			float num = destination.x - ((Component)_ogre).transform.position.x;
			if (!(Mathf.Abs(num) < 1f))
			{
				Vector2 move = ((num > 0f) ? Vector2.right : Vector2.left);
				_ogre.movement.move = move;
				yield return null;
				continue;
			}
			break;
		}
	}

	protected override void OnDisable()
	{
		base.OnDisable();
		PlayerInput.blocked.Detach((object)this);
	}
}
