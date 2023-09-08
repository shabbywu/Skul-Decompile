using System;
using FX;
using Singletons;
using UnityEngine;

namespace Characters.Abilities;

[Serializable]
public class AttachAbilityToTargetOnGaveDamage : Ability
{
	public class Instance : AbilityInstance<AttachAbilityToTargetOnGaveDamage>
	{
		public Instance(Character owner, AttachAbilityToTargetOnGaveDamage ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Combine(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		protected override void OnDetach()
		{
			Character character = owner;
			character.onGaveDamage = (GaveDamageDelegate)Delegate.Remove(character.onGaveDamage, new GaveDamageDelegate(OnGaveDamage));
		}

		private void OnGaveDamage(ITarget target, in Damage originalDamage, in Damage gaveDamage, double damageDealt)
		{
			//IL_00ed: Unknown result type (might be due to invalid IL or missing references)
			if (!((Object)(object)target.character == (Object)null) && !target.character.health.dead && (!ability._needCritical || gaveDamage.critical) && (string.IsNullOrWhiteSpace(ability._attackKey) || gaveDamage.key.Equals(ability._attackKey, StringComparison.OrdinalIgnoreCase)) && ability._targetFilter[target.character.type] && ability._motionTypeFilter[gaveDamage.motionType] && ability._attackTypeFilter[gaveDamage.attackType])
			{
				target.character.ability.Add(ability._abilityComponent.ability);
				PersistentSingleton<SoundManager>.Instance.PlaySound(ability._attachAudioClipInfo, ((Component)owner).transform.position);
			}
		}
	}

	[SerializeField]
	private SoundInfo _attachAudioClipInfo;

	[SerializeField]
	private bool _needCritical;

	[SerializeField]
	private CharacterTypeBoolArray _targetFilter = new CharacterTypeBoolArray(true, true, true, true, true);

	[SerializeField]
	private MotionTypeBoolArray _motionTypeFilter;

	[SerializeField]
	private AttackTypeBoolArray _attackTypeFilter;

	[SerializeField]
	private string _attackKey;

	[AbilityComponent.Subcomponent]
	[SerializeField]
	private AbilityComponent _abilityComponent;

	public override void Initialize()
	{
		base.Initialize();
		_abilityComponent.Initialize();
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
