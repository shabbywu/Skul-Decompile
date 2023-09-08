using System;

namespace ExpressionParser;

public class OperationPower : IValue
{
	private IValue m_Value;

	private IValue m_Power;

	public double Value => Math.Pow(m_Value.Value, m_Power.Value);

	public OperationPower(IValue aValue, IValue aPower)
	{
		m_Value = aValue;
		m_Power = aPower;
	}

	public override string ToString()
	{
		return "( " + m_Value?.ToString() + "^" + m_Power?.ToString() + " )";
	}
}
