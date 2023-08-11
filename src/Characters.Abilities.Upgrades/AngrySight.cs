using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Upgrades;

[Serializable]
public sealed class AngrySight : Ability
{
	public sealed class Instance : AbilityInstance<AngrySight>
	{
		private float _remainAngryTime;

		private bool _buffAttached;

		private CharacterSpotLight _spotLight;

		public override Sprite icon
		{
			get
			{
				if (!_buffAttached)
				{
					return null;
				}
				return base.icon;
			}
		}

		public override float iconFillAmount => _remainAngryTime / ability._angryTime;

		public Instance(Character owner, AngrySight ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onTakeDamage.Add(0, (TakeDamageDelegate)HandleOnTakeDamage);
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Add(0, (GiveDamageDelegate)HandleOnGiveDamage);
			_spotLight = ((Component)owner).GetComponent<CharacterSpotLight>();
		}

		private bool HandleOnTakeDamage(ref Damage damage)
		{
			if (damage.attackType == Damage.AttackType.None || damage.amount < 1.0)
			{
				return false;
			}
			_remainAngryTime = ability._angryTime;
			if (!_buffAttached)
			{
				DetachInvulnerable();
				_buffAttached = true;
				return true;
			}
			_buffAttached = true;
			return false;
		}

		private bool HandleOnGiveDamage(ITarget target, ref Damage damage)
		{
			if (!_buffAttached)
			{
				return false;
			}
			damage.percentMultiplier *= ability._damagePercentMultiplier;
			return false;
		}

		public override void UpdateTime(float deltaTime)
		{
			base.UpdateTime(deltaTime);
			_remainAngryTime -= deltaTime;
			if (_remainAngryTime <= 0f)
			{
				if (_buffAttached)
				{
					Down();
				}
				_buffAttached = false;
			}
		}

		private void DetachInvulnerable()
		{
			//IL_0038: Unknown result type (might be due to invalid IL or missing references)
			owner.invulnerable.Detach((object)this);
			_spotLight.Activate();
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._attachAudioClipInfo, ((Component)owner).transform.position);
		}

		private void Down()
		{
			//IL_0037: Unknown result type (might be due to invalid IL or missing references)
			owner.invulnerable.Attach((object)this);
			_spotLight.Deactivate();
			PersistentSingleton<SoundManager>.Instance.PlaySound(ability._detachAudioClipInfo, ((Component)owner).transform.position);
		}

		protected override void OnDetach()
		{
			owner.health.onTakeDamage.Remove((TakeDamageDelegate)HandleOnTakeDamage);
			((PriorityList<GiveDamageDelegate>)owner.onGiveDamage).Remove((GiveDamageDelegate)HandleOnGiveDamage);
			owner.invulnerable.Detach((object)this);
			_spotLight.Deactivate();
		}
	}

	[SerializeField]
	private SoundInfo _attachAudioClipInfo;

	[SerializeField]
	private SoundInfo _detachAudioClipInfo;

	[SerializeField]
	private float _damagePercentMultiplier = 1f;

	[SerializeField]
	private float _distance;

	[SerializeField]
	private float _angryTime;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
