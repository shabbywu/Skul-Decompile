using System;
using Characters.Gear.Weapons;
using Characters.Gear.Weapons.Gauges;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class Samurai2Passive : Ability, IAbilityInstance
{
	[SerializeField]
	private ValueGauge _gauge;

	[SerializeField]
	private Weapon _weapon;

	[SerializeField]
	private SkillInfo[] _skills;

	[SerializeField]
	private SkillInfo[] _enhancedSkills;

	[Header("Operation")]
	[SerializeField]
	private float _operationInterval;

	private float _operationRemainTime;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	private CoroutineReference _operationRunner;

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
		_operations.Initialize();
		for (int i = 0; i < _skills.Length; i++)
		{
			_enhancedSkills[i].action.onStart += Expire;
		}
	}

	private void Expire()
	{
		_gauge.Clear();
		expired = true;
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		this.owner = owner;
		return this;
	}

	public void UpdateTime(float deltaTime)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		_operationRemainTime -= deltaTime;
		if (_operationRemainTime < 0f)
		{
			_operationRemainTime += _operationInterval;
			((CoroutineReference)(ref _operationRunner)).Stop();
			_operationRunner = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)owner, _operations.CRun(owner));
		}
	}

	public void Refresh()
	{
	}

	public void Attach()
	{
		//IL_0044: Unknown result type (might be due to invalid IL or missing references)
		_operationRemainTime = 0f;
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
