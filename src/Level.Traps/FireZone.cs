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
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Unknown result type (might be due to invalid IL or missing references)
		if (_repeat)
		{
			_coroutineReference = CoroutineReferenceExtension.StartCoroutineWithReference((MonoBehaviour)(object)this, CRun());
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
			yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)_character.chronometer.master, _interval);
		}
	}

	public override void Deactivate()
	{
		((CoroutineReference)(ref _coroutineReference)).Stop();
	}
}
