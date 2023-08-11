using Characters.Abilities.CharacterStat;
using Characters.Movements;
using UnityEngine;

namespace Characters.Gear.Synergy.Inscriptions;

public sealed class Soar : InscriptionInstance
{
	[SerializeField]
	[Header("2세트 효과")]
	private StatBonus _statBonus;

	[Header("4세트 효과")]
	[SerializeField]
	private float _damageMultiplier;

	protected override void Initialize()
	{
		_statBonus.Initialize();
	}

	public override void UpdateBonus(bool wasActive, bool wasOmen)
	{
	}

	public override void Attach()
	{
		base.character.movement.onJump += OnJump;
		base.character.movement.onGrounded += OnGrounded;
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Add(int.MaxValue, (GiveDamageDelegate)OnGiveDamage);
	}

	private bool OnGiveDamage(ITarget target, ref Damage damage)
	{
		if (keyword.step < keyword.steps.Count - 1)
		{
			return false;
		}
		if (base.character.movement.controller.isGrounded)
		{
			return false;
		}
		damage.percentMultiplier *= _damageMultiplier;
		return false;
	}

	private void OnGrounded()
	{
		if (keyword.step >= 1)
		{
			base.character.ability.Remove(_statBonus);
		}
	}

	private void OnJump(Movement.JumpType jumpType, float jumpHeight)
	{
		if (keyword.step >= 1)
		{
			base.character.ability.Add(_statBonus);
		}
	}

	public override void Detach()
	{
		base.character.movement.onJump -= OnJump;
		base.character.movement.onGrounded -= OnGrounded;
		((PriorityList<GiveDamageDelegate>)base.character.onGiveDamage).Remove((GiveDamageDelegate)OnGiveDamage);
		base.character.ability.Remove(_statBonus);
	}
}
