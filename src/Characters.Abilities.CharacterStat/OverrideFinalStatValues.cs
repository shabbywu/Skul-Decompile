using System;
using UnityEngine;

namespace Characters.Abilities.CharacterStat;

[Serializable]
public sealed class OverrideFinalStatValues : Ability
{
	public sealed class Instance : AbilityInstance<OverrideFinalStatValues>
	{
		public Instance(Character owner, OverrideFinalStatValues ability)
			: base(owner, ability)
		{
		}

		protected override void OnAttach()
		{
			owner.stat.onUpdated.Add(ability._priority, (Stat.OnUpdatedDelegate)HandleOnStatUpdated);
		}

		private double[] HandleOnStatUpdated(double[,] values)
		{
			double[] array = new double[Stat.Kind.values.Count];
			Stat.Value[] values2 = ((ReorderableArray<Stat.Value>)ability._statValues).values;
			for (int i = 0; i < Stat.Kind.values.Count; i++)
			{
				array[i] = values[Stat.Category.Final.index, i];
			}
			for (int j = 0; j < values2.Length; j++)
			{
				array[values2[j].kindIndex] = values2[j].value;
			}
			return array;
		}

		protected override void OnDetach()
		{
			owner.stat.onUpdated.Remove((Stat.OnUpdatedDelegate)HandleOnStatUpdated);
		}
	}

	[SerializeField]
	[Header("최종 고정 스텟값, Category는 사용되지 않음")]
	private Stat.Values _statValues;

	[SerializeField]
	private int _priority;

	public override IAbilityInstance CreateInstance(Character owner)
	{
		return new Instance(owner, this);
	}
}
