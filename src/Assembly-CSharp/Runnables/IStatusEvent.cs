using Characters;

namespace Runnables;

public interface IStatusEvent
{
	void Apply(Character owner, Character target);

	void Release(Character owner, Character target);
}
