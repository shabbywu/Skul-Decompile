using UnityEngine;

public sealed class DontDestroyOnLoad : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad((Object)(object)((Component)this).gameObject);
	}
}
