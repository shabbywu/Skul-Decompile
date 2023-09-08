using UnityEngine;

public class PlatformSpecifier : MonoBehaviour
{
	[SerializeField]
	private bool _standalone = true;

	[SerializeField]
	private bool _console = true;

	private void Awake()
	{
		if (Application.isConsolePlatform && !_console)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
		else if (!Application.isConsolePlatform && !_standalone)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
	}
}
