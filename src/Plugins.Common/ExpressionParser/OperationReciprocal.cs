namespace ExpressionParser;

public class OperationReciprocal : IValue
{
	private IValue m_Value;

	public double Value => 1.0 / m_Value.Value;

	public OperationReciprocal(IValue aValue)
	{
		m_Value = aValue;
	}

	public override string ToString()
	{
		return "( 1/" + m_Value?.ToString() + " )";
	}
}
