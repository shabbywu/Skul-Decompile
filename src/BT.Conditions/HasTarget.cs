using Characters;
using UnityEngine;

namespace BT.Conditions;

public sealed class HasTarget : Condition
{
	protected override bool Check(Context context)
	{
		return (Object)(object)context.Get<Character>(Key.Target) != (Object)null;
	}
}
