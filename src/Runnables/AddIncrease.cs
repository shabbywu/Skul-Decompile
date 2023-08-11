using Level.Specials;
using UnityEngine;

namespace Runnables;

public abstract class AddIncrease : Runnable
{
	[SerializeField]
	private TimeCostEvent _costReward;

	public override void Run()
	{
		_costReward.AddSpeed(GetIncrease());
	}

	protected abstract int GetIncrease();
}
