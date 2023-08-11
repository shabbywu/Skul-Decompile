using Characters;

namespace Runnables;

public interface IHitEvent
{
	void OnHit(in Damage originalDamage, in Damage tookDamage, double damageDealt);
}
