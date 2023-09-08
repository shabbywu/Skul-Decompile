using Data;
using Level;
using Platforms;
using Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Services;

public sealed class Service : Singleton<Service>
{
	[SerializeField]
	[GetComponent]
	private ControllerManager _controllerVibration;

	[SerializeField]
	[GetComponent]
	private LevelManager _levelManager;

	[SerializeField]
	[GetComponent]
	private GearManager _gearManager;

	[SerializeField]
	[GetComponent]
	private FloatingTextSpawner _floatingTextSpawner;

	[GetComponent]
	[SerializeField]
	private LineTextManager _lineTextManager;

	[SerializeField]
	[GetComponent]
	private FadeInOut _fadeInOut;

	public static bool quitting { get; private set; }

	public ControllerManager controllerVibation => _controllerVibration;

	public LevelManager levelManager => _levelManager;

	public GearManager gearManager => _gearManager;

	public FloatingTextSpawner floatingTextSpawner => _floatingTextSpawner;

	public LineTextManager lineTextManager => _lineTextManager;

	public FadeInOut fadeInOut => _fadeInOut;

	protected override void Awake()
	{
		base.Awake();
		GameData.Initialize();
		Application.quitting += delegate
		{
			quitting = true;
		};
		QualitySettings.vSyncCount = 1;
		Physics2D.autoSyncTransforms = false;
	}

	private void Update()
	{
		Physics2D.SyncTransforms();
	}

	public void ResetGameScene()
	{
		SceneManager.LoadScene("Main");
		levelManager.ClearEvents();
		GameData.Initialize();
		GameData.Save.instance.ResetAll();
		PoolObject.DespawnAll();
		PoolObject.Clear();
		PersistentSingleton<PlatformManager>.Instance.SaveDataToFile();
	}

	private void LateUpdate()
	{
		Physics2D.SyncTransforms();
	}
}
