using UnityEngine;

public class LoadGameResource : MonoBehaviour
{
	[SerializeField]
	private bool _waitForCompletion;

	private void Awake()
	{
		GameResourceLoader.Load();
		if (_waitForCompletion)
		{
			GameResourceLoader.instance.WaitForCompletion();
		}
	}
}
