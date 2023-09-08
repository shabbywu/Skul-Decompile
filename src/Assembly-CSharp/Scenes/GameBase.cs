using System.Collections;
using FX;
using Level;
using UI;
using UnityEngine;

namespace Scenes;

public class GameBase : Scene<GameBase>
{
	[SerializeField]
	private UIManager _uiManager;

	[SerializeField]
	private Camera _camera;

	[SerializeField]
	private CameraController _cameraController;

	[SerializeField]
	private CameraController _minimapCameraController;

	[SerializeField]
	private GameFadeInOut _gameFadeInOut;

	[SerializeField]
	private PoolObjectContainer _poolObjectContainer;

	private ParallaxBackground _background;

	public UIManager uiManager => _uiManager;

	public Camera camera => _camera;

	public CameraController cameraController => _cameraController;

	public CameraController minimapCameraController => _minimapCameraController;

	public GameFadeInOut gameFadeInOut => _gameFadeInOut;

	public PoolObjectContainer poolObjectContainer => _poolObjectContainer;

	public void SetBackground(ParallaxBackground background, float originHeight)
	{
		//IL_005f: Unknown result type (might be due to invalid IL or missing references)
		//IL_006a: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_background != (Object)null)
		{
			Object.Destroy((Object)(object)((Component)_background).gameObject);
		}
		if (!((Object)(object)background == (Object)null))
		{
			_background = Object.Instantiate<ParallaxBackground>(background, ((Component)_cameraController).transform);
			((Component)_background).transform.localPosition = new Vector3(0f, 0f, 0f - ((Component)cameraController).transform.localPosition.z);
			((MonoBehaviour)this).StartCoroutine(CInitialize(originHeight));
		}
	}

	public void ChangeBackgroundWithFade(ParallaxBackground background, float originHeight)
	{
		//IL_0060: Unknown result type (might be due to invalid IL or missing references)
		//IL_006b: Unknown result type (might be due to invalid IL or missing references)
		if ((Object)(object)_background == (Object)null)
		{
			SetBackground(background, originHeight);
			return;
		}
		((MonoBehaviour)this).StartCoroutine(CDestroy(_background));
		_background = Object.Instantiate<ParallaxBackground>(background, ((Component)_cameraController).transform);
		((Component)_background).transform.localPosition = new Vector3(0f, 0f, 0f - ((Component)cameraController).transform.localPosition.z);
		((MonoBehaviour)this).StartCoroutine(CInitialize(originHeight));
		_background.FadeOut();
		static IEnumerator CDestroy(ParallaxBackground parallax)
		{
			yield return parallax.CFadeIn();
			Object.Destroy((Object)(object)((Component)parallax).gameObject);
		}
	}

	private IEnumerator CInitialize(float originHeight)
	{
		yield return (object)new WaitForEndOfFrame();
		yield return (object)new WaitForEndOfFrame();
		yield return (object)new WaitForEndOfFrame();
		yield return (object)new WaitForEndOfFrame();
		yield return (object)new WaitForEndOfFrame();
		_background.Initialize(originHeight);
	}
}
