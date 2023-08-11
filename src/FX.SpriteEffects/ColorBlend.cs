using UnityEngine;

namespace FX.SpriteEffects;

public class ColorBlend : SpriteEffect
{
	private readonly MaterialPropertyBlock _propertyBlock;

	private readonly Color _color;

	private readonly float _duration;

	private float _time;

	public ColorBlend(int priority, Color color, float duration)
		: base(priority)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Expected O, but got Unknown
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		_propertyBlock = new MaterialPropertyBlock();
		_color = color;
		if (duration == 0f)
		{
			duration = float.PositiveInfinity;
		}
		_duration = duration;
		_time = 0f;
	}

	internal override void Apply(Renderer renderer)
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		renderer.GetPropertyBlock(_propertyBlock);
		_propertyBlock.SetColor(SpriteEffect._baseColor, _color);
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
