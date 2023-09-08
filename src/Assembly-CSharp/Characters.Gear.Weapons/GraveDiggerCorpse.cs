using System.Collections;
using Characters.Abilities.Customs;
using Characters.Monsters;
using FX;
using Level;
using Singletons;
using UnityEngine;

namespace Characters.Gear.Weapons;

public class GraveDiggerCorpse : DestructibleObject
{
	[SerializeField]
	private Monster _minion;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private SpriteRenderer _spriteRenderer;

	[Space]
	[SerializeField]
	private int _lifetime;

	[SerializeField]
	[Header("체력 (타격횟수)")]
	private int _health;

	[SerializeField]
	[Header("Hit Effect")]
	private SoundInfo _hitSound;

	[SerializeField]
	[Space]
	private Color _startColor;

	[SerializeField]
	private Color _endColor;

	[SerializeField]
	private Curve _hitColorCurve;

	private CoroutineReference _cEaseColorReference;

	[SerializeField]
	private CharacterSynchronization _syncWithOwner;

	private float _remainLifetime;

	private int _remainHealth;

	private GraveDiggerPassive _graveDiggerPassive;

	public Monster minion => _minion;

	public override Collider2D collider => _collider;

	public void SetPassive(GraveDiggerPassive graveDiggerPassive, Character owner)
	{
		_graveDiggerPassive = graveDiggerPassive;
		_syncWithOwner.Synchronize(_minion.character, owner);
	}

	private void OnEnable()
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		_remainHealth = _health;
		_spriteRenderer.color = _endColor;
		_remainLifetime = _lifetime;
	}

	private void Update()
	{
		_remainLifetime -= Chronometer.global.deltaTime;
		if (_remainLifetime <= 0f)
		{
			_minion.Despawn();
		}
	}

	public override void Hit(Character from, ref Damage damage, Vector2 force)
	{
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		_remainHealth--;
		if (_hitSound != null)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_hitSound, ((Component)this).transform.position);
		}
		if (_remainHealth <= 0)
		{
			_graveDiggerPassive.HandleCorpseDie(((Component)this).transform.position);
			_minion.character.health.Kill();
		}
		else
		{
			_cEaseColorReference.Stop();
			_cEaseColorReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CEaseColor());
		}
	}

	private IEnumerator CEaseColor()
	{
		float duration = _hitColorCurve.duration;
		for (float time = 0f; time < duration; time += Chronometer.global.deltaTime)
		{
			_spriteRenderer.color = Color.Lerp(_startColor, _endColor, _hitColorCurve.Evaluate(time));
			yield return null;
		}
		_spriteRenderer.color = _endColor;
	}
}
