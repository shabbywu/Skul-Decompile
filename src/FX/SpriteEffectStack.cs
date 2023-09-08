using FX.SpriteEffects;
using UnityEngine;

namespace FX;

public class SpriteEffectStack : MonoBehaviour, ISpriteEffectStack
{
	private readonly PriorityList<SpriteEffect> _effects = new PriorityList<SpriteEffect>();

	protected Chronometer _chronometer;

	[SerializeField]
	protected SpriteRenderer _spriteRenderer;

	private MaterialPropertyBlock props;

	public SpriteRenderer mainRenderer
	{
		get
		{
			return _spriteRenderer;
		}
		set
		{
			_spriteRenderer = value;
		}
	}

	protected virtual void Awake()
	{
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Expected O, but got Unknown
		if ((Object)(object)_spriteRenderer == (Object)null)
		{
			_spriteRenderer = ((Component)this).GetComponent<SpriteRenderer>();
		}
		props = new MaterialPropertyBlock();
	}

	protected virtual void LateUpdate()
	{
		for (int num = _effects.Count - 1; num >= 0; num--)
		{
			if (!_effects[num].Update(_chronometer.DeltaTime()))
			{
				_effects.RemoveAt(num);
			}
		}
		SpriteEffect.@default.Apply((Renderer)(object)_spriteRenderer);
		for (int i = 0; i < _effects.Count; i++)
		{
			_effects[i].Apply((Renderer)(object)_spriteRenderer);
		}
	}

	public void Add(SpriteEffect effect)
	{
		_effects.Add(effect.priority, effect);
	}

	public bool Contains(SpriteEffect effect)
	{
		return _effects.Contains(effect);
	}

	public bool Remove(SpriteEffect effect)
	{
		return _effects.Remove(effect);
	}
}
