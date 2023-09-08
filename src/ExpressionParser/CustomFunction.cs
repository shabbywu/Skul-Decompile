using System;
using System.Linq;

namespace ExpressionParser;

public class CustomFunction : IValue
{
	private IValue[] m_Params;

	private Func<double[], double> m_Delegate;

	private string m_Name;

	public double Value
	{
		get
		{
			if (m_Params == null)
			{
				return m_Delegate(null);
			}
			return m_Delegate(m_Params.Select((IValue p) => p.Value).ToArray());
		}
	}

	public CustomFunction(string aName, Func<double[], double> aDelegate, params IValue[] aValues)
	{
		m_Delegate = aDelegate;
		m_Params = aValues;
		m_Name = aName;
	}

	public override string ToString()
	{
		if (m_Params == null)
		{
			return m_Name;
		}
		return m_Name + "( " + string.Join(", ", m_Params.Select((IValue v) => v.ToString()).ToArray()) + " )";
	}
}
