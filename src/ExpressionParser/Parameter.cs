namespace ExpressionParser;

public class Parameter : Number
{
	public string Name { get; private set; }

	public override string ToString()
	{
		return Name + "[" + base.ToString() + "]";
	}

	public Parameter(string aName)
		: base(0.0)
	{
		Name = aName;
	}
}
