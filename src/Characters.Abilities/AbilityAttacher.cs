using System;
using Characters.Abilities.Customs;
using UnityEditor;
using UnityEngine;

namespace Characters.Abilities;

public abstract class AbilityAttacher : MonoBehaviour
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public static readonly Type[] types;

		public static readonly string[] names;

		static SubcomponentAttribute()
		{
			types = new Type[26]
			{
				typeof(AlwaysAbilityAttacher),
				typeof(TriggerAbilityAttacher),
				typeof(RandomTriggerAbilityAttacher),
				typeof(DuringCombatAbilityAttacher),
				typeof(DuringChargingAbilityAttacher),
				typeof(DuringRunningActionAbilityAttacher),
				typeof(HealthAttacher),
				typeof(ShieldAttacher),
				typeof(EnemyWithinRangeAttacher),
				typeof(EffectZoneAbilityAttacher),
				typeof(AirAndGroundAbility),
				typeof(InMapAbilityAttacher),
				typeof(OwnItemAbilityAttacher),
				typeof(CurrencyAbilityAttacher),
				typeof(FirstAttackAbilityAttacher),
				null,
				typeof(HeadCategoryAttacher),
				typeof(CurrentHeadCategoryAttacher),
				typeof(CurrentHeadTagAttacher),
				null,
				typeof(MaxStatStackAbilityAttacher),
				typeof(CharacterAliveAbilityAttacher),
				typeof(GhoulConsumeStatAttacher),
				typeof(LivingArmor2PassiveAttacher),
				typeof(EntColliderAbilityAttacher),
				typeof(OnUseReassembleAttacher)
			};
			int length = typeof(AbilityAttacher).Namespace.Length;
			names = new string[types.Length];
			for (int i = 0; i < names.Length; i++)
			{
				Type type = types[i];
				if (type == null)
				{
					string text = names[i - 1];
					int num = text.LastIndexOf('/');
					if (num == -1)
					{
						names[i] = string.Empty;
					}
					else
					{
						names[i] = text.Substring(0, num + 1);
					}
				}
				else
				{
					names[i] = type.FullName.Substring(length + 1, type.FullName.Length - length - 1).Replace('.', '/');
				}
			}
		}

		public SubcomponentAttribute()
			: base(true, types, names)
		{
		}
	}

	[Serializable]
	internal class Subcomponents : SubcomponentArray<AbilityAttacher>
	{
		public void Initialize(Character character)
		{
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].Initialize(character);
			}
		}

		public void StartAttach()
		{
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].StartAttach();
			}
		}

		public void StopAttach()
		{
			for (int i = 0; i < base._components.Length; i++)
			{
				base._components[i].StopAttach();
			}
		}
	}

	public Character owner { get; private set; }

	public void Initialize(Character character)
	{
		owner = character;
		OnIntialize();
	}

	public abstract void StartAttach();

	public abstract void StopAttach();

	public abstract void OnIntialize();
}
