using System;
using UnityEditor;
using UnityEngine;

namespace Characters.Operations.SetPosition;

public abstract class Policy : MonoBehaviour
{
	[AttributeUsage(AttributeTargets.Field)]
	public class SubcomponentAttribute : UnityEditor.SubcomponentAttribute
	{
		public new static readonly Type[] types = new Type[27]
		{
			typeof(ToBTTarget),
			typeof(ToBDTarget),
			typeof(ToBDTargetOpposition),
			typeof(ToBDKeepDistance),
			typeof(ToKeepDistance),
			typeof(ToBDTransform),
			typeof(ToBDTargetPlatformPoint),
			typeof(ToBounds),
			typeof(ToClosestTarget),
			typeof(ToCharacterBased),
			typeof(ToLookingSide),
			typeof(ToObject),
			typeof(ToOppositionPlatform),
			typeof(ToPlatformPoint),
			typeof(ToPlayer),
			typeof(ToPlayerBased),
			typeof(ToRandomPoint),
			typeof(ToRayPoint),
			typeof(ToTargetOpposition),
			typeof(ToSavedPosition),
			typeof(ToRandomEnemyBased),
			typeof(ToRandomTarget),
			typeof(ToColliderBased),
			typeof(ToCirclePoint),
			typeof(ToOwner),
			typeof(ToLinear),
			typeof(ToCenterPoints)
		};

		public SubcomponentAttribute(bool allowCustom = true)
			: base(allowCustom, types)
		{
		}
	}

	public abstract Vector2 GetPosition();

	public abstract Vector2 GetPosition(Character owner);
}
