using System;
using System.Collections.Generic;
using System.IO;

public class CsvReader
{
	internal sealed class Field
	{
		internal int Start;

		internal int End;

		internal bool Quoted;

		internal int EscapedQuotesCount;

		private string cachedValue;

		internal int Length => End - Start + 1;

		internal Field()
		{
		}

		internal Field Reset(int start)
		{
			Start = start;
			End = start - 1;
			Quoted = false;
			EscapedQuotesCount = 0;
			cachedValue = null;
			return this;
		}

		internal string GetValue(char[] buf)
		{
			if (cachedValue == null)
			{
				cachedValue = GetValueInternal(buf);
			}
			return cachedValue;
		}

		private string GetValueInternal(char[] buf)
		{
			if (Quoted)
			{
				int start = Start + 1;
				int num = Length - 2;
				string text = ((num > 0) ? GetString(buf, start, num) : string.Empty);
				if (EscapedQuotesCount > 0)
				{
					text = text.Replace("\"\"", "\"");
				}
				return text;
			}
			int length = Length;
			if (length <= 0)
			{
				return string.Empty;
			}
			return GetString(buf, Start, length);
		}

		private string GetString(char[] buf, int start, int len)
		{
			int num = buf.Length;
			start = ((start < num) ? start : (start % num));
			if (start + len - 1 >= num)
			{
				int num2 = buf.Length - start;
				string text = new string(buf, start, num2);
				string text2 = new string(buf, 0, len - num2);
				return text + text2;
			}
			return new string(buf, start, len);
		}
	}

	private int delimLength;

	private TextReader rdr;

	private char[] buffer;

	private int bufferLength;

	private int bufferLoadThreshold;

	private int lineStartPos;

	private int actualBufferLen;

	private List<Field> fields;

	private int fieldsCount;

	private int linesRead;

	public string Delimiter { get; private set; }

	public int BufferSize { get; set; } = 32768;


	public bool TrimFields { get; set; } = true;


	public int FieldsCount => fieldsCount;

	public string this[int idx]
	{
		get
		{
			if (idx < fieldsCount)
			{
				_ = fields[idx];
				return fields[idx].GetValue(buffer);
			}
			return null;
		}
	}

	public CsvReader(TextReader rdr)
		: this(rdr, ",")
	{
	}

	public CsvReader(TextReader rdr, string delimiter)
	{
		this.rdr = rdr;
		Delimiter = delimiter;
		delimLength = delimiter.Length;
		if (delimLength == 0)
		{
			throw new ArgumentException("Delimiter cannot be empty.");
		}
	}

	private int ReadBlockAndCheckEof(char[] buffer, int start, int len, ref bool eof)
	{
		if (len == 0)
		{
			return 0;
		}
		int num = rdr.ReadBlock(buffer, start, len);
		if (num < len)
		{
			eof = true;
		}
		return num;
	}

	private bool FillBuffer()
	{
		bool eof = false;
		int num = bufferLength - actualBufferLen;
		if (num >= bufferLoadThreshold)
		{
			int num2 = (lineStartPos + actualBufferLen) % buffer.Length;
			if (num2 >= lineStartPos)
			{
				actualBufferLen += ReadBlockAndCheckEof(buffer, num2, buffer.Length - num2, ref eof);
				if (lineStartPos > 0)
				{
					actualBufferLen += ReadBlockAndCheckEof(buffer, 0, lineStartPos, ref eof);
				}
			}
			else
			{
				actualBufferLen += ReadBlockAndCheckEof(buffer, num2, num, ref eof);
			}
		}
		return eof;
	}

	private string GetLineTooLongMsg()
	{
		return string.Format("CSV line #{1} length exceedes buffer size ({0})", BufferSize, linesRead);
	}

	private int ReadQuotedFieldToEnd(int start, int maxPos, bool eof, ref int escapedQuotesCount)
	{
		int i;
		for (i = start; i < maxPos; i++)
		{
			int num = ((i < bufferLength) ? i : (i % bufferLength));
			if (buffer[num] == '"')
			{
				if (i + 1 >= maxPos || buffer[(i + 1) % bufferLength] != '"')
				{
					return i;
				}
				i++;
				escapedQuotesCount++;
			}
		}
		if (eof)
		{
			return i - 1;
		}
		throw new InvalidDataException(GetLineTooLongMsg());
	}

	private bool ReadDelimTail(int start, int maxPos, ref int end)
	{
		int i;
		for (i = 1; i < delimLength; i++)
		{
			int num = start + i;
			int num2 = ((num < bufferLength) ? num : (num % bufferLength));
			if (num >= maxPos || buffer[num2] != Delimiter[i])
			{
				return false;
			}
		}
		end = start + i - 1;
		return true;
	}

	private Field GetOrAddField(int startIdx)
	{
		fieldsCount++;
		while (fieldsCount > fields.Count)
		{
			fields.Add(new Field());
		}
		Field field = fields[fieldsCount - 1];
		field.Reset(startIdx);
		return field;
	}

	public int GetValueLength(int idx)
	{
		if (idx < fieldsCount)
		{
			Field field = fields[idx];
			if (!field.Quoted)
			{
				return field.Length;
			}
			return field.Length - field.EscapedQuotesCount;
		}
		return -1;
	}

	public void ProcessValueInBuffer(int idx, Action<char[], int, int> handler)
	{
		if (idx < fieldsCount)
		{
			Field field = fields[idx];
			if ((field.Quoted && field.EscapedQuotesCount > 0) || field.End >= bufferLength)
			{
				char[] array = field.GetValue(buffer).ToCharArray();
				handler(array, 0, array.Length);
			}
			else if (field.Quoted)
			{
				handler(buffer, field.Start + 1, field.Length - 2);
			}
			else
			{
				handler(buffer, field.Start, field.Length);
			}
		}
	}

	public bool Read()
	{
		if (fields == null)
		{
			fields = new List<Field>();
			fieldsCount = 0;
		}
		if (buffer == null)
		{
			bufferLoadThreshold = Math.Min(BufferSize, 8192);
			bufferLength = BufferSize + bufferLoadThreshold;
			buffer = new char[bufferLength];
			lineStartPos = 0;
			actualBufferLen = 0;
		}
		bool flag = FillBuffer();
		fieldsCount = 0;
		if (actualBufferLen <= 0)
		{
			return false;
		}
		linesRead++;
		int num = lineStartPos + actualBufferLen;
		int end = lineStartPos;
		Field orAddField = GetOrAddField(end);
		bool flag2 = false;
		char c = Delimiter[0];
		bool trimFields = TrimFields;
		while (true)
		{
			if (end < num)
			{
				int num2 = ((end < bufferLength) ? end : (end % bufferLength));
				char c2 = buffer[num2];
				switch (c2)
				{
				case '"':
					if (flag2)
					{
						orAddField.End = end;
					}
					else if (orAddField.Quoted || orAddField.Length > 0)
					{
						orAddField.End = end;
						orAddField.Quoted = false;
						flag2 = true;
					}
					else
					{
						int num3 = ReadQuotedFieldToEnd(end + 1, num, flag, ref orAddField.EscapedQuotesCount);
						orAddField.Start = end;
						orAddField.End = num3;
						orAddField.Quoted = true;
						end = num3;
					}
					goto IL_01f5;
				case '\r':
					if (end + 1 < num && buffer[(end + 1) % bufferLength] == '\n')
					{
						end++;
					}
					end++;
					break;
				case '\n':
					end++;
					break;
				default:
					if (c2 == c && (delimLength == 1 || ReadDelimTail(end, num, ref end)))
					{
						orAddField = GetOrAddField(end + 1);
						flag2 = false;
					}
					else if (!(c2 == ' ' && trimFields))
					{
						if (orAddField.Length == 0)
						{
							orAddField.Start = end;
						}
						if (orAddField.Quoted)
						{
							orAddField.Quoted = false;
							flag2 = true;
						}
						orAddField.End = end;
					}
					goto IL_01f5;
				}
				break;
			}
			if (flag)
			{
				break;
			}
			throw new InvalidDataException(GetLineTooLongMsg());
			IL_01f5:
			end++;
		}
		actualBufferLen -= end - lineStartPos;
		lineStartPos = end % bufferLength;
		if (fieldsCount == 1 && fields[0].Length == 0)
		{
			return Read();
		}
		return true;
	}
}
