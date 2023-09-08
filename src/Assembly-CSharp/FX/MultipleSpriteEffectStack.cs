using FX.SpriteEffects;
using UnityEngine;

namespace FX;

public class MultipleSpriteEffectStack : MonoBehaviour, ISpriteEffectStack
{
	private readonly PriorityList<SpriteEffect> _effects = new PriorityList<SpriteEffect>();

	[SerializeField]
	private SpriteRenderer _mainRenderer;

	[SerializeField]
	private SpriteRenderer[] _renderers;

	private MaterialPropertyBlock props;

	public SpriteRenderer mainRenderer => _mainRenderer;

	private void Awake()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Expected O, but got Unknown
		props = new MaterialPropertyBlock();
	}

	protected virtual void LateUpdate()
	{
		for (int num = _effects.Count - 1; num >= 0; num--)
		{
			if (!_effects[num].Update(Chronometer.global.deltaTime))
			{
				_effects.RemoveAt(num);
			}
		}
		for (int i = 0; i < _renderers.Length; i++)
		{
			SpriteEffect.@default.Apply((Renderer)(object)_renderers[i]);
			for (int j = 0; j < _effects.Count; j++)
			{
				_effects[j].Apply((Renderer)(object)_renderers[i]);
			}
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
