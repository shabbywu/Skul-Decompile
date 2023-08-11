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
	private Method _easingMehtod;

	private void OnEnable()
	{
		((MonoBehaviour)this).StartCoroutine(CFadeOut());
	}

	private IEnumerator CFadeOut()
	{
		Color color = Color.white;
		Function easeFunction = EasingFunction.GetEasingFunction(_easingMehtod);
		float t = 0f;
		while (t <= _duration)
		{
			t += Time.deltaTime;
			color.a = easeFunction.Invoke(1f, 0f, t);
			_spriteRenderer.color = color;
			yield return null;
		}
	}
}
