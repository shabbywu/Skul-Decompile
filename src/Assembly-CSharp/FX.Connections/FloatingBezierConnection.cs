using System;
using System.Collections;
using UnityEngine;

namespace FX.Connections;

[RequireComponent(typeof(BezierCurve))]
public class FloatingBezierConnection : Connection
{
	[SerializeField]
	[GetComponent]
	private BezierCurve _bezierCurve;

	[SerializeField]
	private float _middleYOffset = 5f;

	[SerializeField]
	private float _floatingRange = 5f;

	[SerializeField]
	private float _floatingSpeed = 0.2f;

	[SerializeField]
	private float _trackingSpeed = 1.3f;

	[SerializeField]
	private float _middleTrackingSpeed = 0.016f;

	private const float _speedCorrection = 6f;

	protected override void Show()
	{
		((Component)this).gameObject.SetActive(true);
		((MonoBehaviour)this).StartCoroutine(CShow());
	}

	protected override void Hide()
	{
		((MonoBehaviour)this).StopAllCoroutines();
		((Component)this).gameObject.SetActive(false);
	}

	private IEnumerator CShow()
	{
		Vector3 startCurrent = base.startPosition;
		Vector3 endCurrent = base.endPosition;
		int middleCount = _bezierCurve.count - 2;
		Vector2[] middleCurrents = (Vector2[])(object)new Vector2[middleCount];
		Vector2[] randomOffsets = (Vector2[])(object)new Vector2[middleCount];
		float floatingTime = 0f;
		for (int i = 0; i < middleCount; i++)
		{
			middleCurrents[i] = GetMiddlePosition(i);
			randomOffsets[i] = new Vector2(Random.Range(0f, 360f), Random.Range(0f, 360f));
		}
		while (!lostConnection)
		{
			startCurrent = Vector2.op_Implicit(GetTrackingVector(Vector2.op_Implicit(startCurrent), Vector2.op_Implicit(base.startPosition), _trackingSpeed));
			endCurrent = Vector2.op_Implicit(GetTrackingVector(Vector2.op_Implicit(endCurrent), Vector2.op_Implicit(base.endPosition), _trackingSpeed));
			for (int j = 0; j < middleCount; j++)
			{
				Vector2 middlePosition = GetMiddlePosition(j);
				middlePosition.y += _middleYOffset;
				middlePosition += GetFloatingOffset(floatingTime, randomOffsets[j]);
				middleCurrents[j] = GetTrackingVector(middleCurrents[j], middlePosition, _middleTrackingSpeed);
			}
			floatingTime += _floatingSpeed * 360f * Chronometer.global.deltaTime;
			_bezierCurve.SetStart(Vector2.op_Implicit(startCurrent));
			_bezierCurve.SetEnd(Vector2.op_Implicit(endCurrent));
			for (int k = 0; k < middleCount; k++)
			{
				_bezierCurve.SetVector(k + 1, middleCurrents[k]);
			}
			_bezierCurve.UpdateCurve();
			yield return null;
		}
		Disconnect();
	}

	private Vector2 GetTrackingVector(Vector2 current, Vector2 target, float speed)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_0029: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		Vector2 val = (target - current) * Mathf.Min(1f, Chronometer.global.deltaTime * 6f * speed);
		return current + val;
	}

	private Vector2 GetMiddlePosition(int index)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		index++;
		float num = (float)index / (float)(_bezierCurve.count - 1);
		return Vector2.Lerp(Vector2.op_Implicit(base.startPosition), Vector2.op_Implicit(base.endPosition), num);
	}

	private Vector2 GetFloatingOffset(float floatingTime, Vector2 randomOffset)
	{
		//IL_0009: Unknown result type (might be due to invalid IL or missing references)
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		Vector2 result = default(Vector2);
		result.x = _floatingRange * Mathf.Sin((floatingTime + randomOffset.x) * ((float)Math.PI / 180f));
		result.y = _floatingRange * Mathf.Sin((floatingTime + randomOffset.y) * ((float)Math.PI / 180f));
		return result;
	}
}
