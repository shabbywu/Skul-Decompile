using System;
using System.Collections;
using Scenes;
using UnityEngine;

namespace FX;

public class ScreenFlash : MonoBehaviour
{
	[Serializable]
	public class Info
	{
		public enum SortingOrder
		{
			Frontmost,
			Rearmost
		}

		[SerializeField]
		private Color _color = Color.black;

		[Tooltip("페이드인이 완료된 후 지속될 시간")]
		[SerializeField]
		private float _duration;

		[SerializeField]
		[SortingLayer]
		[Space]
		private int _sortingLayer;

		[SerializeField]
		private SortingOrder _sortingOrder;

		[Space]
		[SerializeField]
		private AnimationCurve _fadeIn;

		[SerializeField]
		private float _fadeInDuration;

		[Space]
		[SerializeField]
		private AnimationCurve _fadeOut;

		[SerializeField]
		private float _fadeOutDuration;

		public Color color => _color;

		public float duration => _duration;

		public int sortingLayer => _sortingLayer;

		public SortingOrder sortingOrder => _sortingOrder;

		public AnimationCurve fadeIn => _fadeIn;

		public float fadeInDuration => _fadeInDuration;

		public AnimationCurve fadeOut => _fadeOut;

		public float fadeOutDuration => _fadeOutDuration;
	}

	[SerializeField]
	private PoolObject _poolObject;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	private float _pixelPerUnit;

	private float _width;

	private float _height;

	private Info _info;

	private float _fadedPercent;

	public SpriteRenderer spriteRenderer => _spriteRenderer;

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

	private void FullScreen()
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0005: Unknown result type (might be due to invalid IL or missing references)
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_0095: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0071: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_009d: Unknown result type (might be due to invalid IL or missing references)
		//IL_009f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 zero = Vector3.zero;
		zero.z = 10f;
		((Component)this).transform.localPosition = zero;
		Camera camera = Scene<GameBase>.instance.camera;
		float num = camera.orthographicSize * 2f;
		Vector2 val = default(Vector2);
		((Vector2)(ref val))._002Ector(camera.aspect * num, num);
		Vector2 one = Vector2.one;
		one = ((!(val.x >= val.y)) ? (one * (val.y * _pixelPerUnit / _height)) : (one * (val.x * _pixelPerUnit / _width)));
		((Component)this).transform.localScale = Vector2.op_Implicit(one);
	}

	public void Play(Info info)
	{
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		FullScreen();
		_info = info;
		_fadedPercent = 0f;
		((Renderer)_spriteRenderer).sortingLayerID = info.sortingLayer;
		if (info.sortingOrder == Info.SortingOrder.Frontmost)
		{
			((Renderer)_spriteRenderer).sortingOrder = 32767;
		}
		else
		{
			((Renderer)_spriteRenderer).sortingOrder = -32768;
		}
		_spriteRenderer.color = info.color;
		((MonoBehaviour)this).StartCoroutine(CPlay());
	}

	private IEnumerator CPlay()
	{
		yield return CFadeIn(_info.fadeIn, _info.fadeInDuration);
		yield return Chronometer.global.WaitForSeconds(_info.duration);
		yield return CFadeOut(_info.fadeOut, _info.fadeOutDuration);
	}

	public void FadeOut()
	{
		((MonoBehaviour)this).StopAllCoroutines();
		((MonoBehaviour)this).StartCoroutine(CFadeOut(_info.fadeOut, _info.fadeOutDuration));
	}

	private IEnumerator CFadeIn(AnimationCurve curve, float duration)
	{
		Color color = _spriteRenderer.color;
		float alpha = _info.color.a;
		if (duration > 0f)
		{
			while (_fadedPercent < 1f)
			{
				color.a = alpha * Mathf.LerpUnclamped(0f, 1f, curve.Evaluate(_fadedPercent));
				_spriteRenderer.color = color;
				yield return null;
				_fadedPercent += Chronometer.global.deltaTime / duration;
			}
		}
		_fadedPercent = 1f;
		color.a = alpha;
		_spriteRenderer.color = color;
	}

	private IEnumerator CFadeOut(AnimationCurve curve, float duration)
	{
		Color color = _spriteRenderer.color;
		float alpha = _info.color.a;
		if (duration > 0f)
		{
			while (_fadedPercent > 0f)
			{
				color.a = alpha * Mathf.LerpUnclamped(0f, 1f, curve.Evaluate(_fadedPercent));
				_spriteRenderer.color = color;
				yield return null;
				_fadedPercent -= Chronometer.global.deltaTime / duration;
			}
		}
		_fadedPercent = 0f;
		_poolObject.Despawn();
	}
}
