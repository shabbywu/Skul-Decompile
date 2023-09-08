using UnityEngine;

namespace Characters.Actions.Cooldowns;

public class Infinity : Cooldown
{
	protected static Infinity _singleton;

	internal static Infinity singleton
	{
		get
		{
			//IL_0012: Unknown result type (might be due to invalid IL or missing references)
			//IL_0017: Unknown result type (might be due to invalid IL or missing references)
			if ((Object)(object)_singleton == (Object)null)
			{
				_singleton = new GameObject("Infinity")
				{
					hideFlags = (HideFlags)61
				}.AddComponent<Infinity>();
			}
			return _singleton;
		}
	}

	public override float remainPercent => 0f;

	public override bool canUse => true;

	internal override bool Consume()
	{
		return true;
	}
}
