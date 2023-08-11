using UnityEngine;

namespace FX.SpriteEffects;

public class AlphaThreshold : SpriteEffect
{
	private readonly MaterialPropertyBlock _propertyBlock;

	private readonly float _value;

	private readonly float _duration;

	private float _time;

	public AlphaThreshold(int priority, float value, float duration)
		: base(priority)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		_propertyBlock = new MaterialPropertyBlock();
		_value = value;
		if (duration == 0f)
		{
			duration = float.PositiveInfinity;
		}
		_duration = duration;
		_time = 0f;
	}

	internal override void Apply(Renderer renderer)
	{
		renderer.GetPropertyBlock(_propertyBlock);
		_propertyBlock.SetFloat(SpriteEffect._alphaThreshold, _value);
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
