using System;
using System.Collections;
using Data;
using FX;
using Singletons;
using UnityEngine;

namespace Level;

[RequireComponent(typeof(PoolObject), typeof(Rigidbody2D))]
public class CurrencyParticle : MonoBehaviour
{
	private static readonly Vector2 _minVelocity = new Vector2(-4f, 7f);

	private static readonly Vector2 _maxVelocity = new Vector2(4f, 17f);

	private const float _minTorque = -10f;

	private const float _maxTorque = 10f;

	[SerializeField]
	[Header("Required")]
	[GetComponent]
	private PoolObject _poolObject;

	[SerializeField]
	[GetComponent]
	private Collider2D _collider;

	[SerializeField]
	[GetComponent]
	private Rigidbody2D _rigidbody;

	[Header("FX")]
	[SerializeField]
	private EffectInfo _effect;

	[SerializeField]
	private SoundInfo _sound;

	[NonSerialized]
	public GameData.Currency.Type currencyType;

	[NonSerialized]
	public int currencyAmount;

	private void OnEnable()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		_collider.isTrigger = false;
		_rigidbody.gravityScale = 3f;
		_rigidbody.velocity = MMMaths.RandomVector2(_minVelocity, _maxVelocity);
		_rigidbody.AddTorque(Random.Range(-10f, 10f));
		((MonoBehaviour)this).StartCoroutine(CUpdate());
	}

	private void OnDisable()
	{
		GameData.Currency.currencies[currencyType].Earn(currencyAmount);
	}

	public void SetForce(Vector2 force)
	{
		//IL_0000: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0027: Unknown result type (might be due to invalid IL or missing references)
		//IL_0037: Unknown result type (might be due to invalid IL or missing references)
		//IL_003d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0042: Unknown result type (might be due to invalid IL or missing references)
		//IL_0053: Unknown result type (might be due to invalid IL or missing references)
		//IL_0058: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0077: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_0099: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b9: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c9: Unknown result type (might be due to invalid IL or missing references)
		if (!(Mathf.Abs(force.x) < 1f) && !(Mathf.Abs(force.y) < 1f))
		{
			force.y = Mathf.Abs(force.y);
			force *= 4f;
			force = Vector2.op_Implicit(Quaternion.AngleAxis(Random.Range(-15f, 15f), Vector3.forward) * Vector2.op_Implicit(force) * Random.Range(0.8f, 1.2f));
			force.y += 10f;
			_rigidbody.velocity = Vector2.zero;
			_rigidbody.angularVelocity = 0f;
			_rigidbody.AddForce(force * Random.Range(0.5f, 1f), (ForceMode2D)1);
			_rigidbody.AddTorque(Random.Range(-0.5f, 0.5f), (ForceMode2D)1);
		}
	}

	private IEnumerator CUpdate()
	{
		yield return Chronometer.global.WaitForSeconds(Random.Range(0.9f, 1.1f));
		PersistentSingleton<SoundManager>.Instance.PlaySound(_sound, ((Component)this).transform.position);
		_effect.Spawn(((Component)this).transform.position);
		_poolObject.Despawn();
	}
}
