using FX.SpriteEffects;
using UnityEngine;

namespace Characters.Operations.Fx;

public class ShaderEffect : CharacterOperation
{
	[SerializeField]
	private int _priority;

	[SerializeField]
	private bool _proportionalToTenacity;

	[SerializeField]
	private GenericSpriteEffect.ColorOverlay _colorOverlay;

	[SerializeField]
	private GenericSpriteEffect.ColorBlend _colorBlend;

	[SerializeField]
	private GenericSpriteEffect.Outline _outline;

	[SerializeField]
	private GenericSpriteEffect.GrayScale _grayScale;

	private float _duration;

	private Character _owner;

	private GenericSpriteEffect _effect;

	private void Awake()
	{
		if (_colorOverlay.enabled)
		{
			_duration = _colorOverlay.duration;
		}
		if (_colorBlend.enabled)
		{
			_duration = Mathf.Max(_duration, _colorBlend.duration);
		}
		if (_outline.enabled)
		{
			_duration = Mathf.Max(_duration, _outline.duration);
		}
		if (_grayScale.enabled)
		{
			_duration = Mathf.Max(_duration, _grayScale.duration);
		}
	}

	public override void Run(Character owner)
	{
		_owner = owner;
		if (_owner.spriteEffectStack != null)
		{
			_effect = new GenericSpriteEffect(_priority, _duration, _proportionalToTenacity ? ((float)owner.stat.GetFinal(Stat.Kind.StoppingResistance)) : 1f, _colorOverlay, _colorBlend, _outline, _grayScale);
			_owner.spriteEffectStack.Add(_effect);
		}
	}

	public override void Stop()
	{
		if (_effect != null && _owner.spriteEffectStack != null)
		{
			_owner.spriteEffectStack.Remove(_effect);
		}
	}
}
