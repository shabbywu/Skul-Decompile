using System.Linq;

namespace ExpressionParser;

public class OperationProduct : IValue
{
	private IValue[] m_Values;

	public double Value => m_Values.Select((IValue v) => v.Value).Aggregate((double v1, double v2) => v1 * v2);

	public OperationProduct(params IValue[] aValues)
	{
		m_Values = aValues;
	}

	public override string ToString()
	{
		return "( " + string.Join(" * ", m_Values.Select((IValue v) => v.ToString()).ToArray()) + " )";
	}
}
