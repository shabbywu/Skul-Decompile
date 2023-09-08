using FX;
using Services;
using Singletons;
using UnityEngine;
using UnityEngine.Serialization;

namespace Characters;

public class CharacterDieEffect : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	protected Character _character;

	[SerializeField]
	private Transform _effectSpawnPoint;

	[SerializeField]
	private EffectInfo _effect;

	[SerializeField]
	private ParticleEffectInfo _particleInfo;

	[SerializeField]
	private float _vibrationAmount;

	[SerializeField]
	private float _vibrationDuration;

	[SerializeField]
	private SoundInfo _sound;

	[FormerlySerializedAs("_destroyCharacter")]
	[SerializeField]
	private bool _deactivateCharacter = true;

	public EffectInfo effect
	{
		set
		{
			_effect = value;
		}
	}

	public ParticleEffectInfo particleInfo
	{
		set
		{
			_particleInfo = value;
		}
	}

	protected virtual void Awake()
	{
		_character.health.onDiedTryCatch += Spawn;
	}

	protected void Spawn()
	{
		//IL_0026: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		//IL_002b: Unknown result type (might be due to invalid IL or missing references)
		//IL_003a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0062: Unknown result type (might be due to invalid IL or missing references)
		//IL_0067: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		Vector3 position = (((Object)(object)_effectSpawnPoint == (Object)null) ? ((Component)_character).transform.position : _effectSpawnPoint.position);
		if (_effect != null)
		{
			_effect.Spawn(position);
		}
		_particleInfo?.Emit(Vector2.op_Implicit(((Component)_character).transform.position), ((Collider2D)_character.collider).bounds, _character.movement.push);
		if (_vibrationDuration > 0f)
		{
			Singleton<Service>.Instance.controllerVibation.vibration.Attach(this, _vibrationAmount, _vibrationDuration);
		}
		if (_sound != null)
		{
			PersistentSingleton<SoundManager>.Instance.PlaySound(_sound, ((Component)this).transform.position);
		}
		if (_deactivateCharacter)
		{
			((Component)this).gameObject.SetActive(false);
			((Behaviour)_character.collider).enabled = false;
		}
		Object.Destroy((Object)(object)this);
	}

	public void Detach()
	{
		_character.health.onDiedTryCatch -= Spawn;
	}

	private void OnDestroy()
	{
		_character.health.onDiedTryCatch -= Spawn;
	}
}
