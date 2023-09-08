using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks.Custom;

[TaskDescription("일정량의 데미지를 받을 때 까지 Running을 실행")]
public sealed class RunningUntilAmountOfDamage : Action
{
	[SerializeField]
	private SharedCharacter _owner;

	private Health _health;

	[SerializeField]
	private SharedFloat _amountOfDamage;

	private float _amountOfDamageValue;

	private double _tookDamage;

	public override void OnAwake()
	{
		_health = ((SharedVariable<Character>)_owner).Value.health;
	}

	public override void OnStart()
	{
		_tookDamage = 0.0;
		_health.onTookDamage += SumTookDamage;
		_amountOfDamageValue = ((SharedVariable<float>)_amountOfDamage).Value;
	}

	public override TaskStatus OnUpdate()
	{
		if (!(_tookDamage < (double)_amountOfDamageValue))
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)3;
	}

	public override void OnEnd()
	{
		_health.onTookDamage -= SumTookDamage;
	}

	private void SumTookDamage(in Damage originalDamage, in Damage tookDamage, double damageDelta)
	{
		_tookDamage += damageDelta;
	}
}
