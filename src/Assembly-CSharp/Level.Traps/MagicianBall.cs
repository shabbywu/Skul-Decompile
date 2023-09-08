using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps;

public class MagicianBall : Trap
{
	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _runOperations;

	private Character _character;

	private void Awake()
	{
		_character = ((Component)this).GetComponentInParent<Character>();
		_runOperations.Initialize();
	}

	private void OnEnable()
	{
		_runOperations.Initialize();
		_runOperations.Run(_character);
	}
}
