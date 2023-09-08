namespace Characters.Abilities;

public interface IStackResolver
{
	void Initialize();

	void Attach(Character owner);

	void Detach(Character owner);

	int GetStack(ref Damage damage);

	void UpdateTime(float deltaTime);
}
