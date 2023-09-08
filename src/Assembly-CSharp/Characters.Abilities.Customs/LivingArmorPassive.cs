using System;
using Characters.Gear.Weapons;
using Characters.Gear.Weapons.Gauges;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class LivingArmorPassive : Ability, IAbilityInstance
{
	[SerializeField]
	private ValueGauge _gauge;

	[SerializeField]
	private Weapon _weapon;

	[SerializeField]
	private SkillInfo[] _skills;

	[SerializeField]
	private SkillInfo[] _enhancedSkills;

	[SerializeField]
	private float _attackOperationInterval;

	private float _attackOperationRemainTime;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _attackOperations;

	private CoroutineReference _attackOperationRunner;

	private EffectPoolInstance _loopEffectInstance;

	public Character owner { get; set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon => _defaultIcon;

	public float iconFillAmount => 0f;

	public int iconStacks => 0;

	public bool expired { get; private set; }

	public override void Initialize()
	{
		base.Initialize();
		_attackOperations.Initialize();
		for (int i = 0; i < _skills.Length; i++)
		{
			_enhancedSkills[i].action.onStart += Expire;
		}
	}

	private void Expire()
	{
		if (_gauge.isMax() || _gauge.gaugePercent <= _gauge.defaultBarGaugeColor.proportion)
		{
			_gauge.Clear();
		}
		else
		{
			float num = _gauge.maxValue * _gauge.defaultBarGaugeColor.proportion;
			float num2 = _gauge.maxValue * _gauge.secondBarGaugeColor.proportion;
			float num3 = (_gauge.currentValue - num) / num2;
			float value = _gauge.maxValue * _gauge.defaultBarGaugeColor.proportion * num3;
			_gauge.Set(value);
		}
		expired = true;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		this.owner = owner;
		return this;
	}

	public void UpdateTime(float deltaTime)
	{
		_attackOperationRemainTime -= deltaTime;
		if (_attackOperationRemainTime < 0f)
		{
			_attackOperationRemainTime += _attackOperationInterval;
			_attackOperationRunner.Stop();
			_attackOperationRunner = ((MonoBehaviour)(object)owner).StartCoroutineWithReference(_attackOperations.CRun(owner));
		}
	}

	public void Refresh()
	{
	}

	public void Attach()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		_attackOperationRemainTime = 0f;
		expired = false;
		_weapon.AttachSkillChanges(_skills, _enhancedSkills);
		_loopEffectInstance = ((base.loopEffect == null) ? null : base.loopEffect.Spawn(((Component)owner).transform.position, owner));
	}

	public void Detach()
	{
		_weapon.DetachSkillChanges(_skills, _enhancedSkills);
		if ((Object)(object)_loopEffectInstance != (Object)null)
		{
			_loopEffectInstance.Stop();
			_loopEffectInstance = null;
		}
	}
}
