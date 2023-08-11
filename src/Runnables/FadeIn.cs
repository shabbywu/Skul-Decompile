using System.Collections;
using Services;
using Singletons;

namespace Runnables;

public class FadeIn : CRunnable
{
	public override IEnumerator CRun()
	{
		yield return Singleton<Service>.Instance.fadeInOut.CFadeIn();
	}
}
