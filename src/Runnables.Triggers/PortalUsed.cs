using Services;
using Singletons;

namespace Runnables.Triggers;

public class PortalUsed : Trigger
{
	protected override bool Check()
	{
		return Singleton<Service>.Instance.levelManager.skulPortalUsed;
	}
}
