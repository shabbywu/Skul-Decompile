using System.Linq;

namespace ExpressionParser;

public class MultiParameterList : IValue
{
	private IValue[] m_Values;

	public IValue[] Parameters => m_Values;

	public double Value => m_Values.Select((IValue v) => v.Value).FirstOrDefault();

	public MultiParameterList(params IValue[] aValues)
	{
		m_Values = aValues;
	}

	public override string ToString()
	{
		return string.Join(", ", m_Values.Select((IValue v) => v.ToString()).ToArray());
	}
}
