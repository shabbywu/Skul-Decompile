using System;
using Characters.Abilities.Items;
using Characters.Operations;
using FX.SpriteEffects;
using GameResources;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class Revive : Ability
{
	public class Instance : AbilityInstance<Revive>
	{
		public Instance(Character owner, Revive ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.health.onDie += ReviveOwner;
		}

		protected override void OnDetach()
		{
			owner.health.onDie -= ReviveOwner;
		}

		private void ReviveOwner()
		{
			//IL_00cf: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d4: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e4: Unknown result type (might be due to invalid IL or missing references)
			//IL_0139: Unknown result type (might be due to invalid IL or missing references)
			if (owner.health.currentHealth > 0.0)
			{
				return;
			}
			ChosenHerosCirclet.Instance instanceByInstanceType = owner.ability.GetInstanceByInstanceType<ChosenHerosCirclet.Instance>();
			if (instanceByInstanceType == null || !instanceByInstanceType.canUse)
			{
				owner.health.onDie -= ReviveOwner;
				((ChronometerBase)Chronometer.global).AttachTimeScale((object)this, 0.2f, 0.5f);
				if (ability._percentHeal)
				{
					owner.health.PercentHeal((float)ability._percentHealAmount * 0.01f);
				}
				else
				{
					owner.health.Heal(ability._heal);
				}
				CommonResource.instance.reassembleParticle.Emit(Vector2.op_Implicit(((Component)owner).transform.position), ((Collider2D)owner.collider).bounds, owner.movement.push);
				owner.CancelAction();
				((ChronometerBase)owner.chronometer.master).AttachTimeScale((object)this, 0.01f, 0.5f);
				owner.spriteEffectStack.Add(new ColorBlend(int.MaxValue, Color.clear, 0.5f));
				GetInvulnerable getInvulnerable = new GetInvulnerable();
				getInvulnerable.duration = 3f;
				owner.spriteEffectStack.Add(new Invulnerable(0, 0.2f, getInvulnerable.duration));
				owner.ability.Add(getInvulnerable);
				if ((Object)(object)ability._ability != (Object)null)
				{
					owner.ability.Add(ability._ability.ability);
				}
				ability._operations.Run(owner);
				owner.ability.Remove(this);
			}
		}
	}

	[SerializeField]
	private int _heal = 30;

	[SerializeField]
	private bool _percentHeal;

	[Range(0f, 100f)]
	[SerializeField]
	private int _percentHealAmount;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _ability;

	[CharacterOperation.Subcomponent]
	[SerializeField]
	private CharacterOperation.Subcomponents _operations;

	public override void Initialize()
	{
		base.Initialize();
		_operations.Initialize();
		if ((Object)(object)_ability != (Object)null)
		{
			_ability.Initialize();
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
