using UnityEngine;
using UnityEngine.UI;

namespace TwoDLaserPack;

public class RotationScript : MonoBehaviour
{
	public Slider hSlider;

	public Transform pivot;

	public bool rotationEnabled;

	public float rotationAmount;

	private Transform transformCached;

	private void Start()
	{
		transformCached = ((Component)this).transform;
	}

	private void Update()
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		if (rotationEnabled)
		{
			transformCached.RotateAround(pivot.localPosition, Vector3.forward, rotationAmount);
		}
	}

	public void OnHSliderChanged()
	{
		rotationAmount = hSlider.value;
	}
}
