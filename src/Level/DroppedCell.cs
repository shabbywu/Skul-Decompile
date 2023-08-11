using System.Collections;
using Characters;
using Characters.Gear.Weapons.Gauges;
using FX;
using Singletons;
using UnityEngine;

namespace Level;

public class DroppedCell : MonoBehaviour
{
	private class PickupProxy : MonoBehaviour, IPickupable
	{
		public DroppedCell cell;

		public void PickedUpBy(Character character)
		{
			cell.PickedUpBy(character);
		}
	}

	private static short _sortingOrder = short.MinValue;

	[SerializeField]
	private Collider2D _pickupTrigger;

	[Space]
	[SerializeField]
	private EffectInfo _effect;

	[SerializeField]
	private SoundInfo _sound;

	[GetComponent]
	[Space]
	[SerializeField]
	private PoolObject _poolObject;

	[SerializeField]
	[GetComponent]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	[GetComponent]
	private Rigidbody2D _rigidbody;

	private ValueGauge _prisonerGauge;

	private void Awake()
	{
		((Behaviour)_pickupTrigger).enabled = false;
		((Component)_pickupTrigger).gameObject.AddComponent<PickupProxy>().cell = this;
		((Renderer)_spriteRenderer).sortingOrder = _sortingOrder++;
	}

	private void OnEnable()
	{
		//IL_0040: Unknown result type (might be due to invalid IL or missing references)
		((Behaviour)_pickupTrigger).enabled = false;
		_rigidbody.gravityScale = 3f;
		_rigidbody.velocity = new Vector2(Random.Range(-3f, 3f), Random.Range(5f, 15f));
		_rigidbody.AddTorque((float)(Random.Range(0, 15) * (MMMaths.RandomBool() ? 1 : (-1))));
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
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		_prisonerGauge.Add(1f);
		_effect.Spawn(((Component)this).transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sound, ((Component)this).transform.position);
		_poolObject.Despawn();
	}

	public void Spawn(Vector3 postion, ValueGauge gauge)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Component)_poolObject.Spawn(postion, true)).GetComponent<DroppedCell>()._prisonerGauge = gauge;
	}
}
