using UnityEngine;

namespace FX.SpriteEffects;

public class SpriteShaderEffectApplier : MonoBehaviour
{
	[SerializeField]
	private SpriteEffectStack _spriteEffectStack;

	[SerializeField]
	private bool _playOnStart;

	[SerializeField]
	private int _priority;

	[SerializeField]
	private GenericSpriteEffect.ColorOverlay _colorOverlay;

	[SerializeField]
	private GenericSpriteEffect.ColorBlend _colorBlend;

	[SerializeField]
	private GenericSpriteEffect.Outline _outline;

	[SerializeField]
	private GenericSpriteEffect.GrayScale _grayScale;

	private float _duration;

	private GenericSpriteEffect _effect;

	private void Awake()
	{
		_duration = Mathf.Max(new float[3] { _colorOverlay.duration, _colorBlend.duration, _outline.duration });
	}

	private void Start()
	{
		if (_playOnStart)
		{
			ApplyEffect();
		}
	}

	public void ApplyEffect()
	{
		if (!((Object)(object)_spriteEffectStack == (Object)null))
		{
			_effect = new GenericSpriteEffect(_priority, _duration, 1f, _colorOverlay, _colorBlend, _outline, _grayScale);
			_spriteEffectStack.Add(_effect);
		}
	}
}
