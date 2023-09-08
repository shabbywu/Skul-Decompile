using System.Collections;
using Scenes;
using UnityEngine;

namespace FX;

public class GameFadeInOut : MonoBehaviour
{
	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	private Color _color = Color.black;

	private float _pixelPerUnit;

	private float _width;

	private float _height;

	private void Awake()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		_pixelPerUnit = _spriteRenderer.sprite.pixelsPerUnit;
		Rect rect = _spriteRenderer.sprite.rect;
		_width = ((Rect)(ref rect)).width;
		rect = _spriteRenderer.sprite.rect;
		_height = ((Rect)(ref rect)).height;
	}

	public void SetFadeColor(Color color)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		_color = color;
	}

	private void SetFadeAlpha(float alpha)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		_color.a = alpha;
		_spriteRenderer.color = _color;
	}

	private void FullScreen()
	{
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0059: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_006e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0073: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		Camera camera = Scene<GameBase>.instance.camera;
		float num = camera.orthographicSize * 2f;
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(camera.aspect * num, num);
		Vector2 one = Vector2.one;
		one = ((!(val.x >= val.y)) ? (one * (val.y * _pixelPerUnit / _height)) : (one * (val.x * _pixelPerUnit / _width)));
		((Component)this).transform.localScale = Vector2.op_Implicit(one);
	}

	public void FadeIn(float speed = 1f)
	{
		Activate();
		((MonoBehaviour)this).StartCoroutine(CFadeIn(speed));
	}

	public void Activate()
	{
		((Component)this).gameObject.SetActive(true);
	}

	public void Deactivate()
	{
		((Component)this).gameObject.SetActive(false);
	}

	public IEnumerator CFadeIn(float speed)
	{
		FullScreen();
		for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime * speed)
		{
			SetFadeAlpha(1f - t);
			yield return null;
		}
		SetFadeAlpha(0f);
	}

	public void FadeOut(float speed = 1f)
	{
		Activate();
		((MonoBehaviour)this).StartCoroutine(CFadeOut(speed));
	}

	public IEnumerator CFadeOut(float speed)
	{
		FullScreen();
		for (float t = 0f; t < 1f; t += Time.unscaledDeltaTime * speed)
		{
			SetFadeAlpha(t);
			yield return null;
		}
		SetFadeAlpha(1f);
	}
}
