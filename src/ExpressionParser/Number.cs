namespace ExpressionParser;

public class Number : IValue
{
	private double m_Value;

	public double Value
	{
		get
		{
			return m_Value;
		}
		set
		{
			m_Value = value;
		}
	}

	public Number(double aValue)
	{
		m_Value = aValue;
	}

	public override string ToString()
	{
		return m_Value.ToString() ?? "";
	}
}
