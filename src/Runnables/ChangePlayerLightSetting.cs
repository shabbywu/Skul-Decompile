using Characters;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Runnables;

public sealed class ChangePlayerLightSetting : Runnable
{
	private Light2D _light;

	[SerializeField]
	private bool _rollbackOnDestroyed;

	[SerializeField]
	private float _intensity;

	private float _cachedIntensity;

	public override void Run()
	{
		if ((Object)(object)_light == (Object)null)
		{
			Character player = Singleton<Service>.Instance.levelManager.player;
			_light = ((Component)player).GetComponentInChildren<Light2D>();
		}
		_cachedIntensity = _light.intensity;
		_light.intensity = _intensity;
	}

	private void OnDestroy()
	{
		if (_rollbackOnDestroyed)
		{
			_light.intensity = _cachedIntensity;
		}
	}
}
