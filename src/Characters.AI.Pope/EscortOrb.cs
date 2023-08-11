using System;
using System.Collections;
using Characters.Operations;
using Characters.Operations.Attack;
using UnityEditor;
using UnityEngine;

namespace Characters.AI.Pope;

public class EscortOrb : MonoBehaviour
{
	[Serializable]
	private class Rotate
	{
		[SerializeField]
		internal float speed;
	}

	[Serializable]
	private class Fire
	{
		[Subcomponent(typeof(OperationInfos))]
		[SerializeField]
		private OperationInfos operationInfos;

		[SerializeField]
		internal float duration;

		internal void Initialize()
		{
			operationInfos.Initialize();
		}

		internal void Run(Character character)
		{
			((Component)operationInfos).gameObject.SetActive(true);
			operationInfos.Run(character);
		}
	}

	[SerializeField]
	private Character _character;

	[SerializeField]
	[Subcomponent(typeof(SweepAttack))]
	private SweepAttack _attack;

	[SerializeField]
	private Fire _fire;

	[SerializeField]
	private Transform _pivot;

	[SerializeField]
	private float _speed;

	private float _elapsed;

	private void Awake()
	{
		_fire.Initialize();
		_attack.Initialize();
	}

	private void OnEnable()
	{
		_attack.Initialize();
		_attack.Run(_character);
		((MonoBehaviour)this).StartCoroutine(CStartFireLoop());
	}

	public void Initialize(float startRadian)
	{
		_elapsed = startRadian;
	}

	public void Move(float radius)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_0025: Unknown result type (might be due to invalid IL or missing references)
		//IL_005a: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_007c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = ((Component)_pivot).transform.position - ((Component)_character).transform.position;
		_elapsed += _speed * ((ChronometerBase)_character.chronometer.master).deltaTime;
		_character.movement.MoveHorizontal(Vector2.op_Implicit(val) + new Vector2(Mathf.Cos(_elapsed), Mathf.Sin(_elapsed)) * radius);
	}

	private IEnumerator CStartFireLoop()
	{
		while (true)
		{
			yield return Chronometer.global.WaitForSeconds(_fire.duration);
			_fire.Run(_character);
		}
	}
}
