using System.Collections;
using UnityEngine;
using UnityEngine.Experimental.Rendering.Universal;

public class GodRay : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Light2D _light;

	[SerializeField]
	private float _showRange = 10f;

	[SerializeField]
	private float _minIntensity;

	[SerializeField]
	private Curve _intensityCurve;

	[SerializeField]
	[Range(0f, 1f)]
	private float _noiseBlend = 1f;

	[SerializeField]
	private float _noisePower = 1f;

	[SerializeField]
	private float _noiseShiftSpeed = 0.1f;

	private void Awake()
	{
		_showRange = Mathf.Max(_showRange, 0.01f);
		((MonoBehaviour)this).StartCoroutine(CModifyIntensity());
	}

	private IEnumerator CModifyIntensity()
	{
		float defaultIntensity = _light.intensity;
		_ = _noiseBlend;
		_ = _noiseBlend;
		float noiseShift = Random.value;
		while (true)
		{
			float num = ((Component)Camera.main).transform.position.x - ((Component)this).transform.position.x;
			float time = 1f - Mathf.Clamp01(Mathf.Abs(num) / _showRange);
			float num2 = _intensityCurve.Evaluate(time);
			float num3 = Mathf.PerlinNoise(num / _showRange * 0.5f * _noisePower, noiseShift * _noisePower);
			num3 = 1f - _noiseBlend + num3 * _noiseBlend;
			_light.intensity = _minIntensity + (defaultIntensity - _minIntensity) * num2 * num3;
			noiseShift += Chronometer.global.deltaTime * _noiseShiftSpeed;
			yield return null;
		}
	}
}
