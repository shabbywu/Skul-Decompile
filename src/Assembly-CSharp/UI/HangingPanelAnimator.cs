using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public class HangingPanelAnimator : MonoBehaviour
{
	[SerializeField]
	private Image _backgroundImage;

	[SerializeField]
	private GameObject _container;

	[SerializeField]
	private bool _startOnEnable;

	private Vector2 _appearedPosition;

	private Vector2 _disappearedPosition;

	private void Awake()
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_003f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		_appearedPosition = Vector2.op_Implicit(_container.transform.localPosition);
		_disappearedPosition = _appearedPosition;
		_disappearedPosition.y += ((Graphic)_backgroundImage).rectTransform.sizeDelta.y * ((Transform)((Graphic)_backgroundImage).rectTransform).localScale.y;
	}

	private void OnEnable()
	{
		if (_startOnEnable)
		{
			Appear();
		}
	}

	private IEnumerator CEasePosition(EasingFunction.Method method, Transform transform, Vector2 from, Vector2 to, float speed = 1f)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		float t = 0f;
		EasingFunction.Function easingFunction = EasingFunction.GetEasingFunction(method);
		_container.transform.localPosition = Vector2.op_Implicit(from);
		Vector2 val = default(Vector2);
		for (; t < 1f; t += Time.unscaledDeltaTime * speed)
		{
			val.x = easingFunction(from.x, to.x, t);
			val.y = easingFunction(from.y, to.y, t);
			_container.transform.localPosition = Vector2.op_Implicit(val);
			yield return null;
		}
		_container.transform.localPosition = Vector2.op_Implicit(to);
	}

	public void Appear()
	{
		((Component)this).gameObject.SetActive(true);
		((MonoBehaviour)this).StartCoroutine(CAppear());
	}

	public void Disappear()
	{
		((MonoBehaviour)this).StartCoroutine(CDisappear());
	}

	private IEnumerator CAppear()
	{
		_container.transform.localPosition = Vector2.op_Implicit(_disappearedPosition);
		while (LetterBox.instance.visible)
		{
			yield return null;
		}
		yield return CEasePosition(EasingFunction.Method.EaseOutBounce, _container.transform, _disappearedPosition, _appearedPosition, 0.5f);
	}

	private IEnumerator CDisappear()
	{
		_container.transform.localPosition = Vector2.op_Implicit(_appearedPosition);
		while (LetterBox.instance.visible)
		{
			yield return null;
		}
		yield return CEasePosition(EasingFunction.Method.EaseOutQuad, _container.transform, _appearedPosition, _disappearedPosition);
		((Component)this).gameObject.SetActive(false);
	}
}
