using System;
using System.Collections.Generic;

namespace ExpressionParser;

public class Expression : IValue
{
	public class ParameterException : Exception
	{
		public ParameterException(string aMessage)
			: base(aMessage)
		{
		}
	}

	public IDictionary<string, Parameter> Parameters = new Dictionary<string, Parameter>();

	public IValue ExpressionTree { get; set; }

	public double Value => ExpressionTree.Value;

	public double[] MultiValue
	{
		get
		{
			if (ExpressionTree is MultiParameterList multiParameterList)
			{
				double[] array = new double[multiParameterList.Parameters.Length];
				for (int i = 0; i < array.Length; i++)
				{
					array[i] = multiParameterList.Parameters[i].Value;
				}
				return array;
			}
			return null;
		}
	}

	public override string ToString()
	{
		return ExpressionTree.ToString();
	}

	public ExpressionDelegate ToDelegate(params string[] aParamOrder)
	{
		List<Parameter> list = new List<Parameter>(aParamOrder.Length);
		for (int i = 0; i < aParamOrder.Length; i++)
		{
			if (Parameters.ContainsKey(aParamOrder[i]))
			{
				list.Add(Parameters[aParamOrder[i]]);
			}
			else
			{
				list.Add(null);
			}
		}
		Parameter[] parameters2 = list.ToArray();
		return (double[] p) => Invoke(p, parameters2);
	}

	public MultiResultDelegate ToMultiResultDelegate(params string[] aParamOrder)
	{
		List<Parameter> list = new List<Parameter>(aParamOrder.Length);
		for (int i = 0; i < aParamOrder.Length; i++)
		{
			if (Parameters.ContainsKey(aParamOrder[i]))
			{
				list.Add(Parameters[aParamOrder[i]]);
			}
			else
			{
				list.Add(null);
			}
		}
		Parameter[] parameters2 = list.ToArray();
		return (double[] p) => InvokeMultiResult(p, parameters2);
	}

	private double Invoke(double[] aParams, Parameter[] aParamList)
	{
		int num = Math.Min(aParamList.Length, aParams.Length);
		for (int i = 0; i < num; i++)
		{
			if (aParamList[i] != null)
			{
				aParamList[i].Value = aParams[i];
			}
		}
		return Value;
	}

	private double[] InvokeMultiResult(double[] aParams, Parameter[] aParamList)
	{
		int num = Math.Min(aParamList.Length, aParams.Length);
		for (int i = 0; i < num; i++)
		{
			if (aParamList[i] != null)
			{
				aParamList[i].Value = aParams[i];
			}
		}
		return MultiValue;
	}

	public static Expression Parse(string aExpression)
	{
		return new ExpressionParser().EvaluateExpression(aExpression);
	}
}
