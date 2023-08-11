using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace BehaviorDesigner.Runtime.Tasks;

[TaskDescription("타겟 리스트에 있는 적들 중 힐이 필요한 적을 추려냅니다.리스트가 비게 되면 Fail을 반환합니다.조건에 맞지 않는 캐릭터는 리스트에서 제외됩니다.")]
public sealed class CanHeal : Conditional
{
	public enum Operation
	{
		LessThan,
		LessThanOrEqualTo,
		EqualTo,
		NotEqualTo,
		GreaterThanOrEqualTo,
		GreaterThan
	}

	public enum HealthType
	{
		Percent,
		Constant
	}

	[SerializeField]
	private SharedCharacterList _targetsList;

	[SerializeField]
	public Operation operation;

	[SerializeField]
	private HealthType _healthType;

	[SerializeField]
	[Tooltip("조건에 맞지 않는 캐릭터는 리스트에서 제외됩니다.")]
	private SharedFloat _healHealth;

	public override TaskStatus OnUpdate()
	{
		List<Character> value = ((SharedVariable<List<Character>>)_targetsList).Value;
		float num = ((SharedVariable<float>)_healHealth).Value;
		for (int num2 = value.Count - 1; num2 >= 0; num2--)
		{
			if (!((Component)value[num2]).gameObject.activeSelf)
			{
				value.Remove(value[num2]);
			}
			else if (value[num2].health.dead)
			{
				value.Remove(value[num2]);
			}
			else
			{
				float num3 = ((_healthType == HealthType.Constant) ? ((float)value[num2].health.currentHealth) : ((float)value[num2].health.percent * 100f));
				switch (operation)
				{
				case Operation.LessThan:
					if (num3 >= num)
					{
						value.Remove(value[num2]);
					}
					break;
				case Operation.LessThanOrEqualTo:
					if (num3 > num)
					{
						value.Remove(value[num2]);
					}
					break;
				case Operation.EqualTo:
					if (!Mathf.Approximately(num3, num))
					{
						value.Remove(value[num2]);
					}
					break;
				case Operation.NotEqualTo:
					if (Mathf.Approximately(num3, num))
					{
						value.Remove(value[num2]);
					}
					break;
				case Operation.GreaterThanOrEqualTo:
					if (num3 < num)
					{
						value.Remove(value[num2]);
					}
					break;
				case Operation.GreaterThan:
					if (num3 <= num)
					{
						value.Remove(value[num2]);
					}
					break;
				}
			}
		}
		if (value.Count != 0)
		{
			return (TaskStatus)2;
		}
		return (TaskStatus)1;
	}
}
