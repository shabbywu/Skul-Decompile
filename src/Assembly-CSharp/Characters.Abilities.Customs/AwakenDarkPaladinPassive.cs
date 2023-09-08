using System;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class AwakenDarkPaladinPassive : Ability, IAbilityInstance
{
	public Character owner { get; set; }

	public IAbility ability => this;

	public float remainTime { get; set; }

	public bool attached => true;

	public Sprite icon => _defaultIcon;

	public float iconFillAmount => 1f - remainTime / base.duration;

	public bool expired => remainTime <= 0f;

	public int iconStacks
	{
		get
		{
			throw new NotImplementedException();
		}
	}

	public void Refresh()
	{
		remainTime = base.duration;
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
		remainTime = base.duration;
	}

	public void Detach()
	{
	}
}
