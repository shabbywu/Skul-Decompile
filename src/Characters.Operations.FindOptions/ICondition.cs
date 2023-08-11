namespace Characters.Operations.FindOptions;

public interface ICondition
{
	bool Satisfied(Character character);
}
