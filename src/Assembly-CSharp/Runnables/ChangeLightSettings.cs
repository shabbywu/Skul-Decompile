using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

namespace Runnables;

public class ChangeLightSettings : CRunnable
{
	[SerializeField]
	private Light2D _light;

	[SerializeField]
	private Curve _curve;

	[ColorUsage(false)]
	[SerializeField]
	private Color _color;

	[SerializeField]
	private float _intensity;

	public override IEnumerator CRun()
	{
		Color startColor = _light.color;
		float startIntensity = _light.intensity;
		float elapsed = 0f;
		while (elapsed < _curve.duration)
		{
			elapsed += Chronometer.global.deltaTime;
			float num = _curve.Evaluate(elapsed);
			_light.color = Color.Lerp(startColor, _color, num);
			_light.intensity = Mathf.Lerp(startIntensity, _intensity, num);
			yield return null;
		}
		_light.color = _color;
		_light.intensity = _intensity;
	}
}
