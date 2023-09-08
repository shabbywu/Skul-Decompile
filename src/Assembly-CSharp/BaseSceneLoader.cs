using Scenes;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BaseSceneLoader : MonoBehaviour
{
	private void Awake()
	{
		if ((Object)(object)Scene<Base>.instance == (Object)null)
		{
			SceneManager.LoadScene("Base", (LoadSceneMode)1);
		}
	}
}
