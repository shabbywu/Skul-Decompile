using System.Collections.Generic;
using System.Linq;

namespace ExpressionParser;

public class OperationSum : IValue
{
	private IValue[] m_Values;

	public double Value => m_Values.Select((IValue v) => v.Value).Sum();

	public OperationSum(params IValue[] aValues)
	{
		List<IValue> list = new List<IValue>(aValues.Length);
		foreach (IValue value in aValues)
		{
			if (!(value is OperationSum operationSum))
			{
				list.Add(value);
			}
			else
			{
				list.AddRange(operationSum.m_Values);
			}
		}
		m_Values = list.ToArray();
	}

	public override string ToString()
	{
		return "( " + string.Join(" + ", m_Values.Select((IValue v) => v.ToString()).ToArray()) + " )";
	}
}
