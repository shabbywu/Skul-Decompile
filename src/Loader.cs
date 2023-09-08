using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class Loader : MonoBehaviour
{
	[Serializable]
	private class UnityFloatEvent : UnityEvent<float>
	{
	}

	[SerializeField]
	private string[] _scenesToLoad;

	private List<AsyncOperation> _asyncOperations;

	private void Start()
	{
		((MonoBehaviour)this).StartCoroutine(CLoad());
	}

	private IEnumerator CLoad()
	{
		yield return (object)new WaitForSeconds(1f);
		_asyncOperations = new List<AsyncOperation>(_scenesToLoad.Length);
		string[] scenesToLoad = _scenesToLoad;
		foreach (string scene in scenesToLoad)
		{
			yield return CLoadScene(scene);
		}
		foreach (AsyncOperation asyncOperation in _asyncOperations)
		{
			asyncOperation.allowSceneActivation = true;
		}
		Object.Destroy((Object)(object)((Component)this).gameObject);
	}

	private IEnumerator CLoadScene(string scene)
	{
		int result;
		AsyncOperation operation = ((!int.TryParse(scene, out result)) ? SceneManager.LoadSceneAsync(scene, (LoadSceneMode)1) : SceneManager.LoadSceneAsync(result, (LoadSceneMode)1));
		_asyncOperations.Add(operation);
		operation.allowSceneActivation = false;
		while (operation.progress < 0.9f)
		{
			yield return null;
		}
	}
}
