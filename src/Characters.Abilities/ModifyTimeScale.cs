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
			return (Chronometer)(ability._chronometerType switch
			{
				ChronometerType.Animation => owner.chronometer.animation, 
				ChronometerType.Effect => owner.chronometer.effect, 
				ChronometerType.Projectile => owner.chronometer.projectile, 
				_ => owner.chronometer.master, 
			});
		}

		protected override void OnAttach()
		{
			((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, ability._globalTimeScale);
			((ChronometerBase)GetChronometer()).AttachTimeScale((object)this, ability._timeScale);
		}

		protected override void OnDetach()
		{
			((ChronometerBase)Chronometer.global).DetachTimeScale((object)this);
			((ChronometerBase)GetChronometer()).DetachTimeScale((object)this);
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
