using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
	[HideInInspector]
	[SerializeField]
	private string _scenePath;

	[SerializeField]
	private LoadSceneMode _mode;

	private void Awake()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		SceneManager.LoadScene(_scenePath, _mode);
		Object.Destroy((Object)(object)this);
	}
}
