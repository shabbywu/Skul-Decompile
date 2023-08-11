using System.Collections;
using Characters;
using PhysicsUtils;
using Scenes;
using Services;
using Singletons;
using UI;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;
using UnityEngine.Serialization;

namespace Level;

public class Map : MonoBehaviour
{
	public static class TestingTool
	{
		public static bool safeZone { get; set; } = false;


		public static bool fieldNPC { get; set; } = true;


		public static bool darkenemy { get; set; } = true;

	}

	public enum Type
	{
		Normal,
		Npc,
		Manual,
		Special
	}

	private static readonly NonAllocOverlapper _rewardActivatingRangeFinder;

	[Space]
	[SerializeField]
	private Type _type;

	[SerializeField]
	private ParallaxBackground _background;

	[SerializeField]
	private bool _pauseTimer;

	[SerializeField]
	private bool _displayStageName;

	[SerializeField]
	[FormerlySerializedAs("_hideRightBottomHud")]
	private bool _hideMinimap;

	[SerializeField]
	private bool _verticalLetterBox;

	[SerializeField]
	private CameraZone _cameraZone;

	[SerializeField]
	private Light2D _globalLight;

	[Space]
	[SerializeField]
	private Transform _playerOrigin;

	[SerializeField]
	private Transform _backgroundOrigin;

	[Space]
	[SerializeField]
	private bool _hasCeil;

	[SerializeField]
	private float _ceilHeight = 10f;

	[SerializeField]
	[Header("Gate")]
	private SpriteRenderer _gateWall;

	[SerializeField]
	private SpriteRenderer _gateTable;

	[SerializeField]
	private Transform _gate1Position;

	[SerializeField]
	private Transform _gate2Position;

	[SerializeField]
	[Header("Reward")]
	private MapReward _mapReward;

	[SerializeField]
	private Collider2D _mapRewardActivatingRange;

	private CoroutineReference _lightLerpReference;

	private Gate _gate1;

	private Gate _gate2;

	public static Map Instance { get; private set; }

	public Type type => _type;

	public ParallaxBackground background => _background;

	public EnemyWaveContainer waveContainer { get; private set; }

	public Cage cage { get; private set; }

	public Light2D globalLight => _globalLight;

	public Color originalLightColor { get; private set; }

	public float originalLightIntensity { get; private set; }

	public CameraZone cameraZone
	{
		get
		{
			return _cameraZone;
		}
		set
		{
			_cameraZone = value;
		}
	}

	public Bounds bounds { get; private set; }

	public Vector3 playerOrigin => ((Component)_playerOrigin).transform.position;

	public Vector3 backgroundOrigin => ((Component)_backgroundOrigin).transform.position;

	public bool pauseTimer => _pauseTimer;

	public bool displayStageName => _displayStageName;

	public bool darkEnemy { get; set; }

	public MapReward mapReward => _mapReward;

	static Map()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		_rewardActivatingRangeFinder = new NonAllocOverlapper(1);
		((ContactFilter2D)(ref _rewardActivatingRangeFinder.contactFilter)).SetLayerMask(LayerMask.op_Implicit(512));
	}

	private void Awake()
	{
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0064: Unknown result type (might be due to invalid IL or missing references)
		Instance = this;
		FindRequiredComponents(out var tilemapBaker2);
		tilemapBaker2.Bake();
		bounds = tilemapBaker2.bounds;
		InitializeGates();
		if ((Object)(object)waveContainer != (Object)null)
		{
			waveContainer.Initialize();
			if (TestingTool.safeZone)
			{
				waveContainer.HideAll();
			}
		}
		SetCameraZoneOrDefault();
		MakeBorders();
		originalLightColor = _globalLight.color;
		originalLightIntensity = _globalLight.intensity;
		UIManager uiManager = Scene<GameBase>.instance.uiManager;
		uiManager.headupDisplay.minimapVisible = !_hideMinimap;
		uiManager.scaler.SetVerticalLetterBox(_verticalLetterBox);
		((MonoBehaviour)this).StartCoroutine(CCheckRewardActivating());
		void FindRequiredComponents(out TilemapBaker tilemapBaker)
		{
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0024: Expected O, but got Unknown
			tilemapBaker = null;
			waveContainer = null;
			foreach (Transform item in ((Component)this).transform)
			{
				Transform val3 = item;
				EnemyWaveContainer component = ((Component)val3).GetComponent<EnemyWaveContainer>();
				if ((Object)(object)component != (Object)null)
				{
					waveContainer = component;
				}
				else
				{
					TilemapBaker component2 = ((Component)val3).GetComponent<TilemapBaker>();
					if ((Object)(object)component2 != (Object)null)
					{
						tilemapBaker = component2;
					}
					else
					{
						Cage component3 = ((Component)val3).GetComponent<Cage>();
						if ((Object)(object)component3 != (Object)null)
						{
							cage = component3;
						}
					}
				}
			}
		}
		void InitializeGates()
		{
			//IL_0039: Unknown result type (might be due to invalid IL or missing references)
			//IL_003e: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0060: Unknown result type (might be due to invalid IL or missing references)
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			_gateWall.sprite = currentChapter.gateWall;
			Gate gatePrefab = currentChapter.gatePrefab;
			if ((Object)(object)gatePrefab != (Object)null)
			{
				_gate1 = Object.Instantiate<Gate>(gatePrefab, _gate1Position.position, Quaternion.identity, ((Component)this).transform);
				_gate2 = Object.Instantiate<Gate>(gatePrefab, _gate2Position.position, Quaternion.identity, ((Component)this).transform);
			}
			Object.Destroy((Object)(object)((Component)_gate1Position).gameObject);
			Object.Destroy((Object)(object)((Component)_gate2Position).gameObject);
		}
		void MakeBorders()
		{
			//IL_0002: Unknown result type (might be due to invalid IL or missing references)
			//IL_000a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0032: Unknown result type (might be due to invalid IL or missing references)
			//IL_004a: Unknown result type (might be due to invalid IL or missing references)
			//IL_004f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0052: Unknown result type (might be due to invalid IL or missing references)
			//IL_005c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0071: Unknown result type (might be due to invalid IL or missing references)
			//IL_0076: Unknown result type (might be due to invalid IL or missing references)
			//IL_0079: Unknown result type (might be due to invalid IL or missing references)
			//IL_0083: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			//IL_009c: Unknown result type (might be due to invalid IL or missing references)
			//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00be: Unknown result type (might be due to invalid IL or missing references)
			//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d9: Unknown result type (might be due to invalid IL or missing references)
			//IL_00de: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e1: Unknown result type (might be due to invalid IL or missing references)
			//IL_00eb: Unknown result type (might be due to invalid IL or missing references)
			//IL_0100: Unknown result type (might be due to invalid IL or missing references)
			//IL_0105: Unknown result type (might be due to invalid IL or missing references)
			//IL_0108: Unknown result type (might be due to invalid IL or missing references)
			//IL_0112: Unknown result type (might be due to invalid IL or missing references)
			//IL_0125: Unknown result type (might be due to invalid IL or missing references)
			//IL_012b: Unknown result type (might be due to invalid IL or missing references)
			//IL_01d7: Unknown result type (might be due to invalid IL or missing references)
			//IL_01dc: Unknown result type (might be due to invalid IL or missing references)
			//IL_01df: Unknown result type (might be due to invalid IL or missing references)
			//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
			//IL_0204: Unknown result type (might be due to invalid IL or missing references)
			//IL_0207: Unknown result type (might be due to invalid IL or missing references)
			//IL_0219: Unknown result type (might be due to invalid IL or missing references)
			//IL_021e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0221: Unknown result type (might be due to invalid IL or missing references)
			//IL_022b: Unknown result type (might be due to invalid IL or missing references)
			//IL_023e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0244: Unknown result type (might be due to invalid IL or missing references)
			//IL_014f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0154: Unknown result type (might be due to invalid IL or missing references)
			//IL_0157: Unknown result type (might be due to invalid IL or missing references)
			//IL_0177: Unknown result type (might be due to invalid IL or missing references)
			//IL_017c: Unknown result type (might be due to invalid IL or missing references)
			//IL_017f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0191: Unknown result type (might be due to invalid IL or missing references)
			//IL_0196: Unknown result type (might be due to invalid IL or missing references)
			//IL_0199: Unknown result type (might be due to invalid IL or missing references)
			//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
			//IL_01bd: Unknown result type (might be due to invalid IL or missing references)
			//IL_01c3: Unknown result type (might be due to invalid IL or missing references)
			Vector2 val = default(Vector2);
			Vector2 offset = default(Vector2);
			int num = 20;
			BoxCollider2D obj = ((Component)this).gameObject.AddComponent<BoxCollider2D>();
			val.x = num;
			Bounds val2 = bounds;
			val.y = ((Bounds)(ref val2)).size.y + 50f;
			val2 = bounds;
			offset.x = ((Bounds)(ref val2)).min.x - val.x * 0.5f;
			val2 = bounds;
			offset.y = ((Bounds)(ref val2)).min.y + val.y * 0.5f;
			obj.size = val;
			((Collider2D)obj).offset = offset;
			BoxCollider2D obj2 = ((Component)this).gameObject.AddComponent<BoxCollider2D>();
			val.x = num;
			val2 = bounds;
			val.y = ((Bounds)(ref val2)).size.y + 50f;
			val2 = bounds;
			offset.x = ((Bounds)(ref val2)).max.x + val.x * 0.5f;
			val2 = bounds;
			offset.y = ((Bounds)(ref val2)).min.y + val.y * 0.5f;
			obj2.size = val;
			((Collider2D)obj2).offset = offset;
			if (cameraZone.hasCeil)
			{
				BoxCollider2D obj3 = ((Component)this).gameObject.AddComponent<BoxCollider2D>();
				val2 = bounds;
				val.x = ((Bounds)(ref val2)).size.x + (float)(num * 2);
				val.y = num;
				val2 = bounds;
				offset.x = ((Bounds)(ref val2)).center.x;
				val2 = bounds;
				offset.y = ((Bounds)(ref val2)).max.y + _ceilHeight + val.y * 0.5f;
				obj3.size = val;
				((Collider2D)obj3).offset = offset;
			}
			BoxCollider2D obj4 = ((Component)this).gameObject.AddComponent<BoxCollider2D>();
			val2 = bounds;
			val.x = ((Bounds)(ref val2)).size.x + (float)(num * 2);
			val.y = num;
			val2 = bounds;
			offset.x = ((Bounds)(ref val2)).center.x;
			val2 = bounds;
			offset.y = ((Bounds)(ref val2)).min.y - val.y * 0.5f;
			obj4.size = val;
			((Collider2D)obj4).offset = offset;
			((Component)this).gameObject.AddComponent<Rigidbody2D>().bodyType = (RigidbodyType2D)2;
		}
	}

	public void SetReward(MapReward.Type rewardType)
	{
		_mapReward.type = rewardType;
		Sprite sprite = null;
		if (_mapReward.hasReward)
		{
			Chapter currentChapter = Singleton<Service>.Instance.levelManager.currentChapter;
			sprite = ((rewardType == MapReward.Type.Adventurer) ? currentChapter.gateChoiceTable : currentChapter.gateTable);
		}
		_gateTable.sprite = sprite;
		_mapReward.LoadReward();
	}

	public void SetExits(PathNode node1, PathNode node2)
	{
		if ((Object)(object)_gate1 == (Object)null || (Object)(object)_gate2 == (Object)null)
		{
			return;
		}
		((Renderer)((Component)_gate1).GetComponent<SpriteRenderer>()).sortingOrder = -1;
		((Renderer)((Component)_gate2).GetComponent<SpriteRenderer>()).sortingOrder = -2;
		if (node1.gate == node2.gate && node1.gate != Gate.Type.Boss)
		{
			if (MMMaths.RandomBool())
			{
				_gate1.Set(node1.gate, NodeIndex.Node1);
				_gate2.ShowDestroyed(node1.gate == Gate.Type.Terminal);
			}
			else
			{
				_gate1.ShowDestroyed(node1.gate == Gate.Type.Terminal);
				_gate2.Set(node2.gate, NodeIndex.Node2);
			}
			return;
		}
		if (node1.gate == Gate.Type.None)
		{
			_gate1.ShowDestroyed(node1.gate == Gate.Type.Terminal);
		}
		else
		{
			_gate1.Set(node1.gate, NodeIndex.Node1);
		}
		if (node2.gate == Gate.Type.None)
		{
			_gate2.ShowDestroyed(node2.gate == Gate.Type.Terminal);
		}
		else
		{
			_gate2.Set(node2.gate, NodeIndex.Node2);
		}
	}

	public bool IsInMap(Vector3 position)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		Bounds val = bounds;
		Vector3 min = ((Bounds)(ref val)).min;
		val = bounds;
		Vector3 max = ((Bounds)(ref val)).max;
		bool num = position.x > min.x && position.x < max.x;
		bool flag = position.y > min.y && ((_hasCeil && position.y < max.y + _ceilHeight) || !_hasCeil);
		return num && flag;
	}

	public void ChangeLight(Color color, float intensity, float seconds)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0024: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		((CoroutineReference)(ref _lightLerpReference)).Stop();
		_lightLerpReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CLerp(_globalLight.color, _globalLight.intensity, color, intensity, seconds));
	}

	public void SetCameraZoneOrDefault()
	{
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		GameBase instance = Scene<GameBase>.instance;
		if ((Object)(object)_cameraZone == (Object)null)
		{
			_cameraZone = ((Component)instance.cameraController).gameObject.AddComponent<CameraZone>();
			_cameraZone.bounds = bounds;
			_cameraZone.hasCeil = _hasCeil;
			Vector3 max = ((Bounds)(ref _cameraZone.bounds)).max;
			max.y += _ceilHeight;
			((Bounds)(ref _cameraZone.bounds)).max = max;
			instance.cameraController.zone = _cameraZone;
			instance.minimapCameraController.zone = _cameraZone;
		}
		instance.cameraController.zone = _cameraZone;
		instance.minimapCameraController.zone = _cameraZone;
	}

	public void ResetCameraZone()
	{
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0061: Unknown result type (might be due to invalid IL or missing references)
		//IL_0066: Unknown result type (might be due to invalid IL or missing references)
		//IL_0083: Unknown result type (might be due to invalid IL or missing references)
		GameBase instance = Scene<GameBase>.instance;
		if ((Object)(object)((Component)instance.cameraController).gameObject.GetComponent<CameraZone>() == (Object)null)
		{
			_cameraZone = ((Component)instance.cameraController).gameObject.AddComponent<CameraZone>();
		}
		_cameraZone.bounds = bounds;
		_cameraZone.hasCeil = _hasCeil;
		Vector3 max = ((Bounds)(ref _cameraZone.bounds)).max;
		max.y += _ceilHeight;
		((Bounds)(ref _cameraZone.bounds)).max = max;
		instance.cameraController.zone = _cameraZone;
		instance.minimapCameraController.zone = _cameraZone;
	}

	public void RestoreLight(float seconds)
	{
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_003b: Unknown result type (might be due to invalid IL or missing references)
		((CoroutineReference)(ref _lightLerpReference)).Stop();
		_lightLerpReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CLerp(globalLight.color, globalLight.intensity, originalLightColor, originalLightIntensity, seconds));
	}

	private IEnumerator CLerp(Color colorA, float intensityA, Color colorB, float intensityB, float seconds)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		for (float t = 0f; t < 1f; t += ((ChronometerBase)Chronometer.global).deltaTime / seconds)
		{
			yield return null;
			globalLight.color = Color.Lerp(colorA, colorB, t);
			globalLight.intensity = Mathf.Lerp(intensityA, intensityB, t);
		}
	}

	private IEnumerator CCheckRewardActivating()
	{
		yield return null;
		while (waveContainer.enemyWaves.Length != 0 && (waveContainer.state == EnemyWaveContainer.State.Remain || (Object)(object)_rewardActivatingRangeFinder.OverlapCollider(_mapRewardActivatingRange).GetComponent<Target>() == (Object)null))
		{
			yield return null;
		}
		waveContainer.Stop();
		if (_mapReward.Activate())
		{
			_mapReward.onLoot += ActivateGates;
			Singleton<Service>.Instance.levelManager.InvokeOnActivateMapReward();
		}
		else
		{
			ActivateGates();
		}
		void ActivateGates()
		{
			if ((Object)(object)_gate1 != (Object)null)
			{
				_gate1.Activate();
			}
			if ((Object)(object)_gate2 != (Object)null)
			{
				_gate2.Activate();
			}
		}
	}
}
