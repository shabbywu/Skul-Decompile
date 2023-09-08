using System.Collections;
using UnityEngine;

public class SpriteFadeOut : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private EasingFunction.Method _easingMehtod;

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CFadeOut());
	}

	private IEnumerator CFadeOut()
	{
		Color color = Color.white;
		EasingFunction.Function easeFunction = EasingFunction.GetEasingFunction(_easingMehtod);
		float t = 0f;
		while (t <= _duration)
		{
			t += Time.deltaTime;
			color.a = easeFunction(1f, 0f, t);
			_spriteRenderer.color = color;
			yield return null;
		}
	}
}
