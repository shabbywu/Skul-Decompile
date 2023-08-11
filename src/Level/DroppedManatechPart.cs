using System.Collections;
using Characters;
using Characters.Abilities.Weapons.Wizard;
using Characters.Actions;
using FX;
using Services;
using Singletons;
using UnityEngine;

namespace Level;

public class DroppedManatechPart : MonoBehaviour, IPickupable
{
	private class PickupProxy : MonoBehaviour, IPickupable
	{
		public DroppedManatechPart part;

		public void PickedUpBy(Character character)
		{
			part.PickedUpBy(character);
		}
	}

	[SerializeField]
	[MinMaxSlider(0f, 100f)]
	private Vector2 _xPowerRange;

	[MinMaxSlider(0f, 100f)]
	[SerializeField]
	private Vector2 _yPowerRange;

	[SerializeField]
	private Collider2D _pickupTrigger;

	[SerializeField]
	private EffectInfo _effect;

	[SerializeField]
	private SoundInfo _sound;

	[SerializeField]
	[GetComponent]
	private PoolObject _poolObject;

	[SerializeField]
	[GetComponent]
	private Rigidbody2D _rigidbody;

	private Character _player;

	[Header("Custom")]
	[SerializeField]
	private float _wizardPassiveGauge;

	public PoolObject poolObject => _poolObject;

	public float cooldownReducingAmount { get; set; }

	private void Awake()
	{
		((Behaviour)_pickupTrigger).enabled = false;
		((Component)_pickupTrigger).gameObject.AddComponent<PickupProxy>().part = this;
	}

	private void OnEnable()
	{
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		_rigidbody.gravityScale = 3f;
		float num = Random.Range(_xPowerRange.x, _xPowerRange.y);
		if (MMMaths.RandomBool())
		{
			num *= -1f;
		}
		_rigidbody.velocity = new Vector2(num, Random.Range(_yPowerRange.x, _yPowerRange.y));
		_rigidbody.AddTorque((float)(Random.Range(0, 20) * (MMMaths.RandomBool() ? 1 : (-1))));
		_player = Singleton<Service>.Instance.levelManager.player;
		((MonoBehaviour)this).StartCoroutine(CUpdatePickupable());
	}

	private IEnumerator CUpdatePickupable()
	{
		((Behaviour)_pickupTrigger).enabled = false;
		yield return Chronometer.global.WaitForSeconds(0.5f);
		((Behaviour)_pickupTrigger).enabled = true;
	}

	public void PickedUpBy(Character character)
	{
		//IL_0090: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b6: Unknown result type (might be due to invalid IL or missing references)
		foreach (Action action in _player.actions)
		{
			if (action.type == Action.Type.Skill && action.cooldown.time != null)
			{
				action.cooldown.time.ReduceCooldown(cooldownReducingAmount);
			}
		}
		if ((Object)(object)character != (Object)null)
		{
			character.ability.GetInstanceByInstanceType<WizardPassive.Instance>()?.AddGauge(_wizardPassiveGauge);
		}
		_effect.Spawn(((Component)this).transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sound, ((Component)this).transform.position);
		_poolObject.Despawn();
	}
}
