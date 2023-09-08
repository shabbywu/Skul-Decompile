using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace FX;

public class Vignette : MonoBehaviour
{
	[SerializeField]
	private PoolObject _poolObject;

	[SerializeField]
	private RectTransform _rectTransform;

	[SerializeField]
	private Image _image;

	public void Initialize(Color startColor, Color endColor, Curve curve)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0036: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		((Transform)_rectTransform).localScale = Vector3.one;
		((Transform)_rectTransform).localPosition = Vector3.zero;
		_rectTransform.offsetMax = Vector2.zero;
		_rectTransform.offsetMin = Vector2.zero;
		((MonoBehaviour)this).StartCoroutine(CFade(startColor, endColor, curve));
	}

	private IEnumerator CFade(Color startColor, Color endColor, Curve curve)
	{
		//IL_000e: Unknown result type (might be due to invalid IL or missing references)
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_0016: Unknown result type (might be due to invalid IL or missing references)
		float duration = curve.duration;
		for (float time = 0f; time < duration; time += Chronometer.global.deltaTime)
		{
			((Graphic)_image).color = Color.Lerp(startColor, endColor, curve.Evaluate(time));
			yield return null;
		}
		_poolObject.Despawn();
	}
}
