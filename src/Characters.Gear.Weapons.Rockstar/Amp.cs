using System;
using System.Collections.Generic;
using Characters.Operations;
using UnityEngine;

namespace Characters.Gear.Weapons.Rockstar;

public class Amp : MonoBehaviour
{
	[Serializable]
	private class Timing
	{
		[SerializeField]
		private OperationInfos _operation;

		[SerializeField]
		private float _timing;

		public OperationInfos operation => _operation;

		public float timming => _timing;

		public void Initialize()
		{
			_operation.Initialize();
		}

		public void Run(Character owner)
		{
			((Component)_operation).gameObject.SetActive(true);
			_operation.Run(owner);
		}
	}

	[GetComponentInParent(false)]
	[SerializeField]
	private Weapon _weapon;

	[SerializeField]
	[Tooltip("앰프 프리팹")]
	private OperationRunner _ampOriginal;

	[SerializeField]
	[Tooltip("몇 박자마다 반복 할 것인지 기입")]
	private int _beat = 1;

	[SerializeField]
	[Tooltip("Beat에서 지정 한 박자 내에서 아래 지정한 백분률 지점마다 정해준 OperationInfos를 실행하게 됨\n예를 들어 Beat가 2인 상태로 0, 0.5 두 지점에서 동일한 OpeartionInfos를 실행하면 한 박자에 한 번씩 실행 됨")]
	private Timing[] _timings;

	private List<OperationRunner> _ampObjects = new List<OperationRunner>();

	public int beat
	{
		get
		{
			return _beat;
		}
		private set
		{
			_beat = value;
		}
	}

	public bool ampExists => _ampObjects.Count > 0;

	public event Action onInstantiate;

	private void Awake()
	{
		List<OperationInfos> list = new List<OperationInfos>();
		Timing[] timings = _timings;
		foreach (Timing timing in timings)
		{
			if (!list.Contains(timing.operation))
			{
				list.Add(timing.operation);
				timing.operation.Initialize();
			}
		}
	}

	public void InstantiateAmp(Vector3 position, bool flipX)
	{
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0013: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		OperationRunner operationRunner = _ampOriginal.Spawn();
		((Component)operationRunner).transform.SetPositionAndRotation(position, Quaternion.identity);
		int num = ((!((_weapon.owner.lookingDirection == Character.LookingDirection.Left) ^ flipX)) ? 1 : (-1));
		((Component)operationRunner).transform.localScale = new Vector3((float)num, 1f, 1f);
		((Component)operationRunner).GetComponent<SpriteRenderer>().flipX = flipX;
		operationRunner.operationInfos.Run(_weapon.owner);
		_ampObjects.Add(operationRunner);
		this.onInstantiate?.Invoke();
	}

	public void PlayAmpBeat(int index)
	{
		//IL_0051: Unknown result type (might be due to invalid IL or missing references)
		//IL_0081: Unknown result type (might be due to invalid IL or missing references)
		//IL_009e: Unknown result type (might be due to invalid IL or missing references)
		for (int num = _ampObjects.Count - 1; num >= 0; num--)
		{
			OperationRunner operationRunner = _ampObjects[num];
			if ((Object)(object)operationRunner == (Object)null || !((Component)operationRunner).gameObject.activeSelf)
			{
				_ampObjects.Remove(operationRunner);
			}
			else
			{
				((Component)this).transform.position = ((Component)operationRunner).transform.position;
				float num2 = ((_weapon.owner.lookingDirection == Character.LookingDirection.Right) ? 1f : (-1f));
				num2 *= ((Component)operationRunner).transform.localScale.x;
				((Component)this).transform.localScale = new Vector3(num2, 1f, 1f);
				_timings[index].Run(_weapon.owner);
			}
		}
	}

	public float[] GetTimings()
	{
		float[] array = new float[_timings.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = _timings[i].timming / (float)beat;
		}
		return array;
	}
}
