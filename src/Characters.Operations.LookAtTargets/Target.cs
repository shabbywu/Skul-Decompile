using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.LookAtTargets;

public abstract class Target : MonoBehaviour
{
	public class SubcomponentAttribute : SubcomponentAttribute
	{
		public SubcomponentAttribute()
			: base(true, types)
		{
		}
	}

	public static readonly Type[] types = new Type[12]
	{
		typeof(AITarget),
		typeof(BTTarget),
		typeof(BDTarget),
		typeof(ClosestSideOnPlatform),
		typeof(ClosestSideFromPlayer),
		typeof(FlipInDistanceFromPlatform),
		typeof(Chance),
		typeof(Inverter),
		typeof(Player),
		typeof(PlatformPoint),
		typeof(TargetObject),
		typeof(TurnAround)
	};

	public abstract Character.LookingDirection GetDirectionFrom(Character character);
}
