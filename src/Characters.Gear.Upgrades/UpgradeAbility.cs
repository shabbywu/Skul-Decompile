using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Gear.Upgrades;

public abstract class UpgradeAbility : MonoBehaviour
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	[Serializable]
	public class Subcomponents : SubcomponentArray<UpgradeAbility>
	{
		public UpgradeAbility this[int i] => base.components[i];

		public void Attach(Character target)
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				base.components[i].Attach(target);
			}
		}

		public void DetachAll()
		{
			for (int i = 0; i < base.components.Length; i++)
			{
				base.components[i].Detach();
			}
		}
	}

	public static Type[] types = new Type[7]
	{
		typeof(AttachAbility),
		typeof(AttachSavableAbility),
		typeof(RunOperations),
		typeof(NegotiatorsCoin),
		typeof(AssetManagement),
		typeof(AdamantiumSkeleton),
		typeof(RebornRecovery)
	};

	public abstract void Attach(Character target);

	public abstract void Detach();
}
