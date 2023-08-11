using Characters.Operations;
using UnityEditor;
using UnityEngine;
using Utils;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Strike : SimpleStatBonusKeyword
{
	[Header("2세트 효과")]
	[SerializeField]
	private double[] _statBonusByStep = new double[3] { 0.0, 0.20000000298023224, 0.20000000298023224 };

	[Header("4세트 효과 (Percent)")]
	[SerializeField]
	[Range(0f, 1f)]
	private float _maxStatBonusChance = 0.5f;

	[SerializeField]
	private float _criticalDamageMultiplier = 0.2f;

	[SerializeField]
	private PositionInfo _positionInfo;

	[SerializeField]
	private Transform _targetPoint;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onTargetHit;

	protected override double[] statBonusByStep => _statBonusByStep;

	protected override Stat.Category statCategory => Stat.Category.PercentPoint;

	protected override Stat.Kind statKind => Stat.Kind.CriticalDamage;

	protected override void Initialize()
	{
		base.Initialize();
		_onTargetHit.Initialize();
	}

	public override void Attach()
	{
		base.Attach();
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Add(int.MinValue, (GiveDamageDelegate)OnGiveDamage);
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
		UpdateStat();
	}

	private bool OnGiveDamage(ITarget target, ref Damage damage)
	{
		//IL_0055: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005d: Unknown result type (might be due to invalid IL or missing references)
		//IL_006d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0072: Unknown result type (might be due to invalid IL or missing references)
		//IL_0075: Unknown result type (might be due to invalid IL or missing references)
		//IL_007a: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00be: Unknown result type (might be due to invalid IL or missing references)
		if (keyword.step < _statBonusByStep.Length - 1)
		{
			return false;
		}
		if (!MMMaths.Chance(_maxStatBonusChance))
		{
			return false;
		}
		if ((Object)(object)_targetPoint != (Object)null)
		{
			Bounds bounds = target.collider.bounds;
			Vector3 center = ((Bounds)(ref bounds)).center;
			bounds = target.collider.bounds;
			Vector3 size = ((Bounds)(ref bounds)).size;
			size.x *= _positionInfo.pivotValue.x;
			size.y *= _positionInfo.pivotValue.y;
			Vector3 position = center + size;
			_targetPoint.position = position;
		}
		target.character.health.onTakeDamage.Add(int.MinValue, (TakeDamageDelegate)MultiplyCriticalDamage);
		return false;
		bool MultiplyCriticalDamage(ref Damage takeDamage)
		{
			target.character.health.onTakeDamage.Remove((TakeDamageDelegate)MultiplyCriticalDamage);
			if (takeDamage.critical)
			{
				takeDamage.percentMultiplier *= _criticalDamageMultiplier;
				((MonoBehaviour)base.character).StartCoroutine(_onTargetHit.CRun(base.character));
			}
			return false;
		}
	}

	public override void Detach()
	{
		base.Detach();
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
	}
}
