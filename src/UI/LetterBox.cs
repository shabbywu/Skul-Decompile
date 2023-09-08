using System.Collections;
using Characters.Controllers;
using Scenes;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public class LetterBox : MonoBehaviour
{
	private const float _defaultAnimationDuration = 0.4f;

	[SerializeField]
	private Image _top;

	[SerializeField]
	private Image _bottom;

	private float _originHeight;

	public static LetterBox instance => Scene<GameBase>.instance.uiManager.letterBox;

	public bool visible
	{
		get
		{
			return ((Component)this).gameObject.activeSelf;
		}
		set
		{
			((Component)this).gameObject.SetActive(value);
		}
	}

	private void Awake()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		_originHeight = ((Graphic)_top).rectTransform.sizeDelta.y;
	}

	private void OnDisable()
	{
		PlayerInput.blocked.Detach(this);
	}

	public void Appear(float duration = 0.4f)
	{
		((MonoBehaviour)this).StopAllCoroutines();
		visible = true;
		((MonoBehaviour)this).StartCoroutine(CAppear(duration));
	}

	public void Disappear(float duration = 0.4f)
	{
		if (((Component)this).gameObject.activeSelf)
		{
			((MonoBehaviour)this).StartCoroutine(CDisappear(duration));
		}
	}

	public IEnumerator CAppear(float duration = 0.4f)
	{
		PlayerInput.blocked.Attach(this);
		Scene<GameBase>.instance.uiManager.headupDisplay.visible = false;
		visible = true;
		float elapsed = 0f;
		float source = 0f;
		float destination = _originHeight;
		while (true)
		{
			float num = Mathf.Lerp(source, destination, elapsed / duration);
			((Graphic)_top).rectTransform.sizeDelta = new Vector2(((Graphic)_top).rectTransform.sizeDelta.x, num);
			((Graphic)_bottom).rectTransform.sizeDelta = new Vector2(((Graphic)_bottom).rectTransform.sizeDelta.x, num);
			if (!(elapsed > duration))
			{
				elapsed += Chronometer.global.deltaTime;
				yield return null;
				continue;
			}
			break;
		}
	}

	public IEnumerator CDisappear(float duration = 0.4f)
	{
		Scene<GameBase>.instance.uiManager.headupDisplay.visible = true;
		float elapsed = 0f;
		float destination = 0f;
		while (true)
		{
			float num = Mathf.Lerp(_originHeight, destination, elapsed / duration);
			((Graphic)_top).rectTransform.sizeDelta = new Vector2(((Graphic)_top).rectTransform.sizeDelta.x, num);
			((Graphic)_bottom).rectTransform.sizeDelta = new Vector2(((Graphic)_bottom).rectTransform.sizeDelta.x, num);
			if (elapsed > duration)
			{
				break;
			}
			elapsed += Chronometer.global.deltaTime;
			yield return null;
		}
		visible = false;
		PlayerInput.blocked.Detach(this);
	}
}
