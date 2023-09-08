using System.Collections;
using Characters;
using Characters.Operations;
using UnityEditor;
using UnityEngine;

namespace Level.Traps;

public class FireZone : ControlableTrap
{
	[SerializeField]
	private Character _character;

	[SerializeField]
	private float _interval;

	[SerializeField]
	private Collider2D _range;

	[SerializeField]
	private bool _repeat;

	[SerializeField]
	private bool _activeManually;

	[SerializeField]
	[Subcomponent(typeof(OperationInfo))]
	private OperationInfo.Subcomponents _operations;

	private CoroutineReference _coroutineReference;

	private void Start()
	{
		_operations.Initialize();
		if (!_activeManually)
		{
			Activate();
		}
	}

	public override void Activate()
	{
		if (_repeat)
		{
			_coroutineReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CRun());
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(_operations.CRun(_character));
		}
	}

	private IEnumerator CRun()
	{
		while (true)
		{
			yield return _operations.CRun(_character);
			yield return _character.chronometer.master.WaitForSeconds(_interval);
		}
	}

	public override void Deactivate()
	{
		_coroutineReference.Stop();
	}
}
