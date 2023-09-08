using System;
using System.Collections;
using Scenes;
using UnityEngine;

namespace Level;

public class ParallaxBackground : MonoBehaviour
{
	[Serializable]
	private class Element
	{
		[Serializable]
		internal class Reorderable : ReorderableArray<Element>
		{
		}

		[SerializeField]
		private SpriteRenderer _spriteRenderer;

		[SerializeField]
		private bool _randomize = true;

		[SerializeField]
		private Vector2 _distance;

		[SerializeField]
		private float _hotizontalAutoScroll;

		private Vector2 _spriteSize;

		private SpriteRenderer[] _rendererInstances;

		private Vector2[] _origin;

		private Vector2 _translated;

		private float _mostLeft;

		private float _mostRight;

		internal void Initialize()
		{
			//IL_000c: Unknown result type (might be due to invalid IL or missing references)
			//IL_0011: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0019: Unknown result type (might be due to invalid IL or missing references)
			//IL_001e: Unknown result type (might be due to invalid IL or missing references)
			//IL_010b: Unknown result type (might be due to invalid IL or missing references)
			//IL_0110: Unknown result type (might be due to invalid IL or missing references)
			//IL_0129: Unknown result type (might be due to invalid IL or missing references)
			//IL_012e: Unknown result type (might be due to invalid IL or missing references)
			Bounds bounds = _spriteRenderer.sprite.bounds;
			_spriteSize = Vector2.op_Implicit(((Bounds)(ref bounds)).size);
			int num = Mathf.CeilToInt(80f / _spriteSize.x);
			_origin = (Vector2[])(object)new Vector2[num];
			_rendererInstances = (SpriteRenderer[])(object)new SpriteRenderer[num];
			_rendererInstances[0] = _spriteRenderer;
			for (int i = 1; i < _rendererInstances.Length; i++)
			{
				_rendererInstances[i] = Object.Instantiate<SpriteRenderer>(_spriteRenderer, ((Component)_spriteRenderer).transform.parent);
			}
			_mostRight = (float)(_rendererInstances.Length - 1) * _spriteSize.x / 2f;
			_mostLeft = 0f - _mostRight;
			for (int j = 0; j < _rendererInstances.Length; j++)
			{
				_origin[j] = new Vector2(_mostLeft + _spriteSize.x * (float)j - 1f / 32f * (float)j, _spriteSize.y / 2f - Camera.main.orthographicSize);
				((Component)_rendererInstances[j]).transform.localPosition = Vector2.op_Implicit(_origin[j]);
			}
			if (_randomize)
			{
				_translated.x += Random.Range(0f, _spriteSize.x);
			}
		}

		internal void Update(Vector2 delta, float deltaTime)
		{
			//IL_0013: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_001f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_0028: Unknown result type (might be due to invalid IL or missing references)
			//IL_0029: Unknown result type (might be due to invalid IL or missing references)
			//IL_002e: Unknown result type (might be due to invalid IL or missing references)
			//IL_008e: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			//IL_009e: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
			delta.x += _hotizontalAutoScroll * deltaTime;
			delta = Vector2.Scale(delta, _distance);
			_translated -= delta;
			if (_translated.x < _mostLeft)
			{
				_translated.x = _mostRight;
			}
			if (_translated.x > _mostRight)
			{
				_translated.x = _mostLeft;
			}
			for (int i = 0; i < _rendererInstances.Length; i++)
			{
				SpriteRenderer obj = _rendererInstances[i];
				Vector2 val = _origin[i] + _translated;
				((Component)obj).transform.localPosition = Vector2.op_Implicit(val);
			}
		}

		internal void SetAlpha(float alpha)
		{
			//IL_000f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0014: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			//IL_0023: Unknown result type (might be due to invalid IL or missing references)
			//IL_002f: Unknown result type (might be due to invalid IL or missing references)
			SpriteRenderer[] rendererInstances = _rendererInstances;
			Color color2 = default(Color);
			foreach (SpriteRenderer obj in rendererInstances)
			{
				Color color = obj.color;
				((Color)(ref color2))._002Ector(color.r, color.g, color.b, alpha);
				obj.color = color2;
			}
		}
	}

	private const float _screenWidth = 2560f;

	private const float _pixelPerUnit = 32f;

	private const float _pixel = 1f / 32f;

	[SerializeField]
	private Element.Reorderable _elements;

	private CameraController _cameraController;

	private void Awake()
	{
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		Element[] values = _elements.values;
		for (int i = 0; i < values.Length; i++)
		{
			values[i].Initialize();
		}
		_cameraController = Scene<GameBase>.instance.cameraController;
		UpdateElements(_cameraController.delta, Chronometer.global.deltaTime);
	}

	private void Update()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		UpdateElements(_cameraController.delta, Chronometer.global.deltaTime);
	}

	public void Initialize(float originHeight)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		UpdateElements(new Vector3(0f, originHeight, 0f), 0f);
	}

	private void UpdateElements(Vector3 delta, float deltaTime)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		Element[] values = _elements.values;
		for (int i = 0; i < values.Length; i++)
		{
			values[i].Update(Vector2.op_Implicit(delta), deltaTime);
		}
	}

	private void SetFadeAlpha(float alpha)
	{
		Element[] values = _elements.values;
		for (int i = 0; i < values.Length; i++)
		{
			values[i].SetAlpha(alpha);
		}
	}

	public void FadeIn()
	{
		((MonoBehaviour)this).StartCoroutine(CFadeIn());
	}

	public IEnumerator CFadeIn()
	{
		float t = 0f;
		SetFadeAlpha(1f);
		yield return null;
		for (; t < 1f; t += Time.unscaledDeltaTime * 0.3f)
		{
			SetFadeAlpha(1f - t);
			yield return null;
		}
		SetFadeAlpha(0f);
	}

	public void FadeOut()
	{
		((MonoBehaviour)this).StartCoroutine(CFadeOut());
	}

	public IEnumerator CFadeOut()
	{
		float t = 0f;
		SetFadeAlpha(0f);
		yield return null;
		for (; t < 1f; t += Time.unscaledDeltaTime * 0.3f)
		{
			SetFadeAlpha(t);
			yield return null;
		}
		SetFadeAlpha(1f);
	}
}
