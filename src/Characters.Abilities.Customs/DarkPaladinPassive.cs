using System;
using Characters.Actions;
using Characters.Gear.Weapons.Gauges;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class DarkPaladinPassive : Ability, IAbilityInstance
{
	[SerializeField]
	private int _shieldAmount;

	[SerializeField]
	private ValueGauge _gauge;

	[SerializeField]
	private Stat.Values _stat;

	[Space]
	[SerializeField]
	private EnhanceableComboAction _enhanceableComboAction;

	[SerializeField]
	private EnhanceableChainAction _enhanceableChainAction;

	private EffectPoolInstance _loopEffectInstance;

	private Characters.Shield.Instance _shieldInstance;

	public int iconStacks => (int)_shieldInstance.amount;

	public Character owner { get; set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon => _defaultIcon;

	public float iconFillAmount => 1f - remainTime / base.duration;

	public bool expired => remainTime <= 0f;

	public void Refresh()
	{
		remainTime = base.duration;
		_shieldInstance.amount = _shieldAmount;
	}

	private void OnShieldBroke()
	{
		owner.ability.Remove(this);
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		this.owner = owner;
		return this;
	}

	public void UpdateTime(float deltaTime)
	{
		remainTime -= deltaTime;
	}

	public void Attach()
	{
		//IL_008d: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bc: Unknown result type (might be due to invalid IL or missing references)
		remainTime = base.duration;
		owner.stat.AttachValues(_stat);
		_shieldInstance = owner.health.shield.Add(ability, _shieldAmount, OnShieldBroke);
		_enhanceableComboAction.enhanced = true;
		if ((Object)(object)_enhanceableChainAction != (Object)null)
		{
			_enhanceableChainAction.enhanced = true;
		}
		base.effectOnAttach.Spawn(((Component)owner).transform.position);
		_loopEffectInstance = ((base.loopEffect == null) ? null : base.loopEffect.Spawn(((Component)owner).transform.position, owner));
	}

	public void Detach()
	{
		owner.stat.DetachValues(_stat);
		if (owner.health.shield.Remove(ability))
		{
			_shieldInstance = null;
		}
		_enhanceableComboAction.enhanced = false;
		if ((Object)(object)_enhanceableChainAction != (Object)null)
		{
			_enhanceableChainAction.enhanced = false;
		}
		if ((Object)(object)_loopEffectInstance != (Object)null)
		{
			_loopEffectInstance.Stop();
			_loopEffectInstance = null;
		}
	}
}
