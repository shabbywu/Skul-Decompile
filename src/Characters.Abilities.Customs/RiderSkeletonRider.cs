using System;
using System.Collections.Generic;
using System.Linq;
using Characters.Operations;
using Characters.Operations.Customs;
using Services;
using Singletons;
using UnityEngine;

namespace Characters.Abilities.Customs;

[Serializable]
public class RiderSkeletonRider : Ability
{
	public class Instance : AbilityInstance<RiderSkeletonRider>
	{
		private List<Vector2> _positions;

		private List<float> _times;

		private List<float> _speeds;

		private List<Character.LookingDirection> _flips;

		private readonly int targetFrame = 60;

		public Instance(Character owner, RiderSkeletonRider ability)
			: base(owner, ability)
		{
			int capacity = Mathf.CeilToInt((float)targetFrame * (ability._riderDelays.Last() + 1f));
			_positions = new List<Vector2>(capacity);
			_times = new List<float>(capacity);
			_speeds = new List<float>(capacity);
			_flips = new List<Character.LookingDirection>(capacity);
		}

		protected override void OnAttach()
		{
			OperationInfos[] riders = ability._riders;
			foreach (OperationInfos obj in riders)
			{
				((Component)obj).gameObject.SetActive(false);
				((Component)obj).transform.parent = null;
			}
			Singleton<Service>.Instance.levelManager.onMapLoaded += OnMapLoaded;
		}

		protected override void OnDetach()
		{
			if (!Service.quitting)
			{
				for (int i = 0; i < ability._riders.Length; i++)
				{
					OperationInfos obj = ability._riders[i];
					RiderEndingOperations obj2 = ability._riderEndingOperations[i];
					obj2.Initialize();
					obj2.Run(owner);
					((Component)obj).gameObject.SetActive(false);
					((Component)obj).transform.parent = ((Component)owner).transform;
				}
				Singleton<Service>.Instance.levelManager.onMapLoaded -= OnMapLoaded;
			}
		}

		private void OnMapLoaded()
		{
			OperationInfos[] riders = ability._riders;
			for (int i = 0; i < riders.Length; i++)
			{
				((Component)riders[i]).gameObject.SetActive(false);
			}
			_positions.Clear();
			_times.Clear();
			_speeds.Clear();
			_flips.Clear();
		}

		public override void UpdateTime(float deltaTime)
		{
			//IL_0018: Unknown result type (might be due to invalid IL or missing references)
			//IL_001d: Unknown result type (might be due to invalid IL or missing references)
			base.UpdateTime(deltaTime);
			_positions.Add(Vector2.op_Implicit(((Component)owner).transform.position));
			_times.Add(deltaTime);
			_speeds.Add(owner.animationController.parameter.movementSpeed);
			_flips.Add(owner.lookingDirection);
			SortRiders();
		}

		private void SortRiders()
		{
			//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
			//IL_00d0: Unknown result type (might be due to invalid IL or missing references)
			//IL_00dd: Unknown result type (might be due to invalid IL or missing references)
			//IL_00e2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a2: Unknown result type (might be due to invalid IL or missing references)
			//IL_00a7: Unknown result type (might be due to invalid IL or missing references)
			//IL_0127: Unknown result type (might be due to invalid IL or missing references)
			//IL_0120: Unknown result type (might be due to invalid IL or missing references)
			float num = 0f;
			int num2 = 0;
			float num3 = ability._riderDelays[0];
			for (int num4 = _times.Count - 1; num4 >= 0; num4--)
			{
				float num5 = num + _times[num4];
				if (num3 >= num && num3 < num5)
				{
					OperationInfos operationInfos = ability._riders[num2];
					if (!((Component)operationInfos).gameObject.activeSelf)
					{
						((Component)operationInfos).gameObject.SetActive(true);
						operationInfos.Initialize();
						operationInfos.Run(owner);
					}
					if (num4 == _positions.Count - 1)
					{
						((Component)operationInfos).transform.position = Vector2.op_Implicit(_positions[num4]);
					}
					else
					{
						((Component)operationInfos).transform.position = Vector2.op_Implicit(Vector2.LerpUnclamped(_positions[num4 + 1], _positions[num4], (num3 - num) / (num5 - num)));
					}
					ability._riderAnimators[num2].speed = _speeds[num4];
					((Component)operationInfos).transform.localScale = ((_flips[num4] == Character.LookingDirection.Right) ? lookingRight : lookingLeft);
					num2++;
					if (num2 == ability._riders.Length)
					{
						if (num4 > Application.targetFrameRate)
						{
							_times.RemoveRange(0, num4);
							_positions.RemoveRange(0, num4);
							_speeds.RemoveRange(0, num4);
							_flips.RemoveRange(0, num4);
						}
						break;
					}
					num3 = ability._riderDelays[num2];
				}
				num = num5;
			}
		}
	}

	private static readonly Vector3 lookingRight = new Vector3(1f, 1f, 1f);

	private static readonly Vector3 lookingLeft = new Vector3(-1f, 1f, 1f);

	[SerializeField]
	private OperationInfos[] _riders;

	[SerializeField]
	private float[] _riderDelays;

	private Animator[] _riderAnimators;

	private RiderEndingOperations[] _riderEndingOperations;

	private Action onMapLoaded;

	private void OnEnable()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded += InvokeOnMapLoaded;
	}

	private void OnDisable()
	{
		Singleton<Service>.Instance.levelManager.onMapLoaded -= InvokeOnMapLoaded;
	}

	private void InvokeOnMapLoaded()
	{
		onMapLoaded();
	}

	public override void Initialize()
	{
		base.Initialize();
		_riderAnimators = (Animator[])(object)new Animator[_riders.Length];
		_riderEndingOperations = new RiderEndingOperations[_riders.Length];
		for (int i = 0; i < _riders.Length; i++)
		{
			OperationInfos operationInfos = _riders[i];
			_riderAnimators[i] = ((Component)operationInfos).GetComponent<Animator>();
			_riderEndingOperations[i] = ((Component)operationInfos).GetComponent<RiderEndingOperations>();
		}
	}

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
