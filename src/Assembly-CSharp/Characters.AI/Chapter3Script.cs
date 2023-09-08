using System;
using System.Collections;
using System.Linq;
using Characters.AI.Chimera;
using Characters.Controllers;
using Characters.Operations;
using Characters.Operations.Fx;
using CutScenes;
using Data;
using FX;
using Level;
using Runnables;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEditor;
using UnityEngine;

namespace Characters.AI;

public class Chapter3Script : MonoBehaviour
{
	[Serializable]
	private class DarkQuartzPossibility
	{
		[Serializable]
		internal class Reorderable : ReorderableArray<DarkQuartzPossibility>
		{
			public int Take()
			{
				if (values.Length == 0)
				{
					return 0;
				}
				int num = values.Sum((DarkQuartzPossibility v) => v.weight);
				int num2 = Random.Range(0, num) + 1;
				for (int i = 0; i < values.Length; i++)
				{
					num2 -= values[i].weight;
					if (num2 <= 0)
					{
						return (int)values[i].amount.value;
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
		private Chapter3Script _script;

		internal Intro(Chapter3Script script)
		{
			_script = script;
		}

		public void IntroStart()
		{
			if (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(CutScenes.Key.chimera_Intro))
			{
				_script.fakeBossNameDisplay.ShowAndHideAppearanceText();
			}
		}

		public void IntroEnd()
		{
			((MonoBehaviour)_script).StartCoroutine(_script.chimera.Combat());
		}

		public IEnumerator CMovePlayerToCenter(Vector2 dest)
		{
			//IL_000e: Unknown result type (might be due to invalid IL or missing references)
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			Character player = Singleton<Service>.Instance.levelManager.player;
			player.CancelAction();
			PlayerInput.blocked.Attach(_script);
			yield return MoveTo(Vector2.op_Implicit(dest), player);
			PlayerInput.blocked.Detach(_script);
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
		private Chapter3Script _script;

		internal InGame(Chapter3Script script)
		{
			_script = script;
		}
	}

	private class Outro
	{
		private Chapter3Script _script;

		internal Outro(Chapter3Script script)
		{
			_script = script;
		}

		public void StartOutro()
		{
			_script.StartSequence();
			_script.flash.Run(_script.chimera.character);
			PersistentSingleton<SoundManager>.Instance.StopBackGroundMusic();
		}

		public void EndOutro()
		{
			_script.EndSequence();
			Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
		}
	}

	[SerializeField]
	private MusicInfo _bacgkroundMusicInfo;

	[SerializeField]
	private Characters.AI.Chimera.Chimera chimera;

	[SerializeField]
	[Space]
	[Header("Intro")]
	[Range(0.5f, 1.5f)]
	private float _cameraRatio = 1.2f;

	[Range(0.1f, 10f)]
	[SerializeField]
	private float _cameraZoomOutSpeed = 1f;

	[SerializeField]
	private BossNameDisplay fakeBossNameDisplay;

	[SerializeField]
	private BossNameDisplay bossNameDisplay;

	[SerializeField]
	private Transform _playerIntroPoint;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos alchemistDieOperation;

	[SerializeField]
	[Space]
	[Header("InGame")]
	private GameObject _ceil;

	[Subcomponent(typeof(Characters.Operations.Fx.ScreenFlash))]
	[Header("Outro")]
	[SerializeField]
	private Characters.Operations.Fx.ScreenFlash flash;

	[SerializeField]
	[Header("Reward")]
	private BossChest chest;

	[SerializeField]
	private GameObject _nextGate;

	[SerializeField]
	private DarkQuartzPossibility.Reorderable darkQuartzes;

	[SerializeField]
	private Runnable _cutScene;

	private Intro _intro;

	private Outro _outro;

	private void Start()
	{
		_intro = new Intro(this);
		_outro = new Outro(this);
		chimera.character.health.onDiedTryCatch += delegate
		{
			_outro.StartOutro();
			_outro.EndOutro();
			if (!GameData.HardmodeProgress.hardmode && !GameData.Progress.cutscene.GetData(CutScenes.Key.chimera_Outro))
			{
				_cutScene.Run();
			}
		};
		chest.OnOpen += delegate
		{
			_nextGate.SetActive(true);
		};
		((MonoBehaviour)this).StartCoroutine(CIntro());
	}

	private void OnDestroy()
	{
		if (!Service.quitting)
		{
			PlayerInput.blocked.Detach(this);
			Singleton<Service>.Instance.levelManager.player.movement.blocked.Detach(this);
			if ((Object)(object)Scene<GameBase>.instance.uiManager != (Object)null && (Object)(object)Scene<GameBase>.instance.uiManager.npcConversation != (Object)null)
			{
				Scene<GameBase>.instance.uiManager.npcConversation.Done();
			}
			if ((Object)(object)Scene<GameBase>.instance.uiManager != (Object)null && (Object)(object)Scene<GameBase>.instance.uiManager.headupDisplay != (Object)null)
			{
				Scene<GameBase>.instance.uiManager.headupDisplay.bossHealthBar.CloseAll();
			}
			LetterBox.instance.Disappear();
			if (!((Object)(object)Scene<GameBase>.instance.cameraController == (Object)null) && !Object.op_Implicit((Object)(object)Singleton<Service>.Instance.levelManager.player))
			{
				Scene<GameBase>.instance.cameraController.StartTrack(((Component)Singleton<Service>.Instance.levelManager.player).transform);
			}
		}
	}

	private IEnumerator CIntro()
	{
		StartSequence();
		yield return _intro.CMovePlayerToCenter(Vector2.op_Implicit(_playerIntroPoint.position));
		_intro.IntroStart();
		if (GameData.HardmodeProgress.hardmode || GameData.Progress.cutscene.GetData(CutScenes.Key.chimera_Intro))
		{
			EndIntro();
		}
	}

	public void StartSequence()
	{
		Scene<GameBase>.instance.uiManager.letterBox.Appear();
	}

	public void EndSequence()
	{
		Scene<GameBase>.instance.uiManager.letterBox.Disappear();
	}

	public void HideCeil()
	{
		_ceil.gameObject.SetActive(false);
	}

	public void ShowCeil()
	{
		_ceil.gameObject.SetActive(true);
	}

	public void EndIntro()
	{
		_intro.IntroEnd();
		PersistentSingleton<SoundManager>.Instance.PlayBackgroundMusic(_bacgkroundMusicInfo);
	}

	public void EndOutro()
	{
		if (GameData.HardmodeProgress.hardmode || GameData.Progress.cutscene.GetData(CutScenes.Key.chimera_Outro))
		{
			OpenChest();
		}
	}

	public void OpenChest()
	{
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0038: Unknown result type (might be due to invalid IL or missing references)
		((Component)chest).gameObject.SetActive(true);
		Singleton<Service>.Instance.levelManager.DropDarkQuartz(darkQuartzes.Take(), 30, ((Component)chest).transform.position, Vector2.up);
	}

	public void DisplayBossName()
	{
		bossNameDisplay.ShowAndHideAppearanceText();
	}
}
