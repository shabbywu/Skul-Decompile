using UnityEngine;

namespace Characters.Monsters;

[DisallowMultipleComponent]
[RequireComponent(typeof(Character))]
public class Monster : MonoBehaviour
{
	public delegate void OnDespawnDelegate();

	[SerializeField]
	[GetComponent]
	private PoolObject _poolObject;

	[SerializeField]
	[GetComponent]
	private Character _character;

	public PoolObject poolObject => _poolObject;

	public Character character => _character;

	public event OnDespawnDelegate OnDespawn;

	public Monster Summon(Vector3 position)
	{
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		Monster component = ((Component)_poolObject.Spawn(true)).GetComponent<Monster>();
		((Component)component).transform.position = position;
		component.character.health.Revive();
		return component;
	}

	public void Despawn()
	{
		this.OnDespawn?.Invoke();
		_poolObject.Despawn();
	}
}
