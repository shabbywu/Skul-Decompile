using System;
using System.Collections;
using Characters;
using Characters.Abilities.Customs;
using FX;
using Singletons;
using UnityEngine;

namespace Level;

public class DroppedMummyGun : MonoBehaviour
{
	private class PickupProxy : MonoBehaviour, IPickupable
	{
		public DroppedMummyGun droppedMummyGun;

		public void PickedUpBy(Character character)
		{
			droppedMummyGun.PickedUpBy(character);
		}
	}

	[SerializeField]
	private string _key;

	[Space]
	[SerializeField]
	private Collider2D _pickupTrigger;

	[SerializeField]
	private float _pickupDelay;

	[SerializeField]
	private float _startYVelocity;

	[SerializeField]
	private EffectInfo _effect;

	[SerializeField]
	private SoundInfo _sound;

	[Space]
	[SerializeField]
	[GetComponent]
	private PoolObject _poolObject;

	[SerializeField]
	[GetComponent]
	private Rigidbody2D _rigidbody;

	private MummyPassive _mummyPassive;

	public Rigidbody2D rigidbody => _rigidbody;

	public event Action onPickedUp;

	private void Awake()
	{
		((Behaviour)_pickupTrigger).enabled = false;
		if ((Object)(object)((Component)_pickupTrigger).GetComponent<PickupProxy>() == (Object)null)
		{
			((Component)_pickupTrigger).gameObject.AddComponent<PickupProxy>().droppedMummyGun = this;
		}
	}

	private void OnEnable()
	{
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)_pickupTrigger).enabled = false;
		_rigidbody.gravityScale = 3f;
		_rigidbody.velocity = new Vector2(0f, _startYVelocity);
		((MonoBehaviour)this).StartCoroutine(CUpdatePickupable());
	}

	private IEnumerator CUpdatePickupable()
	{
		((Behaviour)_pickupTrigger).enabled = false;
		yield return Chronometer.global.WaitForSeconds(_pickupDelay);
		((Behaviour)_pickupTrigger).enabled = true;
	}

	public DroppedMummyGun Spawn(Vector3 position, MummyPassive mummyPassive)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		DroppedMummyGun component = ((Component)_poolObject.Spawn(position, true)).GetComponent<DroppedMummyGun>();
		component._mummyPassive = mummyPassive;
		return component;
	}

	public void PickedUpBy(Character character)
	{
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		if (_mummyPassive != null)
		{
			_mummyPassive.PickUpWeapon(_key);
		}
		_effect.Spawn(((Component)this).transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sound, ((Component)this).transform.position);
		_poolObject.Despawn();
		this.onPickedUp?.Invoke();
	}

	public void Despawn()
	{
		_poolObject.Despawn();
	}
}
