using System;
using System.Collections;
using UnityEngine;

namespace Level;

public class FloatingObject2 : MonoBehaviour
{
	[SerializeField]
	[Header("Initial Value")]
	private bool _startOnEnable = true;

	[SerializeField]
	private float _vertical;

	[SerializeField]
	private float _rotation;

	[SerializeField]
	private Curve _translateDegree;

	[SerializeField]
	private bool _randomize;

	[SerializeField]
	private float _speed = 1f;

	private void OnEnable()
	{
		if (_startOnEnable)
		{
			((MonoBehaviour)this).StartCoroutine(CFloat());
		}
	}

	public void Float()
	{
		((MonoBehaviour)this).StartCoroutine(CFloat());
	}

	private IEnumerator CFloat()
	{
		float time = 0f;
		float amountStart = (_randomize ? Random.Range(0f, 1f) : 0f);
		while (true)
		{
			yield return null;
			float amount = amountStart + time * _speed;
			UpdateTransform(amount, _translateDegree.Evaluate(time));
			time += ((ChronometerBase)Chronometer.global).deltaTime;
		}
	}

	private void UpdateTransform(float amount, float degree)
	{
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_0052: Unknown result type (might be due to invalid IL or missing references)
		float sinAmplitudeIn = GetSinAmplitudeIn(amount);
		float cosAmplitudeIn = GetCosAmplitudeIn(amount);
		float num = _vertical * degree * sinAmplitudeIn;
		float num2 = _rotation * degree * cosAmplitudeIn;
		((Component)this).transform.localPosition = new Vector3(0f, num, 0f);
		((Component)this).transform.localEulerAngles = new Vector3(0f, 0f, num2);
	}

	private float GetSinAmplitudeIn(float point)
	{
		return Mathf.Sin(point * 360f * ((float)Math.PI / 180f));
	}

	private float GetCosAmplitudeIn(float point)
	{
		return Mathf.Cos(point * 360f * ((float)Math.PI / 180f));
	}
}
