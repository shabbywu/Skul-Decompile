using UnityEngine;

namespace Characters.Abilities.Statuses;

public sealed class OverrideGigantBossStatusEffect : MonoBehaviour
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private StatusEffect.GigantEnemyFreeze _gigantEnemyFreeze;

	[SerializeField]
	private StatusEffect.Burn _burn;

	[SerializeField]
	private StatusEffect.Poison _poison;

	[SerializeField]
	private StatusEffect.Wound _wound;

	[SerializeField]
	private StatusEffect.Stun _stun;

	private void Start()
	{
		_character.status.freeze.effectHandler.Dispose();
		_character.status.freeze.effectHandler = StatusEffect.CopyFrom(_gigantEnemyFreeze, _character);
		_character.status.burn.effectHandler.Dispose();
		_character.status.burn.effectHandler = StatusEffect.CopyFrom(_burn, _character);
		_character.status.poison.effectHandler.Dispose();
		_character.status.poison.effectHandler = StatusEffect.CopyFrom(_poison, _character);
		_character.status.wound.effectHandler.Dispose();
		_character.status.wound.effectHandler = StatusEffect.CopyFrom(_wound, _character);
		_character.status.stun.effectHandler.Dispose();
		_character.status.stun.effectHandler = StatusEffect.CopyFrom(_stun, _character);
	}
}
