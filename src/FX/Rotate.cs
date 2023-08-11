using UnityEngine;

namespace FX;

public class Rotate : MonoBehaviour
{
	[SerializeField]
	private float _amount;

	private void Update()
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.Rotate(Vector3.forward, _amount * ((ChronometerBase)Chronometer.global).deltaTime, (Space)1);
	}
}
