using UnityEngine;

namespace FX.SpriteEffects;

public sealed class GuardHit : SpriteEffect
{
	private readonly float _duration;

	private MaterialPropertyBlock _propertyBlock;

	private float _time;

	private Color _startColor = new Color(1f, 1f, 1f, 1f);

	private Color _endColor = new Color(1f, 1f, 1f, 0f);

	public GuardHit(int priority = 0, float duration = 0.2f)
		: base(priority)
	{
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Expected O, but got Unknown
		_propertyBlock = new MaterialPropertyBlock();
		_duration = duration;
		_time = 0f;
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
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0030: Unknown result type (might be due to invalid IL or missing references)
		renderer.GetPropertyBlock(_propertyBlock);
		_propertyBlock.SetColor(SpriteEffect._overlayColor, Color.LerpUnclamped(_startColor, _endColor, _time / _duration));
		renderer.SetPropertyBlock(_propertyBlock);
	}

	internal override bool Update(float deltaTime)
	{
		_time += deltaTime;
		return _time < _duration;
	}

	internal override void Expire()
	{
		_time = _duration;
	}
}
