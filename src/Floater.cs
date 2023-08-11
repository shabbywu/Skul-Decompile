using System;
using UnityEngine;

public class Floater : MonoBehaviour
{
	[SerializeField]
	private float _amplitude = 0.2f;

	[SerializeField]
	private float _frequency = 1f;

	private Vector3 _originalPosition;

	private Vector3 _floatingPosition;

	private void Start()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_originalPosition = ((Component)this).transform.position;
	}

	private void Update()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		_floatingPosition = _originalPosition;
		_floatingPosition.y += Mathf.Sin(Time.fixedTime * (float)Math.PI * _frequency) * _amplitude;
		((Component)this).transform.position = _floatingPosition;
	}
}
