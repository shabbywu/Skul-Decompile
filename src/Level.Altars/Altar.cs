using System;
using System.Collections.Generic;
using Characters;
using FX;
using FX.SpriteEffects;
using UnityEngine;

namespace Level.Altars;

public class Altar : MonoBehaviour
{
	[Serializable]
	private class ShaderEffect
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

		private GenericSpriteEffect _effect;

		public void Initialize()
		{
			_effect = new GenericSpriteEffect(_priority, 2.1474836E+09f, 1f, _colorOverlay, _colorBlend, _outline, _grayScale);
		}

		public void Attach(Character target)
		{
			if (target.spriteEffectStack != null)
			{
				target.spriteEffectStack.Add(_effect);
			}
		}

		public void Detach(Character target)
		{
			if (_effect != null && target.spriteEffectStack != null)
			{
				target.spriteEffectStack.Remove(_effect);
			}
		}
	}

	[SerializeField]
	private Stat.Values _stat;

	[SerializeField]
	[GetComponent]
	private Animator _animator;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private Target _target;

	[SerializeField]
	private EffectInfo _effect;

	[SerializeField]
	private ShaderEffect _shaderEffect;

	[SerializeField]
	private int _offset = 1;

	private Prop _prop;

	public Collider2D collider => _collider;

	public List<Character> characters { get; private set; } = new List<Character>();


	public event Action onDestroyed;

	private void Awake()
	{
		_prop = ((Component)this).GetComponentInParent<Prop>();
		_prop.onDestroy += Destroy;
		_shaderEffect.Initialize();
	}

	private void Destroy()
	{
		((Behaviour)_collider).enabled = false;
		this.onDestroyed?.Invoke();
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		Target component = ((Component)collision).GetComponent<Target>();
		EffectPoolInstance spawnedEffect;
		if (!((Object)(object)component == (Object)null) && !((Object)(object)component.character == (Object)null))
		{
			Character character = component.character;
			if (character.type != Character.Type.Trap && character.type != Character.Type.Dummy)
			{
				spawnedEffect = _effect.Spawn(((Component)component).transform.position, component.character);
				((Component)spawnedEffect).GetComponent<SpriteRenderer>();
				_shaderEffect.Attach(component.character);
				component.character.stat.AttachValues(_stat, DespawnEffect);
				characters.Add(component.character);
			}
		}
		void DespawnEffect(Stat stat)
		{
			if (((Component)spawnedEffect).gameObject.activeSelf)
			{
				spawnedEffect.Stop();
			}
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		Target component = ((Component)collision).GetComponent<Target>();
		if (!((Object)(object)component == (Object)null) && !((Object)(object)component.character == (Object)null))
		{
			CharacterHealth health = component.character.health;
			if (health == null || !health.dead)
			{
				component.character.stat.DetachValues(_stat);
				_shaderEffect.Detach(component.character);
				characters.Remove(component.character);
			}
		}
	}
}
