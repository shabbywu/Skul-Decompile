using UnityEngine;

namespace FX.SpriteEffects;

public class EasedColorOverlay : SpriteEffect
{
	private readonly MaterialPropertyBlock _propertyBlock;

	private readonly Color _startColor = Color.white;

	private readonly Color _endColor = new Color(1f, 1f, 1f, 0f);

	private readonly Curve _curve;

	private float _time;

	public EasedColorOverlay(int priority, Color startColor, Color endColor, Curve curve)
		: base(priority)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003c: Expected O, but got Unknown
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		//IL_0045: Unknown result type (might be due to invalid IL or missing references)
		_propertyBlock = new MaterialPropertyBlock();
		_startColor = startColor;
		_endColor = endColor;
		_curve = curve;
		_time = 0f;
	}

	internal override void Apply(Renderer renderer)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		renderer.GetPropertyBlock(_propertyBlock);
		_propertyBlock.SetColor(SpriteEffect._overlayColor, Color.Lerp(_startColor, _endColor, _curve.Evaluate(_time)));
		renderer.SetPropertyBlock(_propertyBlock);
	}

	internal override bool Update(float deltaTime)
	{
		_time += deltaTime;
		return _time < _curve.duration;
	}

	internal override void Expire()
	{
		_time = _curve.duration;
	}
}
