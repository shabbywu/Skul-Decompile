using UnityEngine;

public class GlobalTransform : MonoBehaviour
{
	private void Awake()
	{
		((Component)this).transform.SetParent((Transform)null);
	}
}
