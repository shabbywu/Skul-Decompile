using System;
using System.Collections;
using System.Linq;
using Characters;
using FX;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Level;

public class Prop : DestructibleObject
{
	[Serializable]
	public class DestructionPhaseInfo : ReorderableArray<DestructionPhaseInfo.PhaseSprite>
	{
		[Serializable]
		public class PhaseSprite
		{
			internal double health;

			[SerializeField]
			private float _weight = 1f;

			[SerializeField]
			private Sprite _sprite;

			[SerializeField]
			private RuntimeAnimatorController _animation;

			[SerializeField]
			private ParticleEffectInfo _particle;

			[SerializeField]
			private GameObject _toDeactivate;

			[SerializeField]
			[FormerlySerializedAs("_particleSpawnPoint")]
			public Transform particleSpawnPoint;

			public float weight => _weight;

			public Sprite sprite => _sprite;

			public RuntimeAnimatorController animation => _animation;

			public ParticleEffectInfo particle => _particle;

			public GameObject toDeactivate => _toDeactivate;
		}

		private Prop _prop;

		private float _totalWeight;

		public int current { get; protected set; }

		public void Initialize(Prop prop)
		{
			_prop = prop;
			_totalWeight = values.Sum((PhaseSprite v) => v.weight);
			current = 0;
			PhaseSprite[] array = values;
			foreach (PhaseSprite phaseSprite in array)
			{
				phaseSprite.health = phaseSprite.weight / _totalWeight * _prop._health;
				if ((Object)(object)phaseSprite.particleSpawnPoint == (Object)null)
				{
					phaseSprite.particleSpawnPoint = ((Component)prop).transform;
				}
			}
		}

		public PhaseSprite TakeDamage(Vector3 position, Character owner, double damage, Vector2 force)
		{
			//IL_007f: Unknown result type (might be due to invalid IL or missing references)
			//IL_0084: Unknown result type (might be due to invalid IL or missing references)
			//IL_0094: Unknown result type (might be due to invalid IL or missing references)
			//IL_0099: Unknown result type (might be due to invalid IL or missing references)
			while (damage > 0.0)
			{
				if (values.Length <= current)
				{
					return null;
				}
				PhaseSprite phaseSprite = values[current];
				if (phaseSprite.health > damage)
				{
					phaseSprite.health -= damage;
					break;
				}
				damage -= phaseSprite.health;
				if ((Object)(object)phaseSprite.toDeactivate != (Object)null)
				{
					phaseSprite.toDeactivate.SetActive(false);
				}
				if ((Object)(object)phaseSprite.particle != (Object)null)
				{
					phaseSprite.particle.Emit(Vector2.op_Implicit(phaseSprite.particleSpawnPoint.position), _prop.collider.bounds, force);
				}
				current++;
				if (values.Length <= current)
				{
					return null;
				}
			}
			return values[current];
		}
	}

	public delegate void DidHitDelegate(Character owner, in Damage damage, Vector2 force);

	[SerializeField]
	private Key _key = Key.SmallProp;

	[SerializeField]
	private bool _countHealth;

	[SerializeField]
	private float _health;

	private Collider2D _collider;

	[SerializeField]
	private Color _startColor;

	[SerializeField]
	private Color _endColor;

	[SerializeField]
	private Curve _hitColorCurve;

	private CoroutineReference _cEaseColorReference;

	[SerializeField]
	[GetComponent]
	private SpriteRenderer _spriteRenderer;

	[GetComponent]
	[SerializeField]
	private Animator _animator;

	[SerializeField]
	private DestructionPhaseInfo _destructionPhase;

	[SerializeField]
	private Sprite _wreckage;

	[SerializeField]
	private SoundInfo _hitSound;

	[SerializeField]
	private SoundInfo _destroySound;

	private Target[] _targets;

	public Key key => _key;

	public int phase => _destructionPhase.current;

	public override Collider2D collider => _collider;

	public event DidHitDelegate onDidHit;

	private void Awake()
	{
		if (!_countHealth)
		{
			_health *= Singleton<Service>.Instance.levelManager.currentChapter.currentStage.healthMultiplier;
		}
		Initialize();
	}

	public void Initialize()
	{
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).gameObject.SetActive(true);
		base.destroyed = false;
		_targets = ((Component)this).GetComponentsInChildren<Target>(true);
		_collider = _targets[0].collider;
		for (int i = 0; i < _targets.Length; i++)
		{
			Target obj = _targets[i];
			((Behaviour)obj.collider).enabled = true;
			((Behaviour)obj).enabled = true;
		}
		_destructionPhase.Initialize(this);
		_spriteRenderer.sprite = _destructionPhase.values[0].sprite;
		if ((Object)(object)_animator != (Object)null)
		{
			((Behaviour)_animator).enabled = true;
			_animator.Play(0, 0, 0f);
		}
		_spriteRenderer.color = _endColor;
	}

	public override void Hit(Character owner, ref Damage damage, Vector2 force)
	{
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_0140: Unknown result type (might be due to invalid IL or missing references)
		//IL_0087: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d2: Unknown result type (might be due to invalid IL or missing references)
		damage.Evaluate(immuneToCritical: false);
		DestructionPhaseInfo.PhaseSprite phaseSprite = _destructionPhase.TakeDamage(((Component)this).transform.position, owner, _countHealth ? 1.0 : damage.amount, force);
		this.onDidHit?.Invoke(owner, in damage, force);
		if (phaseSprite == null)
		{
			if (!base.destroyed)
			{
				base.destroyed = true;
				_onDestroy?.Invoke();
				PersistentSingleton<SoundManager>.Instance.PlaySound(_destroySound, ((Component)this).transform.position);
				switch (key)
				{
				case Key.SmallProp:
					Settings.instance.smallPropGoldPossibility.Drop(((Component)this).transform.position);
					break;
				case Key.LargeProp:
					Settings.instance.largePropGoldPossibility.Drop(((Component)this).transform.position);
					break;
				}
				if ((Object)(object)_wreckage != (Object)null)
				{
					ChangeToWreck();
				}
				else
				{
					((Component)this).gameObject.SetActive(false);
				}
			}
		}
		else
		{
			if ((Object)(object)_animator != (Object)null)
			{
				_animator.runtimeAnimatorController = phaseSprite.animation;
			}
			_spriteRenderer.sprite = phaseSprite.sprite;
			PersistentSingleton<SoundManager>.Instance.PlaySound(_hitSound, ((Component)this).transform.position);
			_cEaseColorReference.Stop();
			_cEaseColorReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CEaseColor());
		}
	}

	private void ChangeToWreck()
	{
		for (int i = 0; i < _targets.Length; i++)
		{
			Target obj = _targets[i];
			((Behaviour)obj.collider).enabled = false;
			((Behaviour)obj).enabled = false;
		}
		_spriteRenderer.sprite = _wreckage;
		if ((Object)(object)_animator != (Object)null)
		{
			((Behaviour)_animator).enabled = true;
			_animator.Play(0, 0, 0f);
			((Behaviour)_animator).enabled = false;
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
		if ((Object)(object)_wreckage == (Object)null && base.destroyed)
		{
			Object.Destroy((Object)(object)this);
		}
	}
}
