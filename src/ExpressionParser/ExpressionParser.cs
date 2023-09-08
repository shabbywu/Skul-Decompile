using System;
using System.Collections.Generic;
using System.Linq;

namespace ExpressionParser;

public class ExpressionParser
{
	public class ParseException : Exception
	{
		public ParseException(string aMessage)
			: base(aMessage)
		{
		}
	}

	private List<string> m_BracketHeap = new List<string>();

	private Dictionary<string, Func<double>> m_Consts = new Dictionary<string, Func<double>>();

	private Dictionary<string, Func<double[], double>> m_Funcs = new Dictionary<string, Func<double[], double>>();

	private Expression m_Context;

	public ExpressionParser()
	{
		Random rnd = new Random();
		m_Consts.Add("PI", () => Math.PI);
		m_Consts.Add("e", () => Math.E);
		m_Funcs.Add("sqrt", (double[] p) => Math.Sqrt(p.FirstOrDefault()));
		m_Funcs.Add("abs", (double[] p) => Math.Abs(p.FirstOrDefault()));
		m_Funcs.Add("ln", (double[] p) => Math.Log(p.FirstOrDefault()));
		m_Funcs.Add("floor", (double[] p) => Math.Floor(p.FirstOrDefault()));
		m_Funcs.Add("ceiling", (double[] p) => Math.Ceiling(p.FirstOrDefault()));
		m_Funcs.Add("round", (double[] p) => Math.Round(p.FirstOrDefault()));
		m_Funcs.Add("sin", (double[] p) => Math.Sin(p.FirstOrDefault()));
		m_Funcs.Add("cos", (double[] p) => Math.Cos(p.FirstOrDefault()));
		m_Funcs.Add("tan", (double[] p) => Math.Tan(p.FirstOrDefault()));
		m_Funcs.Add("asin", (double[] p) => Math.Asin(p.FirstOrDefault()));
		m_Funcs.Add("acos", (double[] p) => Math.Acos(p.FirstOrDefault()));
		m_Funcs.Add("atan", (double[] p) => Math.Atan(p.FirstOrDefault()));
		m_Funcs.Add("atan2", (double[] p) => Math.Atan2(p.FirstOrDefault(), p.ElementAtOrDefault(1)));
		m_Funcs.Add("min", (double[] p) => Math.Min(p.FirstOrDefault(), p.ElementAtOrDefault(1)));
		m_Funcs.Add("max", (double[] p) => Math.Max(p.FirstOrDefault(), p.ElementAtOrDefault(1)));
		m_Funcs.Add("rnd", delegate(double[] p)
		{
			if (p.Length == 2)
			{
				return p[0] + rnd.NextDouble() * (p[1] - p[0]);
			}
			return (p.Length == 1) ? (rnd.NextDouble() * p[0]) : rnd.NextDouble();
		});
	}

	public void AddFunc(string aName, Func<double[], double> aMethod)
	{
		if (m_Funcs.ContainsKey(aName))
		{
			m_Funcs[aName] = aMethod;
		}
		else
		{
			m_Funcs.Add(aName, aMethod);
		}
	}

	public void AddConst(string aName, Func<double> aMethod)
	{
		if (m_Consts.ContainsKey(aName))
		{
			m_Consts[aName] = aMethod;
		}
		else
		{
			m_Consts.Add(aName, aMethod);
		}
	}

	public void RemoveFunc(string aName)
	{
		if (m_Funcs.ContainsKey(aName))
		{
			m_Funcs.Remove(aName);
		}
	}

	public void RemoveConst(string aName)
	{
		if (m_Consts.ContainsKey(aName))
		{
			m_Consts.Remove(aName);
		}
	}

	private int FindClosingBracket(ref string aText, int aStart, char aOpen, char aClose)
	{
		int num = 0;
		for (int i = aStart; i < aText.Length; i++)
		{
			if (aText[i] == aOpen)
			{
				num++;
			}
			if (aText[i] == aClose)
			{
				num--;
			}
			if (num == 0)
			{
				return i;
			}
		}
		return -1;
	}

	private void SubstitudeBracket(ref string aExpression, int aIndex)
	{
		int num = FindClosingBracket(ref aExpression, aIndex, '(', ')');
		if (num > aIndex + 1)
		{
			string item = aExpression.Substring(aIndex + 1, num - aIndex - 1);
			m_BracketHeap.Add(item);
			string text = "&" + (m_BracketHeap.Count - 1) + ";";
			aExpression = aExpression.Substring(0, aIndex) + text + aExpression.Substring(num + 1);
			return;
		}
		throw new ParseException("Bracket not closed!");
	}

	private IValue Parse(string aExpression)
	{
		aExpression = aExpression.Trim();
		for (int num = aExpression.IndexOf('('); num >= 0; num = aExpression.IndexOf('('))
		{
			SubstitudeBracket(ref aExpression, num);
		}
		if (Enumerable.Contains(aExpression, ','))
		{
			string[] array = aExpression.Split(new char[1] { ',' });
			List<IValue> list = new List<IValue>(array.Length);
			for (int i = 0; i < array.Length; i++)
			{
				string text = array[i].Trim();
				if (!string.IsNullOrEmpty(text))
				{
					list.Add(Parse(text));
				}
			}
			return new MultiParameterList(list.ToArray());
		}
		if (Enumerable.Contains(aExpression, '+'))
		{
			string[] array2 = aExpression.Split(new char[1] { '+' });
			List<IValue> list2 = new List<IValue>(array2.Length);
			for (int j = 0; j < array2.Length; j++)
			{
				string text2 = array2[j].Trim();
				if (!string.IsNullOrEmpty(text2))
				{
					list2.Add(Parse(text2));
				}
			}
			if (list2.Count == 1)
			{
				return list2[0];
			}
			return new OperationSum(list2.ToArray());
		}
		if (Enumerable.Contains(aExpression, '-'))
		{
			string[] array3 = aExpression.Split(new char[1] { '-' });
			List<IValue> list3 = new List<IValue>(array3.Length);
			if (!string.IsNullOrEmpty(array3[0].Trim()))
			{
				list3.Add(Parse(array3[0]));
			}
			for (int k = 1; k < array3.Length; k++)
			{
				string text3 = array3[k].Trim();
				if (!string.IsNullOrEmpty(text3))
				{
					list3.Add(new OperationNegate(Parse(text3)));
				}
			}
			if (list3.Count == 1)
			{
				return list3[0];
			}
			return new OperationSum(list3.ToArray());
		}
		if (Enumerable.Contains(aExpression, '*'))
		{
			string[] array4 = aExpression.Split(new char[1] { '*' });
			List<IValue> list4 = new List<IValue>(array4.Length);
			for (int l = 0; l < array4.Length; l++)
			{
				list4.Add(Parse(array4[l]));
			}
			if (list4.Count == 1)
			{
				return list4[0];
			}
			return new OperationProduct(list4.ToArray());
		}
		if (Enumerable.Contains(aExpression, '/'))
		{
			string[] array5 = aExpression.Split(new char[1] { '/' });
			List<IValue> list5 = new List<IValue>(array5.Length);
			if (!string.IsNullOrEmpty(array5[0].Trim()))
			{
				list5.Add(Parse(array5[0]));
			}
			for (int m = 1; m < array5.Length; m++)
			{
				string text4 = array5[m].Trim();
				if (!string.IsNullOrEmpty(text4))
				{
					list5.Add(new OperationReciprocal(Parse(text4)));
				}
			}
			return new OperationProduct(list5.ToArray());
		}
		if (Enumerable.Contains(aExpression, '^'))
		{
			int num2 = aExpression.IndexOf('^');
			IValue aValue = Parse(aExpression.Substring(0, num2));
			IValue aPower = Parse(aExpression.Substring(num2 + 1));
			return new OperationPower(aValue, aPower);
		}
		int num3 = aExpression.IndexOf("&");
		if (num3 > 0)
		{
			string text5 = aExpression.Substring(0, num3);
			foreach (KeyValuePair<string, Func<double[], double>> func in m_Funcs)
			{
				if (text5 == func.Key)
				{
					string aExpression2 = aExpression.Substring(func.Key.Length);
					IValue value = Parse(aExpression2);
					return new CustomFunction(aValues: (!(value is MultiParameterList multiParameterList)) ? new IValue[1] { value } : multiParameterList.Parameters, aName: func.Key, aDelegate: func.Value);
				}
			}
		}
		foreach (KeyValuePair<string, Func<double>> C in m_Consts)
		{
			if (aExpression == C.Key)
			{
				return new CustomFunction(C.Key, (Func<double[], double>)((double[] p) => C.Value()), (IValue[])null);
			}
		}
		int num4 = aExpression.IndexOf('&');
		int num5 = aExpression.IndexOf(';');
		if (num4 >= 0 && num5 >= 2)
		{
			if (int.TryParse(aExpression.Substring(num4 + 1, num5 - num4 - 1), out var result) && result >= 0 && result < m_BracketHeap.Count)
			{
				return Parse(m_BracketHeap[result]);
			}
			throw new ParseException("Can't parse substitude token");
		}
		if (double.TryParse(aExpression, out var result2))
		{
			return new Number(result2);
		}
		if (ValidIdentifier(aExpression))
		{
			if (m_Context.Parameters.ContainsKey(aExpression))
			{
				return m_Context.Parameters[aExpression];
			}
			Parameter parameter = new Parameter(aExpression);
			m_Context.Parameters.Add(aExpression, parameter);
			return parameter;
		}
		throw new ParseException("Reached unexpected end within the parsing tree");
	}

	private bool ValidIdentifier(string aExpression)
	{
		aExpression = aExpression.Trim();
		if (string.IsNullOrEmpty(aExpression))
		{
			return false;
		}
		if (aExpression.Length < 1)
		{
			return false;
		}
		if (aExpression.Contains(" "))
		{
			return false;
		}
		if (!Enumerable.Contains("abcdefghijklmnopqrstuvwxyz§$", char.ToLower(aExpression[0])))
		{
			return false;
		}
		if (m_Consts.ContainsKey(aExpression))
		{
			return false;
		}
		if (m_Funcs.ContainsKey(aExpression))
		{
			return false;
		}
		return true;
	}

	public Expression EvaluateExpression(string aExpression)
	{
		Expression expression = (m_Context = new Expression());
		expression.ExpressionTree = Parse(aExpression);
		m_Context = null;
		m_BracketHeap.Clear();
		return expression;
	}

	public double Evaluate(string aExpression)
	{
		return EvaluateExpression(aExpression).Value;
	}

	public static double Eval(string aExpression)
	{
		return new ExpressionParser().Evaluate(aExpression);
	}
}
