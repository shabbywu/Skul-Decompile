using UnityEngine;

namespace Characters.Abilities.Customs;

public class DetachInvulnerableComponent : AbilityComponent<DetachInvulnerable>
{
	[SerializeField]
	private Character _owner;

	[SerializeField]
	private Transform _key;

	private void Awake()
	{
		_owner.invulnerable.Attach((object)_key);
	}
}
