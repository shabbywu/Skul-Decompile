using System.Collections;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level;

public class WaveAction : MonoBehaviour
{
	[GetComponent]
	[SerializeField]
	private Character _character;

	[SerializeField]
	private Wave _target;

	[SerializeField]
	private bool _alsoClear;

	[Subcomponent(typeof(OperationInfos))]
	[SerializeField]
	private OperationInfos _operations;

	private bool _run;

	private void Awake()
	{
		_operations.Initialize();
		_target.onClear += Run;
		if (_alsoClear)
		{
			((MonoBehaviour)this).StartCoroutine(CCheckAlsoClear());
		}
	}

	private void Run()
	{
		if (!_run)
		{
			_operations.Run(_character);
			_run = true;
		}
	}

	private IEnumerator CCheckAlsoClear()
	{
		while ((Object)(object)_target != (Object)null && _target.state != Wave.State.Stopped)
		{
			yield return null;
		}
		if ((Object)(object)_target != (Object)null)
		{
			Run();
		}
	}
}
