using System.Collections;
using Scenes;
using TMPro;
using UnityEngine;

public class LineText : MonoBehaviour
{
	[SerializeField]
	private TextMeshPro _text;

	[SerializeField]
	private SpriteRenderer _speechBubble;

	[SerializeField]
	private float _minWidth = 2f;

	[SerializeField]
	private float _maxWidth = 40f;

	[SerializeField]
	private float _minHeight = 25f / 32f;

	private CoroutineReference? _displayCoroutine;

	public bool finished { get; private set; }

	private void Awake()
	{
		Hide();
	}

	public void Display(string text, float duration)
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		ref CoroutineReference? displayCoroutine = ref _displayCoroutine;
		if (displayCoroutine.HasValue)
		{
			CoroutineReference valueOrDefault = displayCoroutine.GetValueOrDefault();
			((CoroutineReference)(ref valueOrDefault)).Stop();
		}
		if (!Scene<GameBase>.instance.uiManager.npcConversation.visible)
		{
			_displayCoroutine = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CDisplay(text, duration));
		}
	}

	public IEnumerator CDisplay(string text, float duration)
	{
		if (!Scene<GameBase>.instance.uiManager.npcConversation.visible)
		{
			Show(text);
			yield return Chronometer.global.WaitForSeconds(duration);
			Hide();
		}
	}

	private void Show(string text)
	{
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		finished = false;
		((TMP_Text)_text).text = text;
		_speechBubble.size = ResizeDisplayField(Mathf.Clamp(_minWidth, ((TMP_Text)_text).preferredWidth + 0.5f, _maxWidth), Mathf.Max(_minHeight, ((TMP_Text)_text).preferredHeight + 0.5f));
		((Component)_text).gameObject.SetActive(true);
		((Component)_speechBubble).gameObject.SetActive(true);
	}

	public void Hide()
	{
		finished = true;
		((TMP_Text)_text).text = "";
		((Component)_text).gameObject.SetActive(false);
		((Component)_speechBubble).gameObject.SetActive(false);
	}

	private void OnDisable()
	{
		Hide();
	}

	private Vector2 ResizeDisplayField(float width, float height)
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		return new Vector2(width, height);
	}
}
