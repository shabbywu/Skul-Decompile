using System;
using System.Collections;
using UnityEngine;

namespace Level;

public sealed class FloatingObject : MonoBehaviour
{
	[Serializable]
	private class Setting
	{
		[SerializeField]
		internal bool directionRandom;

		[SerializeField]
		internal bool awakePointRandom;

		[SerializeField]
		internal float extent;

		[SerializeField]
		internal float speed;
	}

	[Header("Initial Value")]
	[SerializeField]
	private bool _startOnEnable = true;

	[SerializeField]
	private Setting _vertical;

	[SerializeField]
	private Setting _horizontal;

	[SerializeField]
	private Setting _rotation;

	private Vector2 _awakePosition;

	private Quaternion _awakeRotation;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		_awakePosition = Vector2.op_Implicit(((Component)this).transform.localPosition);
		_awakeRotation = ((Component)this).transform.localRotation;
	}

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
		float verticalAmount = (_vertical.awakePointRandom ? Random.Range(0f, (float)Math.PI * 2f) : 0f);
		float horizontalAmount = (_horizontal.awakePointRandom ? Random.Range(0f, (float)Math.PI * 2f) : 0f);
		float rotationAmount = (_rotation.awakePointRandom ? Random.Range(0f, (float)Math.PI * 2f) : 0f);
		if (_vertical.directionRandom)
		{
			_vertical.speed *= (MMMaths.RandomBool() ? 1 : (-1));
		}
		if (_horizontal.directionRandom)
		{
			_horizontal.speed *= (MMMaths.RandomBool() ? 1 : (-1));
		}
		if (_rotation.directionRandom)
		{
			_rotation.speed *= (MMMaths.RandomBool() ? 1 : (-1));
		}
		while (true)
		{
			yield return null;
			UpdatePosition();
			UpdateRotation();
		}
		void UpdatePosition()
		{
			//IL_0091: Unknown result type (might be due to invalid IL or missing references)
			//IL_0096: Unknown result type (might be due to invalid IL or missing references)
			float deltaTime2 = ((ChronometerBase)Chronometer.global).deltaTime;
			verticalAmount += deltaTime2 * _vertical.speed;
			horizontalAmount += deltaTime2 * _horizontal.speed;
			float num2 = GetSineAmplitudeIn(horizontalAmount) * _horizontal.extent;
			float num3 = GetSineAmplitudeIn(verticalAmount) * _vertical.extent;
			((Component)this).transform.localPosition = Vector2.op_Implicit(new Vector2(_awakePosition.x + num2, _awakePosition.y + num3));
		}
		void UpdateRotation()
		{
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0056: Unknown result type (might be due to invalid IL or missing references)
			//IL_005b: Unknown result type (might be due to invalid IL or missing references)
			float deltaTime = ((ChronometerBase)Chronometer.global).deltaTime;
			rotationAmount += deltaTime * _rotation.speed;
			float num = GetSineAmplitudeIn(rotationAmount) * _rotation.extent;
			((Component)this).transform.localRotation = Quaternion.AngleAxis(num + ((Quaternion)(ref _awakeRotation)).eulerAngles.z, Vector3.forward);
		}
	}

	private float GetSineAmplitudeIn(float point)
	{
		return Mathf.Sin(point * 360f * ((float)Math.PI / 180f));
	}
}
