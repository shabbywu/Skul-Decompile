using System.Collections;
using Characters;
using Characters.Abilities.Customs;
using Characters.Abilities.Weapons.Ghoul;
using FX;
using Singletons;
using UnityEngine;

namespace Level;

public class DroppedGhoulFlesh : MonoBehaviour
{
	private class PickupProxy : MonoBehaviour, IPickupable
	{
		public DroppedGhoulFlesh ghoulFlesh;

		public void PickedUpBy(Character character)
		{
			ghoulFlesh.PickedUpBy(character);
		}
	}

	[SerializeField]
	private Collider2D _pickupTrigger;

	[SerializeField]
	private EffectInfo _effect;

	[SerializeField]
	private SoundInfo _sound;

	[GetComponent]
	[SerializeField]
	private PoolObject _poolObject;

	[SerializeField]
	[GetComponent]
	private Rigidbody2D _rigidbody;

	private GhoulPassive _ghoulPassive;

	private GhoulPassive2 _ghoulHealthPassive;

	private void Awake()
	{
		((Behaviour)_pickupTrigger).enabled = false;
		((Component)_pickupTrigger).gameObject.AddComponent<PickupProxy>().ghoulFlesh = this;
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

	public void Spawn(Vector3 postion, GhoulPassive ghoulPassive)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Component)_poolObject.Spawn(postion, true)).GetComponent<DroppedGhoulFlesh>()._ghoulPassive = ghoulPassive;
	}

	public void Spawn(Vector3 postion, GhoulPassive2 ghoulHealthPassive)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		((Component)_poolObject.Spawn(postion, true)).GetComponent<DroppedGhoulFlesh>()._ghoulHealthPassive = ghoulHealthPassive;
	}

	public void PickedUpBy(Character character)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		_ghoulHealthPassive.AddStack();
		_effect.Spawn(((Component)this).transform.position);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sound, ((Component)this).transform.position);
		_poolObject.Despawn();
	}
}
