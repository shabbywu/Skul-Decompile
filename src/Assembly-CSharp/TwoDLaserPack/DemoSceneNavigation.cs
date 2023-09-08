using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace TwoDLaserPack;

public class DemoSceneNavigation : MonoBehaviour
{
	public Button buttonNextDemo;

	private void Start()
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Expected O, but got Unknown
		((UnityEvent)buttonNextDemo.onClick).AddListener(new UnityAction(OnButtonNextDemoClick));
	}

	private void OnButtonNextDemoClick()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		Scene activeScene = SceneManager.GetActiveScene();
		int buildIndex = ((Scene)(ref activeScene)).buildIndex;
		if (buildIndex < SceneManager.sceneCount - 1)
		{
			SceneManager.LoadScene(buildIndex + 1);
		}
		else
		{
			Singleton<Service>.Instance.ResetGameScene();
		}
	}

	private void Update()
	{
	}
}
