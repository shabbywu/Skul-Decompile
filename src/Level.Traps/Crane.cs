using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps;

public class Crane : Trap
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	[GetComponent]
	private SpriteRenderer _spriteRenderer;

	[SerializeField]
	private Sprite _wreckage;

	[Subcomponent(typeof(OperationInfo))]
	[SerializeField]
	private OperationInfo.Subcomponents _onHitOperations;

	private void Awake()
	{
		_onHitOperations.Initialize();
		_character.health.onDie += Run;
	}

	private void Run()
	{
		_character.health.onDie -= Run;
		_spriteRenderer.sprite = _wreckage;
		((MonoBehaviour)this).StartCoroutine(_onHitOperations.CRun(_character));
	}
}
