using UnityEngine;

public class rotateCamera : MonoBehaviour
{
	public float turnSpeed = 50f;

	public int count;

	public int maxCount;

	public bool left;

	private void Update()
	{
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0048: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		if (left)
		{
			if (count >= maxCount)
			{
				((Component)this).transform.Rotate(Vector3.up, (0f - turnSpeed) * Time.deltaTime);
				count = 0;
				left = false;
			}
			else
			{
				((Component)this).transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
				count++;
			}
		}
		else if (count >= maxCount)
		{
			((Component)this).transform.Rotate(Vector3.up, turnSpeed * Time.deltaTime);
			count = 0;
			left = true;
		}
		else
		{
			((Component)this).transform.Rotate(Vector3.up, (0f - turnSpeed) * Time.deltaTime);
			count++;
		}
	}
}
