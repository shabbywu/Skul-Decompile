using System.Collections;
using Data;
using Level;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadTestChapter : MonoBehaviour
{
	private IEnumerator Start()
	{
		GameData.Initialize();
		SceneManager.LoadScene("Base", (LoadSceneMode)1);
		yield return null;
		GameResourceLoader.Load();
		GameResourceLoader.instance.WaitForCompletion();
		yield return null;
		Singleton<Service>.Instance.levelManager.Load(Chapter.Type.Test);
	}
}
