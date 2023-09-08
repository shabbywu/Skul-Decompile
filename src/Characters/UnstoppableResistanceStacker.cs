using System;
using UnityEngine;

namespace Characters;

[RequireComponent(typeof(Character))]
public sealed class UnstoppableResistanceStacker : MonoBehaviour
{
	[Serializable]
	private class StackController
	{
		[SerializeField]
		private float refreshTime = 1f;

		[SerializeField]
		[Header("증가 설정, 상태이상 발생시 increaseValue값 만큼 스탯 증가, maintainTime만큼 유지")]
		private float _increaseValue;

		[SerializeField]
		private float _maintainTime;

		[Header("감소 설정, reduceInterval마다 reduceValue만큼 스탯 감소시킴")]
		[SerializeField]
		private float _reduceInterval;

		[SerializeField]
		private float _reduceValue;

		[SerializeField]
		[Header("범위 및 스탯 설정")]
		[MinMaxSlider(0f, 100f)]
		private Vector2 _statValueMinMax;

		[SerializeField]
		private Stat.Values _targetStat;

		private Character _owner;

		private Stat.Values _stackableStat;

		private float _remainMaintainTime;

		private float _remainReduceTime;

		private float _cacheValue;

		private float _remainRefreshTime;

		internal void Initialize(Character character, CharacterStatus.Kind kind)
		{
			_increaseValue *= 0.01f;
			_reduceValue *= 0.01f;
			_statValueMinMax.x *= 0.01f;
			_statValueMinMax.y *= 0.01f;
			_cacheValue = float.MinValue;
			_owner = character;
			_stackableStat = _targetStat.Clone();
			character.stat.AttachValues(_stackableStat);
			switch (kind)
			{
			case CharacterStatus.Kind.Freeze:
				character.status.freeze.onAttachEvents += IncreaseValue;
				character.status.freeze.onRefreshEvents += TryIncreaseValue;
				break;
			case CharacterStatus.Kind.Stun:
				character.status.stun.onAttachEvents += IncreaseValue;
				character.status.stun.onRefreshEvents += TryIncreaseValue;
				break;
			}
		}

		internal void UpdateTime(float deltaTime)
		{
			_remainMaintainTime -= deltaTime;
			_remainRefreshTime -= deltaTime;
			if (_remainMaintainTime <= 0f)
			{
				_remainReduceTime -= deltaTime;
				if (_remainReduceTime < 0f)
				{
					ReduceValue();
					_remainReduceTime += _reduceInterval;
				}
			}
		}

		private void ReduceValue()
		{
			for (int i = 0; i < _stackableStat.values.Length; i++)
			{
				float num = Mathf.Clamp((float)_stackableStat.values[i].value + _reduceValue, _statValueMinMax.x, _statValueMinMax.y);
				if (_cacheValue == num)
				{
					return;
				}
				_cacheValue = num;
				_stackableStat.values[i].value = num;
			}
			_owner.stat.SetNeedUpdate();
		}

		private void TryIncreaseValue(Character giver, Character taker)
		{
			if (!(_remainRefreshTime > 0f))
			{
				IncreaseValue(giver, taker);
			}
		}

		private void IncreaseValue(Character giver, Character taker)
		{
			_remainMaintainTime = _maintainTime;
			_remainRefreshTime = refreshTime;
			for (int i = 0; i < _stackableStat.values.Length; i++)
			{
				float num = Mathf.Clamp((float)_stackableStat.values[i].value - _increaseValue, _statValueMinMax.x, _statValueMinMax.y);
				if (_cacheValue == num)
				{
					return;
				}
				_cacheValue = num;
				_stackableStat.values[i].value = num;
			}
			_owner.stat.SetNeedUpdate();
		}
	}

	[GetComponent]
	[SerializeField]
	private Character _owner;

	[SerializeField]
	private StackController _freezeController;

	[SerializeField]
	private StackController _stunController;

	private void Awake()
	{
		_freezeController.Initialize(_owner, CharacterStatus.Kind.Freeze);
		_stunController.Initialize(_owner, CharacterStatus.Kind.Stun);
	}

	private void Update()
	{
		_freezeController.UpdateTime(_owner.chronometer.master.deltaTime);
		_stunController.UpdateTime(_owner.chronometer.master.deltaTime);
	}
}
