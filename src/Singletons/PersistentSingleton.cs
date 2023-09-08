using UnityEngine;

namespace Singletons;

public class PersistentSingleton<T> : MonoBehaviour where T : Component
{
	protected static T _instance;

	public static T Instance
	{
		get
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)_instance == (Object)null)
			{
				_instance = Object.FindObjectOfType<T>();
				if ((Object)(object)_instance == (Object)null)
				{
					_instance = new GameObject().AddComponent<T>();
				}
			}
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		if ((Object)(object)_instance == (Object)null)
		{
			_instance = (T)(object)((this is T) ? this : null);
			Object.DontDestroyOnLoad((Object)(object)((Component)((Component)this).transform).gameObject);
		}
		else if ((Object)(object)this != (Object)(object)_instance)
		{
			Object.Destroy((Object)(object)((Component)this).gameObject);
		}
	}
}
