using UnityEngine;

namespace FX.SpriteEffects;

public class Invulnerable : SpriteEffect
{
	private readonly MaterialPropertyBlock _propertyBlock;

	private readonly float _flickInterval;

	private readonly float _duration;

	private float _flickerTime;

	private float _time;

	private bool _transparent;

	public Invulnerable(int priority, float flickInterval, float duration = float.PositiveInfinity)
		: base(priority)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		_propertyBlock = new MaterialPropertyBlock();
		_flickInterval = flickInterval;
		_duration = duration;
	}

	internal override void Apply(Renderer renderer)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		renderer.GetPropertyBlock(_propertyBlock);
		Color color = _propertyBlock.GetColor(SpriteEffect._baseColor);
		color.a = (_transparent ? 0.3f : 0.7f);
		_propertyBlock.SetColor(SpriteEffect._baseColor, color);
		renderer.SetPropertyBlock(_propertyBlock);
	}

	internal override bool Update(float deltaTime)
	{
		_time += deltaTime;
		_flickerTime -= deltaTime;
		if (_flickerTime <= 0f)
		{
			_flickerTime = _flickInterval;
			_transparent = !_transparent;
		}
		return _time < _duration;
	}

	internal override void Expire()
	{
		_time = _duration;
	}
}
