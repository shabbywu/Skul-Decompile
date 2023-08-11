using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class TextMeshProAnimator : MonoBehaviour
{
	public struct TextSpeedItem
	{
		public int speed;

		public int index;
	}

	public int currentFrame;

	public bool useCustomText;

	public string customText;

	public float shakeAmount = 1f;

	public TextMeshAnimator_IndependencyType shakeIndependency = TextMeshAnimator_IndependencyType.Character;

	public float waveAmount = 1f;

	public float waveSpeed = 1f;

	public float waveSeparation = 1f;

	public TextMeshAnimator_IndependencyType waveIndependency = TextMeshAnimator_IndependencyType.Vertex;

	public float wiggleAmount = 1f;

	public float wiggleSpeed = 0.125f;

	public float wiggleMinimumDuration = 0.5f;

	public TextMeshAnimator_IndependencyType wiggleIndependency = TextMeshAnimator_IndependencyType.Character;

	private TextMeshProUGUI TMProGUI;

	private Vector3[][] vertex_Base;

	private bool[] shakesEnabled;

	private float[] shakeVelocities;

	private TextMeshAnimator_IndependencyType[] shakeIndependencies;

	private bool[] wavesEnabled;

	private float[] waveVelocities;

	private float[] waveSpeeds;

	private float[] waveSeparations;

	private TextMeshAnimator_IndependencyType[] waveIndependencies;

	private bool[] wigglesEnabled;

	private float[] wiggleVelocities;

	private float[] wiggleSpeeds;

	private float[] wigglePrevPos;

	private float[] wiggleTargetPos;

	private float[] wiggleTimeCurrent;

	private float[] wiggleTimeTotal;

	private TextMeshAnimator_IndependencyType[] wiggleIndependencies;

	[SerializeField]
	public int charsVisible;

	[SerializeField]
	public int[] scrollSpeeds;

	public string text
	{
		get
		{
			return customText;
		}
		set
		{
			customText = value;
			if (useCustomText)
			{
				if ((Object)(object)TMProGUI == (Object)null)
				{
					TMProGUI = ((Component)this).GetComponent<TextMeshProUGUI>();
				}
				((TMP_Text)TMProGUI).text = ParseText(value);
				SyncToTextMesh();
			}
		}
	}

	public int totalChars => ((TMP_Text)TMProGUI).textInfo.characterCount;

	public void SyncToTextMesh()
	{
		((TMP_Text)TMProGUI).ForceMeshUpdate(false, false);
		vertex_Base = new Vector3[((TMP_Text)TMProGUI).textInfo.meshInfo.Length][];
		int num = 0;
		for (int i = 0; i < ((TMP_Text)TMProGUI).textInfo.meshInfo.Length; i++)
		{
			vertex_Base[i] = (Vector3[])(object)new Vector3[((TMP_Text)TMProGUI).textInfo.meshInfo[i].vertices.Length];
			if (num < vertex_Base[i].Length)
			{
				num = vertex_Base[i].Length;
			}
			Array.Copy(((TMP_Text)TMProGUI).textInfo.meshInfo[i].vertices, vertex_Base[i], ((TMP_Text)TMProGUI).textInfo.meshInfo[i].vertices.Length);
		}
		wigglePrevPos = new float[num * 2];
		wiggleTargetPos = new float[num * 2];
		wiggleTimeCurrent = new float[num * 2];
		wiggleTimeTotal = new float[num * 2];
		((TMP_Text)TMProGUI).ForceMeshUpdate(false, false);
	}

	public void UpdateText(string text = null)
	{
		if ((Object)(object)TMProGUI == (Object)null)
		{
			TMProGUI = ((Component)this).gameObject.GetComponent<TextMeshProUGUI>();
		}
		if (useCustomText)
		{
			if (text == null)
			{
				this.text = this.text;
			}
			else
			{
				this.text = text;
			}
		}
		else
		{
			((TMP_Text)TMProGUI).text = ParseText(((TMP_Text)TMProGUI).text);
			SyncToTextMesh();
		}
	}

	private string ParseText(string inputText)
	{
		List<bool> list = new List<bool>();
		List<float> list2 = new List<float>();
		List<TextMeshAnimator_IndependencyType> list3 = new List<TextMeshAnimator_IndependencyType>();
		bool item = false;
		float result = 1f;
		TextMeshAnimator_IndependencyType item2 = shakeIndependency;
		List<bool> list4 = new List<bool>();
		List<float> list5 = new List<float>();
		List<float> list6 = new List<float>();
		List<float> list7 = new List<float>();
		List<TextMeshAnimator_IndependencyType> list8 = new List<TextMeshAnimator_IndependencyType>();
		bool item3 = false;
		float result2 = 1f;
		float result3 = 1f;
		float result4 = 1f;
		TextMeshAnimator_IndependencyType item4 = waveIndependency;
		List<bool> list9 = new List<bool>();
		List<float> list10 = new List<float>();
		List<float> list11 = new List<float>();
		List<TextMeshAnimator_IndependencyType> list12 = new List<TextMeshAnimator_IndependencyType>();
		bool item5 = false;
		float result5 = 1f;
		float result6 = 1f;
		TextMeshAnimator_IndependencyType item6 = wiggleIndependency;
		List<int> list13 = new List<int>();
		int item7 = 2;
		string text = "";
		for (int i = 0; i < inputText.Length; i++)
		{
			if (inputText[i] == '<')
			{
				int num = i;
				while (i < inputText.Length)
				{
					if (inputText[i++] != '>')
					{
						continue;
					}
					string text2 = inputText.Substring(num, i - num);
					if (text2.ToUpper().Contains("COLOR") || text2.ToUpper().Contains("SIZE"))
					{
						Debug.Log((object)("THE TAG IS " + text2));
						text += text2;
					}
					else if (text2.ToUpper().Contains("SPRITE"))
					{
						text += text2;
					}
					else if (text2.ToUpper().Contains("/SHAKE"))
					{
						item = false;
						result = 1f;
					}
					else if (text2.ToUpper().Contains("SHAKE"))
					{
						item = true;
						string text3 = "INTENSITY=";
						if (text2.ToUpper().Contains(text3))
						{
							int num2 = text2.ToUpper().IndexOf(text3) + text3.Length;
							int j;
							for (j = num2; j < text2.Length && (char.IsDigit(text2[j]) || text2[j] == '.'); j++)
							{
							}
							string text4 = text2.Substring(num2, j - num2);
							if (!float.TryParse(text4, out result))
							{
								Debug.LogError((object)$"'{text4}' is not a valid value for shake amount.");
							}
						}
						if (text2.ToUpper().Contains("UNITED"))
						{
							item2 = TextMeshAnimator_IndependencyType.United;
						}
						if (text2.ToUpper().Contains("WORD"))
						{
							item2 = TextMeshAnimator_IndependencyType.Word;
						}
						if (text2.ToUpper().Contains("CHARACTER"))
						{
							item2 = TextMeshAnimator_IndependencyType.Character;
						}
						if (text2.ToUpper().Contains("VERTEX"))
						{
							item2 = TextMeshAnimator_IndependencyType.Vertex;
						}
					}
					else if (text2.ToUpper().Contains("/WAVE"))
					{
						item3 = false;
						result2 = 1f;
						result3 = 1f;
						result4 = 1f;
					}
					else if (text2.ToUpper().Contains("WAVE"))
					{
						item3 = true;
						string text5 = "INTENSITY=";
						if (text2.ToUpper().Contains(text5))
						{
							int num3 = text2.ToUpper().IndexOf(text5) + text5.Length;
							int k;
							for (k = num3; k < text2.Length && (char.IsDigit(text2[k]) || text2[k] == '.'); k++)
							{
							}
							string text6 = text2.Substring(num3, k - num3);
							if (!float.TryParse(text6, out result2))
							{
								Debug.LogError((object)$"'{text6}' is not a valid value for wave amount.");
							}
						}
						string text7 = "SPEED=";
						if (text2.ToUpper().Contains(text7))
						{
							int num4 = text2.ToUpper().IndexOf(text7) + text7.Length;
							int l;
							for (l = num4; l < text2.Length && (char.IsDigit(text2[l]) || text2[l] == '.'); l++)
							{
							}
							string text8 = text2.Substring(num4, l - num4);
							if (!float.TryParse(text8, out result3))
							{
								Debug.LogError((object)$"'{text8}' is not a valid value for wave speed.");
							}
						}
						string text9 = "SEPARATION=";
						if (text2.ToUpper().Contains(text9))
						{
							int num5 = text2.ToUpper().IndexOf(text9) + text9.Length;
							int m;
							for (m = num5; m < text2.Length && (char.IsDigit(text2[m]) || text2[m] == '.'); m++)
							{
							}
							string text10 = text2.Substring(num5, m - num5);
							if (!float.TryParse(text10, out result4))
							{
								Debug.LogError((object)$"'{text10}' is not a valid value for wave separation.");
							}
						}
						if (text2.ToUpper().Contains("UNITED"))
						{
							item4 = TextMeshAnimator_IndependencyType.United;
						}
						if (text2.ToUpper().Contains("WORD"))
						{
							item4 = TextMeshAnimator_IndependencyType.Word;
						}
						if (text2.ToUpper().Contains("CHARACTER"))
						{
							item4 = TextMeshAnimator_IndependencyType.Character;
						}
						if (text2.ToUpper().Contains("VERTEX"))
						{
							item4 = TextMeshAnimator_IndependencyType.Vertex;
						}
					}
					else if (text2.ToUpper().Contains("/WIGGLE"))
					{
						item5 = false;
						result5 = 1f;
						result6 = 1f;
					}
					else if (text2.ToUpper().Contains("WIGGLE"))
					{
						item5 = true;
						string text11 = "INTENSITY=";
						if (text2.ToUpper().Contains(text11))
						{
							int num6 = text2.ToUpper().IndexOf(text11) + text11.Length;
							int n;
							for (n = num6; n < text2.Length && (char.IsDigit(text2[n]) || text2[n] == '.'); n++)
							{
							}
							string text12 = text2.Substring(num6, n - num6);
							if (!float.TryParse(text12, out result5))
							{
								Debug.LogError((object)$"'{text12}' is not a valid value for wiggle amount.");
							}
						}
						string text13 = "SPEED=";
						if (text2.ToUpper().Contains(text13))
						{
							int num7 = text2.ToUpper().IndexOf(text13) + text13.Length;
							int num8;
							for (num8 = num7; num8 < text2.Length && (char.IsDigit(text2[num8]) || text2[num8] == '.'); num8++)
							{
							}
							string text14 = text2.Substring(num7, num8 - num7);
							if (!float.TryParse(text14, out result6))
							{
								Debug.LogError((object)$"'{text14}' is not a valid value for wiggle speed.");
							}
						}
						if (text2.ToUpper().Contains("UNITED"))
						{
							item6 = TextMeshAnimator_IndependencyType.United;
						}
						if (text2.ToUpper().Contains("WORD"))
						{
							item6 = TextMeshAnimator_IndependencyType.Word;
						}
						if (text2.ToUpper().Contains("CHARACTER"))
						{
							item6 = TextMeshAnimator_IndependencyType.Character;
						}
						if (text2.ToUpper().Contains("VERTEX"))
						{
							item6 = TextMeshAnimator_IndependencyType.Vertex;
						}
					}
					else if (text2.ToUpper().Contains("/SPEED"))
					{
						item7 = 3;
					}
					else if (text2.ToUpper().Contains("SPEED"))
					{
						string text15 = "AMT=";
						int num9 = text2.ToUpper().IndexOf(text15) + text15.Length;
						int num10;
						for (num10 = num9; num10 < text2.Length && (char.IsDigit(text2[num10]) || text2[num10] == '.'); num10++)
						{
						}
						item7 = int.Parse(text2.Substring(num9, num10 - num9));
					}
					break;
				}
			}
			if (i < inputText.Length)
			{
				if (!char.IsControl(inputText[i]) && inputText[i] != ' ')
				{
					list.Add(item);
					list2.Add(result);
					list3.Add(item2);
					list4.Add(item3);
					list5.Add(result2);
					list6.Add(result3);
					list7.Add(result4);
					list8.Add(item4);
					list9.Add(item5);
					list10.Add(result5);
					list11.Add(result6);
					list12.Add(item6);
					list13.Add(item7);
				}
				text += inputText[i];
			}
		}
		shakesEnabled = list.ToArray();
		shakeVelocities = list2.ToArray();
		shakeIndependencies = list3.ToArray();
		wavesEnabled = list4.ToArray();
		waveVelocities = list5.ToArray();
		waveSpeeds = list6.ToArray();
		waveSeparations = list7.ToArray();
		waveIndependencies = list8.ToArray();
		wigglesEnabled = list9.ToArray();
		wiggleVelocities = list10.ToArray();
		wiggleSpeeds = list11.ToArray();
		wiggleIndependencies = list12.ToArray();
		scrollSpeeds = list13.ToArray();
		return text;
	}

	private void Start()
	{
		UpdateText();
	}

	public void BeginAnimation(string text = null)
	{
		UpdateText(text);
		currentFrame = 0;
	}

	private Vector3 ShakeVector(float amount)
	{
		//IL_0010: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(Random.Range(0f - amount, amount), Random.Range(0f - amount, amount));
	}

	private Vector3 WaveVector(float amount, float time)
	{
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		return new Vector3(0f, Mathf.Sin(time) * amount);
	}

	private Vector3 WiggleVector(float amount, float speed, ref int i)
	{
		//IL_01cc: Unknown result type (might be due to invalid IL or missing references)
		wiggleTimeCurrent[i * 2] += speed;
		if (wiggleTimeTotal[i * 2] == 0f || wiggleTimeCurrent[i * 2] / wiggleTimeTotal[i * 2] >= 1f)
		{
			wiggleTimeCurrent[i * 2] -= wiggleTimeTotal[i * 2];
			wiggleTimeTotal[i * 2] = Random.Range(wiggleMinimumDuration, 1f);
			wigglePrevPos[i * 2] = wiggleTargetPos[i * 2];
			wiggleTargetPos[i * 2] = Random.Range(0f - amount, amount);
		}
		wiggleTimeCurrent[i * 2 + 1] += speed;
		if (wiggleTimeTotal[i * 2 + 1] == 0f || wiggleTimeCurrent[i * 2 + 1] / wiggleTimeTotal[i * 2 + 1] >= 1f)
		{
			wiggleTimeCurrent[i * 2 + 1] -= wiggleTimeTotal[i * 2 + 1];
			wiggleTimeTotal[i * 2 + 1] = Random.Range(wiggleMinimumDuration, 1f);
			wigglePrevPos[i * 2 + 1] = wiggleTargetPos[i * 2 + 1];
			wiggleTargetPos[i * 2 + 1] = Random.Range(0f - amount, amount);
		}
		Vector3 result = new Vector3(Mathf.SmoothStep(wigglePrevPos[i * 2], wiggleTargetPos[i * 2], wiggleTimeCurrent[i * 2] / wiggleTimeTotal[i * 2]), Mathf.SmoothStep(wigglePrevPos[i * 2 + 1], wiggleTargetPos[i * 2 + 1], wiggleTimeCurrent[i * 2 + 1] / wiggleTimeTotal[i * 2 + 1]));
		i++;
		return result;
	}

	private void Update()
	{
		//IL_0002: Unknown result type (might be due to invalid IL or missing references)
		//IL_000a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0056: Unknown result type (might be due to invalid IL or missing references)
		//IL_005b: Unknown result type (might be due to invalid IL or missing references)
		//IL_00bf: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0125: Unknown result type (might be due to invalid IL or missing references)
		//IL_012a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0211: Unknown result type (might be due to invalid IL or missing references)
		//IL_0216: Unknown result type (might be due to invalid IL or missing references)
		//IL_0296: Unknown result type (might be due to invalid IL or missing references)
		//IL_029b: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d9: Unknown result type (might be due to invalid IL or missing references)
		//IL_02de: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_02b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_041b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0420: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_04e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_056f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0574: Unknown result type (might be due to invalid IL or missing references)
		//IL_0577: Unknown result type (might be due to invalid IL or missing references)
		//IL_057c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0581: Unknown result type (might be due to invalid IL or missing references)
		//IL_0543: Unknown result type (might be due to invalid IL or missing references)
		//IL_0548: Unknown result type (might be due to invalid IL or missing references)
		//IL_0658: Unknown result type (might be due to invalid IL or missing references)
		//IL_065d: Unknown result type (might be due to invalid IL or missing references)
		//IL_06e8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06ed: Unknown result type (might be due to invalid IL or missing references)
		//IL_078c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0791: Unknown result type (might be due to invalid IL or missing references)
		//IL_0736: Unknown result type (might be due to invalid IL or missing references)
		//IL_073b: Unknown result type (might be due to invalid IL or missing references)
		//IL_073e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0743: Unknown result type (might be due to invalid IL or missing references)
		//IL_0748: Unknown result type (might be due to invalid IL or missing references)
		//IL_070a: Unknown result type (might be due to invalid IL or missing references)
		//IL_070f: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = default(Vector3);
		Vector3 val2 = default(Vector3);
		Vector3 val3 = default(Vector3);
		for (int i = 0; i < ((TMP_Text)TMProGUI).textInfo.meshInfo.Length; i++)
		{
			int num = 0;
			float num2 = 1f;
			if (shakeVelocities.Length > num)
			{
				num2 = shakeVelocities[num];
			}
			int num3 = 0;
			TextMeshAnimator_IndependencyType textMeshAnimator_IndependencyType = shakeIndependency;
			if (textMeshAnimator_IndependencyType == TextMeshAnimator_IndependencyType.United)
			{
				val = ShakeVector(shakeAmount);
			}
			float num4 = 1f;
			if (waveVelocities.Length > num)
			{
				num4 = waveVelocities[num];
			}
			float num5 = 1f;
			if (waveSpeeds.Length > num)
			{
				num5 = waveSpeeds[num];
			}
			int num6 = 0;
			TextMeshAnimator_IndependencyType textMeshAnimator_IndependencyType2 = waveIndependency;
			if (textMeshAnimator_IndependencyType2 == TextMeshAnimator_IndependencyType.United)
			{
				val2 = WaveVector(waveAmount, (float)currentFrame * (waveSpeed * num5));
			}
			float num7 = 1f;
			if (wiggleVelocities.Length > num)
			{
				num7 = wiggleVelocities[num];
			}
			float num8 = 1f;
			if (wiggleSpeeds.Length > num)
			{
				num8 = wiggleSpeeds[num];
			}
			int num9 = 0;
			int i2 = 0;
			TextMeshAnimator_IndependencyType textMeshAnimator_IndependencyType3 = wiggleIndependency;
			if (textMeshAnimator_IndependencyType3 == TextMeshAnimator_IndependencyType.United)
			{
				val3 = WiggleVector(wiggleAmount, wiggleSpeed * num8, ref i2);
			}
			int num10 = 0;
			while (num10 < ((TMP_Text)TMProGUI).textInfo.meshInfo[i].vertices.Length)
			{
				for (byte b = 0; b < 4; b++)
				{
					((TMP_Text)TMProGUI).textInfo.meshInfo[i].vertices[num10 + b] = vertex_Base[i][num10 + b];
				}
				TextMeshAnimator_IndependencyType textMeshAnimator_IndependencyType4 = textMeshAnimator_IndependencyType;
				if (num < shakeIndependencies.Length)
				{
					textMeshAnimator_IndependencyType = shakeIndependencies[num];
				}
				if (num >= 1 && num < shakeIndependencies.Length + 1)
				{
					textMeshAnimator_IndependencyType4 = shakeIndependencies[num - 1];
				}
				if (textMeshAnimator_IndependencyType == TextMeshAnimator_IndependencyType.Word && num3 < ((TMP_Text)TMProGUI).text.Length && (((TMP_Text)TMProGUI).text[num3] == ' ' || char.IsControl(((TMP_Text)TMProGUI).text[num3]) || textMeshAnimator_IndependencyType4 != TextMeshAnimator_IndependencyType.Word || num3 == 0))
				{
					val = ShakeVector(shakeAmount);
					if (((TMP_Text)TMProGUI).text[num3] == ' ' || char.IsControl(((TMP_Text)TMProGUI).text[num3]))
					{
						num3++;
					}
				}
				num3++;
				bool flag = false;
				if (shakesEnabled.Length > num)
				{
					flag = shakesEnabled[num];
				}
				if (flag)
				{
					if (shakeVelocities.Length > num)
					{
						num2 = shakeVelocities[num];
					}
					if (textMeshAnimator_IndependencyType == TextMeshAnimator_IndependencyType.Character)
					{
						val = ShakeVector(shakeAmount);
					}
					for (byte b2 = 0; b2 < 4; b2++)
					{
						if (textMeshAnimator_IndependencyType == TextMeshAnimator_IndependencyType.Vertex)
						{
							val = ShakeVector(shakeAmount);
						}
						ref Vector3 reference = ref ((TMP_Text)TMProGUI).textInfo.meshInfo[i].vertices[num10 + b2];
						reference += val * num2;
					}
				}
				if (waveSpeeds.Length > num)
				{
					num5 = waveSpeeds[num];
				}
				float num11 = waveSeparation;
				if (waveSeparations.Length > num)
				{
					num11 = waveSeparations[num];
				}
				TextMeshAnimator_IndependencyType textMeshAnimator_IndependencyType5 = textMeshAnimator_IndependencyType2;
				if (num < waveIndependencies.Length)
				{
					textMeshAnimator_IndependencyType2 = waveIndependencies[num];
				}
				if (num >= 1 && num < waveIndependencies.Length + 1)
				{
					textMeshAnimator_IndependencyType5 = waveIndependencies[num - 1];
				}
				if (textMeshAnimator_IndependencyType2 == TextMeshAnimator_IndependencyType.Word && num6 < ((TMP_Text)TMProGUI).text.Length && (((TMP_Text)TMProGUI).text[num6] == ' ' || char.IsControl(((TMP_Text)TMProGUI).text[num6]) || textMeshAnimator_IndependencyType5 != TextMeshAnimator_IndependencyType.Word || num6 == 0))
				{
					val2 = WaveVector(waveAmount, (float)currentFrame * (waveSpeed * num5) + waveSpeed * num5 + ((TMP_Text)TMProGUI).textInfo.meshInfo[i].vertices[num10].x / (waveSeparation * num11));
					if (((TMP_Text)TMProGUI).text[num6] == ' ' || char.IsControl(((TMP_Text)TMProGUI).text[num6]))
					{
						num6++;
					}
				}
				num6++;
				bool flag2 = false;
				if (wavesEnabled.Length > num)
				{
					flag2 = wavesEnabled[num];
				}
				if (flag2)
				{
					if (waveVelocities.Length > num)
					{
						num4 = waveVelocities[num];
					}
					if (textMeshAnimator_IndependencyType2 == TextMeshAnimator_IndependencyType.Character)
					{
						val2 = WaveVector(waveAmount, (float)currentFrame * (waveSpeed * num5) + ((TMP_Text)TMProGUI).textInfo.meshInfo[i].vertices[num10].x / (waveSeparation * num11));
					}
					for (byte b3 = 0; b3 < 4; b3++)
					{
						if (textMeshAnimator_IndependencyType2 == TextMeshAnimator_IndependencyType.Vertex)
						{
							val2 = WaveVector(waveAmount, (float)currentFrame * (waveSpeed * num5) + ((TMP_Text)TMProGUI).textInfo.meshInfo[i].vertices[num10 + b3].x / (waveSeparation * num11));
						}
						ref Vector3 reference2 = ref ((TMP_Text)TMProGUI).textInfo.meshInfo[i].vertices[num10 + b3];
						reference2 += val2 * num4;
					}
				}
				num8 = wiggleSpeed;
				if (wiggleSpeeds.Length > num)
				{
					num8 = wiggleSpeeds[num];
				}
				TextMeshAnimator_IndependencyType textMeshAnimator_IndependencyType6 = textMeshAnimator_IndependencyType3;
				if (num < wiggleIndependencies.Length)
				{
					textMeshAnimator_IndependencyType3 = wiggleIndependencies[num];
				}
				if (num >= 1 && num < wiggleIndependencies.Length + 1)
				{
					textMeshAnimator_IndependencyType6 = wiggleIndependencies[num - 1];
				}
				if (textMeshAnimator_IndependencyType3 == TextMeshAnimator_IndependencyType.Word && num9 < ((TMP_Text)TMProGUI).text.Length && (((TMP_Text)TMProGUI).text[num9] == ' ' || char.IsControl(((TMP_Text)TMProGUI).text[num9]) || textMeshAnimator_IndependencyType6 != TextMeshAnimator_IndependencyType.Word || num9 == 0))
				{
					val3 = WiggleVector(wiggleAmount, wiggleSpeed * num8, ref i2);
					if (((TMP_Text)TMProGUI).text[num9] == ' ' || char.IsControl(((TMP_Text)TMProGUI).text[num9]))
					{
						num9++;
					}
				}
				num9++;
				bool flag3 = false;
				if (wigglesEnabled.Length > num)
				{
					flag3 = wigglesEnabled[num];
				}
				if (flag3)
				{
					if (wiggleVelocities.Length > num)
					{
						num7 = wiggleVelocities[num];
					}
					if (textMeshAnimator_IndependencyType3 == TextMeshAnimator_IndependencyType.Character)
					{
						val3 = WiggleVector(wiggleAmount, wiggleSpeed * num8, ref i2);
					}
					for (byte b4 = 0; b4 < 4; b4++)
					{
						if (textMeshAnimator_IndependencyType3 == TextMeshAnimator_IndependencyType.Vertex)
						{
							val3 = WiggleVector(wiggleAmount, wiggleSpeed * num8, ref i2);
						}
						ref Vector3 reference3 = ref ((TMP_Text)TMProGUI).textInfo.meshInfo[i].vertices[num10 + b4];
						reference3 += val3 * num7;
					}
				}
				if (num10 / 4 + 1 > charsVisible)
				{
					for (int j = 0; j < 4; j++)
					{
						((TMP_Text)TMProGUI).textInfo.meshInfo[i].vertices[num10 + j] = Vector3.zero;
					}
				}
				num10 += 4;
				num++;
			}
			((TMP_Text)TMProGUI).UpdateVertexData();
		}
		currentFrame++;
	}
}
