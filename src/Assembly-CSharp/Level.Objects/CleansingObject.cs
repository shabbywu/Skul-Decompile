using Characters;
using Characters.Abilities;
using FX;
using Singletons;
using UnityEngine;

namespace Level.Objects;

public class CleansingObject : InteractiveObject
{
	[SerializeField]
	[GetComponent]
	private Animator _animator;

	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private EffectInfo _effectinfo;

	private const string deactivateClipCode = "Deactivate";

	public override void InteractWith(Character character)
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		character.playerComponents.savableAbilityManager.Remove(SavableAbilityManager.Name.Curse);
		PersistentSingleton<SoundManager>.Instance.PlaySound(_interactSound, ((Component)this).transform.position);
		_effectinfo.Spawn(_spawnPosition.position);
		_animator.Play("Deactivate");
		Deactivate();
	}
}
