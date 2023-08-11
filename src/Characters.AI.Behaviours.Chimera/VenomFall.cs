using System.Collections;
using System.Collections.Generic;
using Characters.Operations;
using Data;
using UnityEngine;

namespace Characters.AI.Behaviours.Chimera;

public class VenomFall : Behaviour
{
	[SerializeField]
	private float _coolTime;

	[Header("Ready")]
	[SerializeField]
	private OperationInfos _readyOperations;

	[Header("Roar")]
	[SerializeField]
	private OperationInfos _roarOperations;

	[SerializeField]
	[Header("Fire")]
	private OperationInfos[] _operations;

	[SerializeField]
	[Header("Hardmode End")]
	private float _endOperationTimingFromStart = 3.55f;

	[SerializeField]
	private OperationInfos _endOperationsInHardmode;

	[SerializeField]
	private float _term = 2f;

	[SerializeField]
	[Range(1f, 10f)]
	private float _range;

	[SerializeField]
	private Transform points;

	[SerializeField]
	private Transform _startPoint;

	private const int _maxOrder = 4;

	private Queue<int> _order;

	private bool _coolDown = true;

	private bool _running;

	private void Awake()
	{
		for (int i = 0; i < 4; i++)
		{
			_operations[i].Initialize();
		}
		_readyOperations.Initialize();
		_roarOperations.Initialize();
		_endOperationsInHardmode.Initialize();
	}

	public void ShuffleOrder()
	{
		int[] array = new int[4];
		for (int i = 0; i < 4; i++)
		{
			array[i] = i;
		}
		ExtensionMethods.Shuffle<int>((IList<int>)array);
		_order = new Queue<int>(4);
		for (int j = 0; j < 4; j++)
		{
			_order.Enqueue(array[j]);
		}
		SetPoints();
	}

	public void Ready(Character character)
	{
		ShuffleOrder();
		((Component)_readyOperations).gameObject.SetActive(true);
		_readyOperations.Run(character);
		((MonoBehaviour)this).StartCoroutine(CoolDown(character.chronometer.animation));
	}

	public void Roar(Character character)
	{
		((Component)_roarOperations).gameObject.SetActive(true);
		_roarOperations.Run(character);
	}

	public void EndRoar(Character character)
	{
	}

	public override IEnumerator CRun(AIController controller)
	{
		base.result = Result.Doing;
		Character character = controller.character;
		if (!_running)
		{
			((MonoBehaviour)this).StartCoroutine(CStartFall(character));
		}
		base.result = Result.Done;
		yield break;
	}

	private IEnumerator CStartFall(Character owner)
	{
		float term = 0.8f;
		float elapsed = 0f;
		_running = true;
		while (_order.Count > 0)
		{
			yield return null;
			elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
			if (elapsed > term)
			{
				elapsed -= term;
				Next(owner);
			}
		}
		_running = false;
	}

	private void Next(Character owner)
	{
		int num = _order.Dequeue();
		((Component)_operations[num]).gameObject.SetActive(true);
		_operations[num].Run(owner);
		if (_order.Count == 1)
		{
			int num2 = _order.Dequeue();
			((Component)_operations[num2]).gameObject.SetActive(true);
			_operations[num2].Run(owner);
			if (GameData.HardmodeProgress.hardmode)
			{
				((MonoBehaviour)this).StartCoroutine(CStartEndFall(owner));
			}
		}
	}

	private IEnumerator CStartEndFall(Character owner)
	{
		yield return Chronometer.global.WaitForSeconds(_endOperationTimingFromStart);
		if (!owner.health.dead)
		{
			((Component)_endOperationsInHardmode).gameObject.SetActive(true);
			_endOperationsInHardmode.Run(owner);
		}
	}

	private void SetPoints()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0054: Unknown result type (might be due to invalid IL or missing references)
		//IL_005e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		for (int i = 0; i < points.childCount; i++)
		{
			float num = Random.Range(0f, _range);
			points.GetChild(i).position = Vector2.op_Implicit(new Vector2(((Component)_startPoint).transform.position.x + num + (_range + _term) * (float)i, ((Component)_startPoint).transform.position.y));
		}
	}

	private IEnumerator CoolDown(Chronometer chronometer)
	{
		_coolDown = false;
		yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _coolTime);
		_coolDown = true;
	}

	public bool CanUse()
	{
		return _coolDown;
	}
}
