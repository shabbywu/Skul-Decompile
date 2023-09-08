using UnityEngine;

namespace Singletons;

public class PersistentHumbleSingleton<T> : MonoBehaviour where T : Component
{
	protected static T _instance;

	public float InitializationTime;

	public static T Instance
	{
		get
		{
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0033: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)_instance == (Object)null)
			{
				_instance = Object.FindObjectOfType<T>();
				if ((Object)(object)_instance == (Object)null)
				{
					_instance = new GameObject
					{
						hideFlags = (HideFlags)61
					}.AddComponent<T>();
				}
			}
			return _instance;
		}
	}

	protected virtual void Awake()
	{
		InitializationTime = Time.time;
		Object.DontDestroyOnLoad((Object)(object)((Component)this).gameObject);
		T[] array = Object.FindObjectsOfType<T>();
		foreach (T val in array)
		{
			if ((Object)(object)val != (Object)(object)this && ((Component)val).GetComponent<PersistentHumbleSingleton<T>>().InitializationTime < InitializationTime)
			{
				Object.Destroy((Object)(object)((Component)val).gameObject);
			}
		}
		if ((Object)(object)_instance == (Object)null)
		{
			_instance = (T)(object)((this is T) ? this : null);
		}
	}
}
