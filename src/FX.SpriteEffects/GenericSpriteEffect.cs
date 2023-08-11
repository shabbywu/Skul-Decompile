using System;
using UnityEngine;

namespace FX.SpriteEffects;

public sealed class GenericSpriteEffect : SpriteEffect
{
	[Serializable]
	public class ColorOverlay
	{
		[SerializeField]
		private bool _enabled;

		[SerializeField]
		private Color _startColor = Color.white;

		[SerializeField]
		private Color _endColor = new Color(1f, 1f, 1f, 0f);

		[SerializeField]
		private Curve _curve;

		public bool enabled => _enabled;

		public Color startColor => _startColor;

		public Color endColor => _endColor;

		public Curve curve => _curve;

		public float duration => _curve.duration;

		internal void Apply(Renderer renderer, MaterialPropertyBlock propertyBlock, float time)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			if (_enabled)
			{
				propertyBlock.SetColor(SpriteEffect._overlayColor, Color.LerpUnclamped(_startColor, _endColor, _curve.Evaluate(time)));
			}
		}
	}

	[Serializable]
	public class ColorBlend
	{
		[SerializeField]
		private bool _enabled;

		[SerializeField]
		private Color _startColor = Color.white;

		[SerializeField]
		private Color _endColor = new Color(1f, 1f, 1f, 0f);

		[SerializeField]
		private Curve _curve;

		public bool enabled => _enabled;

		public Color startColor => _startColor;

		public Color endColor => _endColor;

		public Curve curve => _curve;

		public float duration => _curve.duration;

		internal void Apply(Renderer renderer, MaterialPropertyBlock propertyBlock, float time)
		{
			//IL_0010: Unknown result type (might be due to invalid IL or missing references)
			//IL_0016: Unknown result type (might be due to invalid IL or missing references)
			//IL_0027: Unknown result type (might be due to invalid IL or missing references)
			if (_enabled)
			{
				propertyBlock.SetColor(SpriteEffect._baseColor, Color.LerpUnclamped(_startColor, _endColor, _curve.Evaluate(time)));
			}
		}
	}

	[Serializable]
	public class Outline
	{
		[SerializeField]
		private bool _enabled;

		[SerializeField]
		private Color _color = Color.white;

		[SerializeField]
		private bool _colorChange;

		[SerializeField]
		private Color _endColor;

		[SerializeField]
		[Range(1f, 10f)]
		private float _brightness = 2f;

		[Range(1f, 6f)]
		[SerializeField]
		private int _width = 1;

		[FrameTime]
		[SerializeField]
		private float _duration;

		public bool enabled => _enabled;

		public Color color => _color;

		public float brightness => _brightness;

		public int width => _width;

		public float duration => _duration;

		internal void Apply(Renderer renderer, MaterialPropertyBlock propertyBlock, float time)
		{
			//IL_001a: Unknown result type (might be due to invalid IL or missing references)
			//IL_0025: Unknown result type (might be due to invalid IL or missing references)
			//IL_002a: Unknown result type (might be due to invalid IL or missing references)
			//IL_006c: Unknown result type (might be due to invalid IL or missing references)
			//IL_004b: Unknown result type (might be due to invalid IL or missing references)
			//IL_004d: Unknown result type (might be due to invalid IL or missing references)
			//IL_005a: Unknown result type (might be due to invalid IL or missing references)
			if (_enabled)
			{
				propertyBlock.SetFloat(SpriteEffect._outlineEnabled, 1f);
				Color val = _color * _brightness;
				val.a = _color.a;
				if (_colorChange)
				{
					propertyBlock.SetColor(SpriteEffect._outlineColor, Color.LerpUnclamped(val, _endColor, time / duration));
				}
				else
				{
					propertyBlock.SetColor(SpriteEffect._outlineColor, val);
				}
				propertyBlock.SetFloat(SpriteEffect._outlineSize, (float)_width);
				propertyBlock.SetFloat(SpriteEffect._alphaThreshold, 0.01f);
			}
		}
	}

	[Serializable]
	public class GrayScale
	{
		[SerializeField]
		private bool _enabled;

		[SerializeField]
		[Range(0f, 1f)]
		private float _startAmount;

		[SerializeField]
		[Range(0f, 1f)]
		private float _endAmount;

		[SerializeField]
		private Curve _curve;

		public bool enabled => _enabled;

		public float startAmount => _startAmount;

		public float endAmount => _endAmount;

		public Curve curve => _curve;

		public float duration => _curve.duration;

		internal void Apply(Renderer renderer, MaterialPropertyBlock propertyBlock, float time)
		{
			if (_enabled)
			{
				propertyBlock.SetFloat(SpriteEffect._grayScaleLerp, Mathf.Lerp(_startAmount, _endAmount, _curve.Evaluate(time)));
			}
		}
	}

	private MaterialPropertyBlock _propertyBlock;

	private readonly ColorOverlay _colorOverlay;

	private readonly ColorBlend _colorBlend;

	private readonly Outline _outline;

	private readonly GrayScale _grayScale;

	private readonly float _duration;

	private readonly float _speed;

	private float _time;

	public GenericSpriteEffect(int priority, float duration, float speed, ColorOverlay colorOverlay, ColorBlend colorBlend, Outline outline, GrayScale grayScale)
		: base(priority)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		_propertyBlock = new MaterialPropertyBlock();
		_duration = duration;
		_speed = speed;
		_colorOverlay = colorOverlay;
		_colorBlend = colorBlend;
		_outline = outline;
		_grayScale = grayScale;
	}

	internal void Reset()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		_propertyBlock = new MaterialPropertyBlock();
		_time = 0f;
	}

	internal override void Apply(Renderer renderer)
	{
		renderer.GetPropertyBlock(_propertyBlock);
		_colorOverlay.Apply(renderer, _propertyBlock, _time);
		_colorBlend.Apply(renderer, _propertyBlock, _time);
		_outline.Apply(renderer, _propertyBlock, _time);
		_grayScale.Apply(renderer, _propertyBlock, _time);
		renderer.SetPropertyBlock(_propertyBlock);
	}

	internal override bool Update(float deltaTime)
	{
		_time += deltaTime * _speed;
		return _time < _duration;
	}

	internal override void Expire()
	{
		_time = _duration;
	}
}
