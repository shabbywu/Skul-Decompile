namespace ExpressionParser;

public class OperationNegate : IValue
{
	private IValue m_Value;

	public double Value => 0.0 - m_Value.Value;

	public OperationNegate(IValue aValue)
	{
		m_Value = aValue;
	}

	public override string ToString()
	{
		return "( -" + m_Value?.ToString() + " )";
	}
}
