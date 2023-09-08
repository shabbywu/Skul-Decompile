using UnityEngine;

public class DeactivateOnAwake : MonoBehaviour
{
	private void Awake()
	{
		((Component)this).gameObject.SetActive(false);
	}
}
