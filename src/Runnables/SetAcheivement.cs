using Platforms;
using UnityEngine;

namespace Runnables;

public sealed class SetAcheivement : Runnable
{
	[SerializeField]
	private Type _type;

	public override void Run()
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		ExtensionMethods.Set(_type);
	}
}
