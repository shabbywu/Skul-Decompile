using Characters.Actions;
using FX;
using UnityEngine;

namespace Characters.Operations;

public class AbyssLaser : CharacterOperation, IAttackDamage
{
	[SerializeField]
	private ChargeAction _chargeAction;

	[SerializeField]
	[Header("Effect")]
	private float _yScaleMin;

	[SerializeField]
	private float _yScaleMax;

	[Space]
	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private bool _attachToSpawnPosition;

	[SerializeField]
	private EffectInfo _info;

	[SerializeField]
	[Header("Attack")]
	private AttackDamage _attackDamage;

	private float _damageMultiplier;

	[SerializeField]
	[Space]
	private float _damageMultiplierMin;

	[SerializeField]
	private float _damageMultiplierMax;

	public float amount => _damageMultiplier * _attackDamage.amount;

	private void Awake()
	{
		if ((Object)(object)_spawnPosition == (Object)null)
		{
			_spawnPosition = ((Component)this).transform;
		}
	}

	public override void Run(Character owner)
	{
		//IL_0032: Unknown result type (might be due to invalid IL or missing references)
		//IL_003e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0043: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a6: Unknown result type (might be due to invalid IL or missing references)
		_damageMultiplier = (_damageMultiplierMax - _damageMultiplierMin) * _chargeAction.chargedPercent + _damageMultiplierMin;
		EffectInfo info = _info;
		Vector3 position = _spawnPosition.position;
		Quaternion rotation = _spawnPosition.rotation;
		EffectPoolInstance effectPoolInstance = info.Spawn(position, owner, ((Quaternion)(ref rotation)).eulerAngles.z);
		if (_attachToSpawnPosition)
		{
			((Component)effectPoolInstance).transform.parent = _spawnPosition;
		}
		float num = (_yScaleMax - _yScaleMin) * _chargeAction.chargedPercent + _yScaleMin;
		((Component)effectPoolInstance).transform.localScale = new Vector3(1f, num, 1f);
	}

	public override void Stop()
	{
		_info.DespawnChildren();
	}
}
