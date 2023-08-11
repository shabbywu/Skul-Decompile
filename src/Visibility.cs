using System;
using UnityEngine;

public class Visibility : MonoBehaviour
{
	private const float outOfCameraZPosition = -100f;

	public bool visible { get; private set; } = true;


	public event Action<bool> onChanged;

	public void SetVisible(bool visible)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		if (this.visible != visible)
		{
			this.visible = visible;
			Vector3 position = ((Component)this).transform.position;
			position.z = (visible ? 0f : (-100f));
			((Component)this).transform.position = position;
			this.onChanged?.Invoke(visible);
		}
	}
}
