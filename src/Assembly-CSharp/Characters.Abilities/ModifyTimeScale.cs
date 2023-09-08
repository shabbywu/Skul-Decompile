using System;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class ModifyTimeScale : Ability
{
	public enum ChronometerType
	{
		Master,
		Animation,
		Effect,
		Projectile
	}

	public class Instance : AbilityInstance<ModifyTimeScale>
	{
		public Instance(Character owner, ModifyTimeScale ability)
			: base(owner, ability)
		{
		}

		private Chronometer GetChronometer()
		{
			return ability._chronometerType switch
			{
				ChronometerType.Animation => owner.chronometer.animation, 
				ChronometerType.Effect => owner.chronometer.effect, 
				ChronometerType.Projectile => owner.chronometer.projectile, 
				_ => owner.chronometer.master, 
			};
		}

		protected override void OnAttach()
		{
			Chronometer.global.AttachTimeScale(this, ability._globalTimeScale);
			GetChronometer().AttachTimeScale(this, ability._timeScale);
		}

		protected override void OnDetach()
		{
			Chronometer.global.DetachTimeScale(this);
			GetChronometer().DetachTimeScale(this);
		}
	}

	[SerializeField]
	private ChronometerType _chronometerType;

	[SerializeField]
	private float _timeScale = 1f;

	[SerializeField]
	private float _globalTimeScale = 1f;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
