using System.Collections;
using UI;
using UnityEngine;
using UnityEngine.UI;

namespace Services;

public class FadeInOut : MonoBehaviour
{
	[SerializeField]
	private Image _fadeBackground;

	[SerializeField]
	private LoadingScreen _loadingScreen;

	[SerializeField]
	private GameObject _loading;

	private Color _color = Color.black;

	public bool fading { get; private set; }

	private void SetFadeAlpha(float alpha)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		_color.a = alpha;
		((Graphic)_fadeBackground).color = _color;
	}

	public void SetFadeColor(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_color = color;
	}

	public void FadeIn()
	{
		((MonoBehaviour)this).StartCoroutine(CFadeIn());
	}

	public IEnumerator CFadeIn()
	{
		fading = true;
		float t = 0f;
		SetFadeAlpha(1f);
		yield return null;
		for (; t < 1f; t += Time.unscaledDeltaTime * 2f)
		{
			SetFadeAlpha(1f - t);
			yield return null;
		}
		SetFadeAlpha(0f);
		fading = false;
	}

	public void FadeOut()
	{
		((MonoBehaviour)this).StartCoroutine(CFadeOut());
	}

	public IEnumerator CFadeOut()
	{
		fading = true;
		float t = 0f;
		SetFadeAlpha(0f);
		yield return null;
		for (; t < 1f; t += Time.unscaledDeltaTime * 2f)
		{
			SetFadeAlpha(t);
			yield return null;
		}
		SetFadeAlpha(1f);
	}

	public IEnumerator CShowLoadingScreen(LoadingScreen.LoadingScreenData loadingScreenData)
	{
		((Component)_loadingScreen).gameObject.SetActive(true);
		yield return _loadingScreen.CShow(loadingScreenData);
	}

	public IEnumerator CHideLoadingScreen()
	{
		yield return _loadingScreen.CHide();
		((Component)_loadingScreen).gameObject.SetActive(false);
	}

	public void ShowLoadingIcon()
	{
		_loading.SetActive(true);
	}

	public void HideLoadingIcon()
	{
		_loading.SetActive(false);
	}

	public void FadeOutImmediately()
	{
		fading = true;
		SetFadeAlpha(1f);
	}
}
