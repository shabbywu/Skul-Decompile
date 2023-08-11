using System;
using System.Collections;
using System.Linq;
using Characters.Controllers;
using Characters.Operations.Fx;
using CutScenes;
using Data;
using FX;
using Level;
using Level.Chapter2;
using Runnables;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;

namespace Characters.AI.TwinSister;

public class Chapter2Script : MonoBehaviour
{
	[Serializable]
	private class DarkQuartzPossibility
	{
		[Serializable]
		internal class Reorderable : ReorderableArray<DarkQuartzPossibility>
		{
			public int Take()
			{
				if (base.values.Length == 0)
				{
					return 0;
				}
				int num = base.values.Sum((DarkQuartzPossibility v) => v.weight);
				int num2 = Random.Range(0, num) + 1;
				for (int i = 0; i < base.values.Length; i++)
				{
					num2 -= base.values[i].weight;
					if (num2 <= 0)
					{
						return (int)base.values[i].amount.value;
					}
				}
				return 0;
			}
		}

		[Range(0f, 100f)]
		public int weight;

		public CustomFloat amount;
	}

	private class Intro
	{
		private Chapter2Script _script;

		public Intro(Chapter2Script script)
		{
			_script = script;
		}

		public void IntroStart()
		{
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = false;
			PlayerInput.blocked.Attach((object)_script);
			Scene<GameBase>.instance.uiManager.letterBox.Appear();
		}

		public void IntroEnd()
		{
			PlayerInput.blocked.Detach((object)_script);
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.OpenChapter2Phase1(_script._shortHair.character, _script._longHair.character);
		}

		public IEnumerator CMovePlayerToCenter(Vector2 dest)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.CancelAction();
			yield return MoveTo(Vector2.op_Implicit(dest), player);
		}

		public IEnumerator CAppearMaster(TwinSisterMasterAI twinSisterMasterAI)
		{
			yield return twinSisterMasterAI.CIntro();
		}

		public IEnumerator CIntroGoldenAide(BehindGoldenAide behindGoldenAide)
		{
			yield return behindGoldenAide.CIntroOut();
		}

		public IEnumerator CAppear(GoldenAideAI goldenAide)
		{
			yield return goldenAide.CIntro();
		}

		private IEnumerator MoveTo(Vector3 destination, Character player)
		{
			//IL_0007: Unknown result type (might be due to invalid IL or missing references)
			//IL_0008: Unknown result type (might be due to invalid IL or missing references)
			while (true)
			{
				float num = destination.x - ((Component)player).transform.position.x;
				if (!(Mathf.Abs(num) < 0.1f))
				{
					Vector2 move = ((num > 0f) ? Vector2.right : Vector2.left);
					player.movement.move = move;
					yield return null;
					continue;
				}
				break;
			}
		}
	}

	private class InGame
	{
		private GoldenAideAI _fieldAide;

		private GoldenAideAI _behindAide;

		private BehindGoldenAide _behind;

		private Chapter2Script _script;

		public InGame(Chapter2Script script)
		{
			_script = script;
		}

		public IEnumerator CGotoBehind()
		{
			yield return _behind.CIn();
		}

		public IEnumerator COutOfBehind()
		{
			yield return _behind.COut();
		}

		public IEnumerator CProcessSingleCombat(TwinSisterMasterAI master)
		{
			yield return master.ProcessSingleCombat(_fieldAide, _behindAide);
		}

		public IEnumerator CExpireSingleCombat(TwinSisterMasterAI master, float duration)
		{
			float elapsed = 0f;
			master.singlePattern = true;
			while (master.singlePattern)
			{
				yield return null;
				elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
				if (elapsed >= duration)
				{
					break;
				}
			}
			master.singlePattern = false;
		}

		public void SetFieldAideAndBehindAide(GoldenAideAI longhair, GoldenAideAI shorthair, BehindGoldenAide longhairBehind, BehindGoldenAide shorthairBehind)
		{
			if (MMMaths.RandomBool())
			{
				_fieldAide = longhair;
				_behindAide = shorthair;
				_behind = shorthairBehind;
			}
			else
			{
				_fieldAide = shorthair;
				_behindAide = longhair;
				_behind = longhairBehind;
			}
		}
	}

	private class Outro
	{
		private Chapter2Script _script;

		public Outro(Chapter2Script script)
		{
			_script = script;
		}

		public void StartOutro()
		{
			Singleton<Service>.Instance.levelManager.player.invulnerable.Attach((object)_script);
			_script.StartSequence();
			_script.flash.Run(_script.darkAide.character);
			PersistentSingleton<SoundManager>.Instance.StopBackGroundMusic();
			((MonoBehaviour)_script).StartCoroutine(CEndOutro());
		}

		private IEnumerator CEndOutro()
		{
			_script.twinSisterMasterAI.PlayAwakenDieReaction();
			yield return Chronometer.global.WaitForSeconds(8f);
			EndOutro();
		}

		private void EndOutro()
		{
			((MonoBehaviour)_script).StartCoroutine(CExit(_script.twinSisterMasterAI, _script.door));
		}

		public IEnumerator CExit(TwinSisterMasterAI masterAI, Door door)
		{
			if (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(CutScenes.Key.leiana_Outro))
			{
				_script._outroTalk.Run();
			}
			while (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(CutScenes.Key.leiana_Outro))
			{
				yield return null;
			}
			yield return Chronometer.global.WaitForSeconds(1f);
			PersistentSingleton<SoundManager>.Instance.PlaySound(_script._openDoorSound, ((Component)_script).transform.position);
			door.Open();
			yield return masterAI.COutro();
			PersistentSingleton<SoundManager>.Instance.PlaySound(_script._closeDoorSound, ((Component)_script).transform.position);
			door.Close();
			yield return Chronometer.global.WaitForSeconds(1f);
			((Component)_script.chest).gameObject.SetActive(true);
			Singleton<Service>.Instance.levelManager.DropDarkQuartz(_script.darkQuartzes.Take(), 30, ((Component)_script.chest).transform.position, Vector2.up);
			_script.EndSequence();
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
		}
	}

	[SerializeField]
	private MusicInfo _bacgkroundMusicInfo;

	[SerializeField]
	private MusicInfo _awakenMusicInfo;

	[SerializeField]
	private DarkAideAI darkAide;

	[SerializeField]
	[Space]
	[Header("Intro")]
	private BossNameDisplay bossNameDisplay;

	[SerializeField]
	private Transform _playerIntroPoint;

	[SerializeField]
	[Space]
	[Header("Door")]
	private Door door;

	[SerializeField]
	private SoundInfo _openDoorSound;

	[SerializeField]
	private SoundInfo _closeDoorSound;

	[Space]
	[Header("Master")]
	[SerializeField]
	private TwinSisterMasterAI twinSisterMasterAI;

	[Header("BehindGoldenAide")]
	[Space]
	[SerializeField]
	private BehindGoldenAide _leftGoldenAide;

	[SerializeField]
	private BehindGoldenAide _rightGoldenAide;

	[Header("FrontGoldenAide")]
	[Space]
	[SerializeField]
	private GoldenAideAI _shortHair;

	[SerializeField]
	private GoldenAideAI _longHair;

	[Header("Wave")]
	[Space]
	[SerializeField]
	private EnemyWave _goldenAideWave;

	[SerializeField]
	private float _siglePatternDuration;

	[Header("Outro")]
	[SerializeField]
	private Characters.Operations.Fx.ScreenFlash flash;

	[SerializeField]
	private Elevator _elevator;

	[SerializeField]
	[Header("Reward")]
	private BossChest chest;

	[SerializeField]
	private DarkQuartzPossibility.Reorderable darkQuartzes;

	[SerializeField]
	private Runnable _introTalk;

	[SerializeField]
	private Runnable _talkToStartCombat;

	[SerializeField]
	private Runnable _outroTalk;

	private bool _introCutScenePlayed;

	private Intro _intro;

	private InGame _inGame;

	private Outro _outro;

	private bool _goldenAideEnd;

	private CoroutineReference _combatReference;

	private CoroutineReference _expireSingleCombatReference;

	public bool introCutScenePlayed
	{
		get
		{
			return _introCutScenePlayed;
		}
		set
		{
			_introCutScenePlayed = value;
		}
	}

	private void Start()
	{
		_intro = new Intro(this);
		_inGame = new InGame(this);
		_outro = new Outro(this);
		chest.OnOpen += delegate
		{
			((Component)_elevator).gameObject.SetActive(true);
		};
		if (GameData.HardmodeProgress.hardmode || GameData.Progress.cutscene.GetData(CutScenes.Key.leiana_Intro))
		{
			_introCutScenePlayed = true;
		}
		((MonoBehaviour)this).StartCoroutine(CIntro());
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			Scene<GameBase>.instance.uiManager.npcConversation.Done();
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
			LetterBox.instance.Disappear();
			PlayerInput.blocked.Detach((object)this);
			if ((Object)(object)Singleton<Service>.Instance.levelManager.player != (Object)null)
			{
				Singleton<Service>.Instance.levelManager.player.invulnerable.Detach((object)this);
			}
			if (!((Object)(object)Scene<GameBase>.instance.cameraController == (Object)null) && !((Object)(object)Singleton<Service>.Instance.levelManager.player == (Object)null))
			{
				Scene<GameBase>.instance.cameraController.StartTrack(((Component)Singleton<Service>.Instance.levelManager.player).transform);
			}
		}
	}

	public void StartSequence()
	{
		Scene<GameBase>.instance.uiManager.letterBox.Appear();
	}

	public void EndSequence()
	{
		Scene<GameBase>.instance.uiManager.letterBox.Disappear();
		Scene<GameBase>.instance.cameraController.StartTrack(((Component)Singleton<Service>.Instance.levelManager.player).transform);
	}

	private IEnumerator CIntro()
	{
		_shortHair.Attachinvincibility();
		_longHair.Attachinvincibility();
		_intro.IntroStart();
		yield return _intro.CMovePlayerToCenter(Vector2.op_Implicit(_playerIntroPoint.position));
		yield return Chronometer.global.WaitForSeconds(2f);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_openDoorSound, ((Component)this).transform.position);
		door.Open();
		yield return Chronometer.global.WaitForSeconds(1f);
		yield return _intro.CAppearMaster(twinSisterMasterAI);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_closeDoorSound, ((Component)this).transform.position);
		door.Close();
		yield return Chronometer.global.WaitForSeconds(1f);
		if (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(CutScenes.Key.leiana_Intro))
		{
			_introTalk.Run();
		}
		while (!_introCutScenePlayed)
		{
			yield return null;
		}
		((MonoBehaviour)this).StartCoroutine(_intro.CIntroGoldenAide(_leftGoldenAide));
		yield return _intro.CIntroGoldenAide(_rightGoldenAide);
		yield return Chronometer.global.WaitForSeconds(1f);
		bossNameDisplay.ShowAndHideAppearanceText();
		_goldenAideWave.Spawn(effect: false);
		((MonoBehaviour)this).StartCoroutine(_shortHair.CIntro());
		yield return _longHair.CIntro();
		if (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(CutScenes.Key.leiana_Intro))
		{
			_talkToStartCombat.Run();
		}
		while (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(CutScenes.Key.leiana_Intro))
		{
			yield return null;
		}
		PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(_bacgkroundMusicInfo);
		yield return Scene<GameBase>.instance.uiManager.letterBox.CDisappear();
		_shortHair.Dettachinvincibility();
		_longHair.Dettachinvincibility();
		_combatReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, Combat());
		_intro.IntroEnd();
	}

	private IEnumerator Combat()
	{
		((MonoBehaviour)this).StartCoroutine(WaitForAwakeningPrepare());
		yield return twinSisterMasterAI.RunIntroOut();
		yield return twinSisterMasterAI.ProcessDualCombat();
		_inGame.SetFieldAideAndBehindAide(_shortHair, _longHair, _leftGoldenAide, _rightGoldenAide);
		yield return _inGame.CGotoBehind();
		_expireSingleCombatReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, _inGame.CExpireSingleCombat(twinSisterMasterAI, _siglePatternDuration));
		yield return _inGame.CProcessSingleCombat(twinSisterMasterAI);
		while (!_goldenAideEnd)
		{
			((MonoBehaviour)this).StartCoroutine(_inGame.COutOfBehind());
			yield return twinSisterMasterAI.ProcessDualCombat();
			_inGame.SetFieldAideAndBehindAide(_shortHair, _longHair, _leftGoldenAide, _rightGoldenAide);
			yield return _inGame.CGotoBehind();
			_expireSingleCombatReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, _inGame.CExpireSingleCombat(twinSisterMasterAI, _siglePatternDuration));
			yield return _inGame.CProcessSingleCombat(twinSisterMasterAI);
		}
	}

	private IEnumerator WaitForAwakeningPrepare()
	{
		while (twinSisterMasterAI.goldenAideDiedCount == 0)
		{
			yield return null;
		}
		_goldenAideEnd = true;
		((CoroutineReference)(ref _expireSingleCombatReference)).Stop();
		GoldenAideAI toBeDarkAide2 = (_shortHair.dead ? _longHair : _shortHair);
		toBeDarkAide2.Attachinvincibility();
		while (twinSisterMasterAI.lockForAwakening)
		{
			yield return null;
		}
		((CoroutineReference)(ref _combatReference)).Stop();
		PersistentSingleton<SoundManager>.Instance.StopBackGroundMusic();
		if (twinSisterMasterAI.goldenAideDiedCount == 1)
		{
			toBeDarkAide2.StopAllCoroutinesWithBehaviour();
			if (twinSisterMasterAI.singlePattern)
			{
				yield return _inGame.COutOfBehind();
				((MonoBehaviour)this).StartCoroutine(twinSisterMasterAI.CPlaySurpriseReaction());
				yield return toBeDarkAide2.CastAwakening();
			}
			else
			{
				_leftGoldenAide.Hide();
				_rightGoldenAide.Hide();
				((MonoBehaviour)this).StartCoroutine(twinSisterMasterAI.CPlaySurpriseReaction());
				yield return toBeDarkAide2.CastAwakening();
			}
			SpawnDarkAide(toBeDarkAide2.character, ((Component)toBeDarkAide2.character).transform.position, toBeDarkAide2.character.lookingDirection);
			toBeDarkAide2.Hide();
		}
		else if (twinSisterMasterAI.goldenAideDiedCount == 2)
		{
			_leftGoldenAide.Hide();
			_rightGoldenAide.Hide();
			toBeDarkAide2 = (MMMaths.RandomBool() ? _longHair : _shortHair);
			toBeDarkAide2.StopAllCoroutinesWithBehaviour();
			toBeDarkAide2.character.health.Revive();
			((MonoBehaviour)this).StartCoroutine(twinSisterMasterAI.CPlaySurpriseReaction());
			yield return toBeDarkAide2.CastAwakening();
			SpawnDarkAide(toBeDarkAide2.character, ((Component)toBeDarkAide2.character).transform.position, toBeDarkAide2.character.lookingDirection);
			toBeDarkAide2.Hide();
		}
		PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(_awakenMusicInfo);
		Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
		yield return Chronometer.global.WaitForSeconds(1f);
		Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.Open(BossHealthbarController.Type.Chapter2_Phase2, darkAide.character);
	}

	private void SpawnDarkAide(Character healthOwner, Vector3 position, Character.LookingDirection lookingDirection)
	{
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Bounds bounds = Map.Instance.bounds;
		float x = ((Bounds)(ref bounds)).center.x;
		twinSisterMasterAI.RemovePlayerHitReaction();
		darkAide.character.health.onDiedTryCatch += _outro.StartOutro;
		((Component)darkAide.character).transform.position = position;
		((Component)darkAide.character).gameObject.SetActive(true);
		darkAide.character.ForceToLookAt(x);
		darkAide.ApplyHealth(healthOwner);
	}
}
