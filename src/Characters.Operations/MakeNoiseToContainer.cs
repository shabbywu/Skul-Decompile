using System.Collections;
using UnityEngine;

namespace Characters.Operations;

public class MakeNoiseToContainer : CharacterOperation
{
	[SerializeField]
	private Transform _container;

	[SerializeField]
	private float _noise;

	[SerializeField]
	private float _restoreTime;

	private Vector2[] _origin;

	private void Awake()
	{
		_origin = (Vector2[])(object)new Vector2[_container.childCount];
	}

	public override void Run(Character owner)
	{
		//IL_001e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0023: Unknown result type (might be due to invalid IL or missing references)
		//IL_0028: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0039: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _container.childCount; i++)
		{
			Transform child = _container.GetChild(i);
			_origin[i] = Vector2.op_Implicit(((Component)child).transform.position);
			child.Translate(Random.insideUnitSphere * _noise);
		}
		if (_restoreTime > 0f)
		{
			((MonoBehaviour)this).StartCoroutine(CRestore(owner.chronometer.master));
		}
	}

	private IEnumerator CRestore(Chronometer chronometer)
	{
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _restoreTime);
		Restore();
	}

	private void Restore()
	{
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0021: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < _container.childCount; i++)
		{
			((Component)_container.GetChild(i)).transform.position = Vector2.op_Implicit(_origin[i]);
		}
	}
}
