using System;
using System.Collections;
using System.Collections.Generic;
using Characters;
using Characters.AI.Pope;
using Characters.Operations;
using CutScenes.Objects.Chapter4;
using Data;
using Level.Pope;
using Runnables;
using Scenes;
using Services;
using Singletons;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

namespace Level.Chapter4;

public class Scenario : MonoBehaviour
{
	[Serializable]
	private class Floor
	{
		[SerializeField]
		private int _floorValue;

		[SerializeField]
		private Platform[] _platforms;

		public int floorValue => _floorValue;

		public Platform[] platforms => _platforms;

		public Transform GetRandomPlatform()
		{
			return ((Component)_platforms.Random()).transform;
		}

		public Transform FindLastStandingPlatform()
		{
			Collider2D lastStandingCollider = Singleton<Service>.Instance.levelManager.player.movement.controller.collisionState.lastStandingCollider;
			if ((Object)(object)lastStandingCollider == (Object)null)
			{
				return null;
			}
			Platform[] array = _platforms;
			foreach (Platform platform in array)
			{
				if ((Object)(object)platform.collider == (Object)(object)lastStandingCollider)
				{
					return ((Component)platform).transform;
				}
			}
			return null;
		}
	}

	[SerializeField]
	private UnityEvent _onChestOpend;

	[SerializeField]
	private GameObject _gate;

	[SerializeField]
	private BossChest _chest;

	[SerializeField]
	private PopeAI popeAI;

	[SerializeField]
	[Header("Phase 1")]
	private Barrier _barrier;

	[SerializeField]
	private FanaticFactory fanaticFactory;

	[SerializeField]
	private Character _darkCrystalLeft;

	[SerializeField]
	[Space]
	private Character _darkCrystalRight;

	[Header("Phase 2")]
	[SerializeField]
	private PlatformContainer _platformContainer;

	[SerializeField]
	private Chair _chair;

	[SerializeField]
	private Fire _fire;

	[SerializeField]
	private Cleansing _cleansing;

	[Header("하드모드")]
	[SerializeField]
	private Floor[] _floorPlatforms;

	[SerializeField]
	private Transform _centerPlatform;

	[SerializeField]
	private Transform _attackPoint;

	[SerializeField]
	private CustomFloat _phase2EnvironmentsInterval;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _phase2EnvironmentsCenterInHardmode;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _phase2EnvironmentsInHardmode;

	private HashSet<Transform> _targets;

	private const int maxAttackCount = 3;

	public event Action OnPhase1Start;

	public event Action OnPhase1End;

	private bool IsCenterPlatform(Transform target)
	{
		return (Object)(object)target == (Object)(object)_centerPlatform;
	}

	private void Start()
	{
		_darkCrystalLeft.health.onDied += TryStart2Phase_Left;
		_darkCrystalRight.health.onDied += TryStart2Phase_Right;
		_chest.OnOpen += delegate
		{
			_gate.SetActive(true);
			UnityEvent onChestOpend = _onChestOpend;
			if (onChestOpend != null)
			{
				onChestOpend.Invoke();
			}
		};
		if (GameData.HardmodeProgress.hardmode)
		{
			_targets = new HashSet<Transform>();
		}
	}

	private IEnumerator CProcessInHardmodePhase2()
	{
		while (Scene<GameBase>.instance.uiManager.letterBox.visible)
		{
			yield return null;
		}
		while (!popeAI.character.health.dead)
		{
			yield return Chronometer.global.WaitForSeconds(_phase2EnvironmentsInterval.value);
			if (popeAI.character.health.dead)
			{
				break;
			}
			_floorPlatforms.Shuffle();
			_targets.Clear();
			Floor[] floorPlatforms = _floorPlatforms;
			for (int i = 0; i < floorPlatforms.Length; i++)
			{
				Transform val = floorPlatforms[i].FindLastStandingPlatform();
				if (!((Object)(object)val == (Object)null))
				{
					_attackPoint.position = val.position;
					if (IsCenterPlatform(val))
					{
						((MonoBehaviour)this).StartCoroutine(_phase2EnvironmentsCenterInHardmode.CRun(popeAI.character));
					}
					else
					{
						((MonoBehaviour)this).StartCoroutine(_phase2EnvironmentsInHardmode.CRun(popeAI.character));
					}
					_targets.Add(val);
					break;
				}
			}
			_ = _targets.Count;
			floorPlatforms = _floorPlatforms;
			foreach (Floor floor in floorPlatforms)
			{
				if (floor.floorValue != 2 && floor.floorValue != 4)
				{
					continue;
				}
				if (_targets.Count >= 1)
				{
					Transform obj = _targets.Random();
					bool flag = false;
					Platform[] platforms = floor.platforms;
					for (int j = 0; j < platforms.Length; j++)
					{
						if (((object)((Component)platforms[j]).transform).Equals((object?)obj))
						{
							flag = true;
						}
					}
					if (flag)
					{
						continue;
					}
				}
				Transform randomPlatform = floor.GetRandomPlatform();
				_attackPoint.position = randomPlatform.position;
				if (IsCenterPlatform(randomPlatform))
				{
					((MonoBehaviour)this).StartCoroutine(_phase2EnvironmentsCenterInHardmode.CRun(popeAI.character));
				}
				else
				{
					((MonoBehaviour)this).StartCoroutine(_phase2EnvironmentsInHardmode.CRun(popeAI.character));
				}
				_targets.Add(randomPlatform);
				break;
			}
			floorPlatforms = _floorPlatforms;
			foreach (Floor floor2 in floorPlatforms)
			{
				bool flag2 = false;
				Platform[] platforms = floor2.platforms;
				foreach (Platform platform in platforms)
				{
					if (_targets.Contains(((Component)platform).transform))
					{
						flag2 = true;
						break;
					}
				}
				if (!flag2)
				{
					Transform randomPlatform2 = floor2.GetRandomPlatform();
					_attackPoint.position = randomPlatform2.position;
					if (IsCenterPlatform(randomPlatform2))
					{
						((MonoBehaviour)this).StartCoroutine(_phase2EnvironmentsCenterInHardmode.CRun(popeAI.character));
					}
					else
					{
						((MonoBehaviour)this).StartCoroutine(_phase2EnvironmentsInHardmode.CRun(popeAI.character));
					}
					_targets.Add(randomPlatform2);
					if (_targets.Count >= 3)
					{
						break;
					}
				}
			}
		}
	}

	private void TryStart2Phase_Left()
	{
		_cleansing.Run();
		_darkCrystalLeft.health.onDied -= TryStart2Phase_Left;
		if (!_darkCrystalRight.health.dead)
		{
			_barrier.Crack();
			return;
		}
		this.OnPhase1End?.Invoke();
		StopDoing();
	}

	private void TryStart2Phase_Right()
	{
		_cleansing.Run();
		_darkCrystalRight.health.onDied -= TryStart2Phase_Right;
		if (!_darkCrystalLeft.health.dead)
		{
			_barrier.Crack();
			return;
		}
		this.OnPhase1End?.Invoke();
		StopDoing();
	}

	private void StopDoing()
	{
		popeAI.StopAllCoroutinesWithBehaviour();
		foreach (Character allEnemy in Map.Instance.waveContainer.GetAllEnemies())
		{
			if (!((Object)(object)popeAI.character == (Object)(object)allEnemy))
			{
				allEnemy.health.Kill();
			}
		}
		fanaticFactory.StopToSummon();
	}

	public void Start1Phase()
	{
		this.OnPhase1Start?.Invoke();
		popeAI.StartCombat();
		fanaticFactory.StartToSummon();
	}

	public void Start2Phase()
	{
		_platformContainer.Appear();
		_chair.Hide();
		_fire.Appear();
		ZoomOut();
		popeAI.NextPhase();
		if (GameData.HardmodeProgress.hardmode)
		{
			((MonoBehaviour)this).StartCoroutine(CProcessInHardmodePhase2());
		}
	}

	private void ZoomOut()
	{
		Scene<GameBase>.instance.cameraController.Zoom(1.3f);
	}

	private void ZoomIn()
	{
		Scene<GameBase>.instance.cameraController.Zoom(1f, 10f);
	}

	private void OnDestroy()
	{
		ZoomIn();
	}
}
