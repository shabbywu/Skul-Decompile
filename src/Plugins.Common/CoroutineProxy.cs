using UnityEngine;

public class CoroutineProxy : MonoBehaviour
{
	private static CoroutineProxy _instance;

	public static CoroutineProxy instance
	{
		get
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_0022: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Expected O, but got Unknown
			if ((Object)(object)_instance == (Object)null)
			{
				GameObject val = new GameObject("CoroutineProxy");
				_instance = val.AddComponent<CoroutineProxy>();
				Object.DontDestroyOnLoad((Object)val);
				((Object)val).hideFlags = (HideFlags)61;
			}
			return _instance;
		}
	}
}
