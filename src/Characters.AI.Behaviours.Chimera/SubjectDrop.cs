using System.Collections;
using System.Collections.Generic;
using Characters.Actions;
using Characters.Operations;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera;

public class SubjectDrop : Behaviour
{
	[SerializeField]
	[Header("Ready")]
	private OperationInfos _readyOperations;

	[SerializeField]
	[Header("Roar")]
	private OperationInfos _roarOperations;

	[SerializeField]
	[Header("Fire")]
	private SequentialAction _fireSequencialAction;

	[SerializeField]
	private OperationInfos[] _fireOperationInfos;

	[SerializeField]
	[Header("End")]
	private OperationInfos _endOperations;

	[SerializeField]
	private float _coolTime;

	[SerializeField]
	private float _term = 2f;

	[Range(1f, 10f)]
	[SerializeField]
	private float _range;

	[SerializeField]
	private float _height;

	[SerializeField]
	private Transform points;

	[SerializeField]
	private Transform _startPoint;

	private Coroutine _coolDown;

	public bool canUse { get; set; } = true;


	private void Awake()
	{
		_readyOperations.Initialize();
		_roarOperations.Initialize();
		_endOperations.Initialize();
		for (int i = 0; i < _fireOperationInfos.Length; i++)
		{
			_fireOperationInfos[i].Initialize();
		}
	}

	public void Ready(Character character)
	{
		((Component)_readyOperations).gameObject.SetActive(true);
		_readyOperations.Run(character);
	}

	public void Roar(Character character)
	{
		((Component)_roarOperations).gameObject.SetActive(true);
		_roarOperations.Run(character);
	}

	public void End(Character character)
	{
		((Component)_endOperations).gameObject.SetActive(true);
		_endOperations.Run(character);
	}

	public void Run(AIController controller)
	{
		if (_coolDown != null)
		{
			((MonoBehaviour)this).StopCoroutine(_coolDown);
		}
		((MonoBehaviour)this).StartCoroutine(CRun(controller));
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		_coolDown = ((MonoBehaviour)this).StartCoroutine(CoolDown(controller.character.chronometer.animation));
		SetPoints();
		ExtensionMethods.Shuffle<OperationInfos>((IList<OperationInfos>)_fireOperationInfos);
		for (int i = 0; i < _fireOperationInfos.Length; i++)
		{
			((Component)_fireOperationInfos[i]).gameObject.SetActive(true);
			_fireOperationInfos[i].Run(controller.character);
			yield return Chronometer.global.WaitForSeconds(0.5f);
		}
		base.result = Result.Done;
	}

	private void SetPoints()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < points.childCount; i++)
		{
			float num = Random.Range(0f, _range);
			points.GetChild(i).position = Vector2.op_Implicit(new Vector2(((Component)_startPoint).transform.position.x + num + (_range + _term) * (float)i, _height));
		}
	}

	private IEnumerator CoolDown(Chronometer chronometer)
	{
		canUse = false;
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _coolTime);
		canUse = true;
	}
}
