using Characters.Operations;
using Level;
using UnityEditor;
using UnityEngine;

namespace Characters.Monsters;

public sealed class MonsterOnDied : MonoBehaviour
{
	[SerializeField]
	private Monster _monster;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	private void Awake()
	{
		_operations.Initialize();
		_monster.character.health.onDied += delegate
		{
			_monster.character.health.Revive();
			_monster.Despawn();
		};
	}

	private void HandleOnDied()
	{
		((MonoBehaviour)Map.Instance).StartCoroutine(_operations.CRun(_monster.character));
	}

	public void DetachOnDiedEvents()
	{
		_monster.character.health.onDied -= HandleOnDied;
	}

	private void OnEnable()
	{
		_monster.character.health.onDied -= HandleOnDied;
		_monster.character.health.onDied += HandleOnDied;
	}

	private void OnDisable()
	{
		DetachOnDiedEvents();
	}
}
