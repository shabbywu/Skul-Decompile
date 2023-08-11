using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class FreeParallaxElement
{
	internal readonly List<Renderer> GameObjectRenderers = new List<Renderer>();

	[Tooltip("Game objects to parallax. These will be cycled in sequence, which allows a long rolling background or different individual objects. If there is only one, and the reposition logic specifies to wrap, a second object will be added that is a clone of the first. It is recommended that these all be the same size.")]
	public List<GameObject> GameObjects;

	[Tooltip("The speed at which this object moves in relation to the speed of the parallax.")]
	[Range(-3f, 3f)]
	[FormerlySerializedAs("SpeedRatio")]
	public float SpeedRatioX;

	[Range(-3f, 3f)]
	public float SpeedRatioY;

	public float AutoScrollX;

	public Vector2 Translated;

	[Tooltip("Contains logic on how this object repositions itself when moving off screen.")]
	public FreeParallaxElementRepositionLogic RepositionLogic;

	[HideInInspector]
	public FreeParallaxElementRepositionLogicFunction RepositionLogicFunction;

	public void SetupState(FreeParallax p, Camera c, int index)
	{
		//IL_0074: Unknown result type (might be due to invalid IL or missing references)
		if (RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOffScreen && RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOnScreen && GameObjects.Count == 1)
		{
			GameObject val = Object.Instantiate<GameObject>(GameObjects[0]);
			val.transform.parent = GameObjects[0].transform.parent;
			val.transform.position = GameObjects[0].transform.position;
			GameObjects.Add(val);
		}
		if (GameObjectRenderers.Count != 0)
		{
			return;
		}
		foreach (GameObject gameObject in GameObjects)
		{
			Renderer component = gameObject.GetComponent<Renderer>();
			if ((Object)(object)component == (Object)null)
			{
				Debug.LogError((object)("Null renderer found at element index " + index + ", each game object in the parallax must have a renderer"));
				break;
			}
			GameObjectRenderers.Add(component);
		}
	}

	public void SetupScale(FreeParallax p, Camera c, int index)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_002e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0050: Unknown result type (might be due to invalid IL or missing references)
		//IL_00a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ac: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_00cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_00d8: Unknown result type (might be due to invalid IL or missing references)
		//IL_0068: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0080: Unknown result type (might be due to invalid IL or missing references)
		//IL_0085: Unknown result type (might be due to invalid IL or missing references)
		//IL_008a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0097: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0101: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f9: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0141: Unknown result type (might be due to invalid IL or missing references)
		//IL_0149: Unknown result type (might be due to invalid IL or missing references)
		//IL_015e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0234: Unknown result type (might be due to invalid IL or missing references)
		//IL_0239: Unknown result type (might be due to invalid IL or missing references)
		//IL_019c: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0267: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_01cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = c.ViewportToWorldPoint(Vector3.zero);
		for (int i = 0; i < GameObjects.Count; i++)
		{
			GameObject val2 = GameObjects[i];
			Renderer val3 = GameObjectRenderers[i];
			Bounds bounds = val3.bounds;
			if (RepositionLogic.ScaleHeight > 0f)
			{
				val2.transform.localScale = Vector3.one;
				float num;
				if (p.IsHorizontal)
				{
					Vector3 val4 = c.WorldToViewportPoint(new Vector3(0f, val.y + ((Bounds)(ref bounds)).size.y, 0f));
					num = RepositionLogic.ScaleHeight / val4.y;
				}
				else
				{
					Vector3 val5 = c.WorldToViewportPoint(new Vector3(val.x + ((Bounds)(ref bounds)).size.x, 0f, 0f));
					num = RepositionLogic.ScaleHeight / val5.x;
				}
				val2.transform.localScale = new Vector3(num, num, 1f);
				bounds = val3.bounds;
			}
			if (RepositionLogic.PositionMode == FreeParallaxPositionMode.IndividualStartOffScreen || RepositionLogic.PositionMode == FreeParallaxPositionMode.IndividualStartOnScreen || !(SpeedRatioX > 0f))
			{
				continue;
			}
			if (p.IsHorizontal)
			{
				float x = c.WorldToViewportPoint(new Vector3(val.x + ((Bounds)(ref bounds)).size.x, 0f, 0f)).x;
				if (x < 1.1f)
				{
					Debug.LogWarning((object)("Game object in element index " + index + " did not fit the screen width but was asked to wrap, so it was stretched. This can be fixed by making sure any parallax graphics that wrap are at least 1.1x times the largest width resolution you support."));
					Vector3 localScale = val2.transform.localScale;
					if (x != 0f)
					{
						localScale.x = localScale.x * (1f / x) + 0.1f;
					}
					val2.transform.localScale = localScale;
				}
				continue;
			}
			float y = c.WorldToViewportPoint(new Vector3(0f, val.y + ((Bounds)(ref bounds)).size.y, 0f)).y;
			if (y < 1.1f)
			{
				Debug.LogWarning((object)("Game object in element index " + index + " did not fit the screen height but was asked to wrap, so it was stretched. This can be fixed by making sure any parallax graphics that wrap are at least 1.1x times the largest height resolution you support."));
				Vector3 localScale2 = val2.transform.localScale;
				if (y != 0f)
				{
					localScale2.y = localScale2.y * (1f / y) + 0.1f;
				}
				val2.transform.localScale = localScale2;
			}
		}
	}

	public void SetupPosition(FreeParallax p, Camera c, int index)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0012: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_0063: Unknown result type (might be due to invalid IL or missing references)
		//IL_0070: Unknown result type (might be due to invalid IL or missing references)
		//IL_0076: Unknown result type (might be due to invalid IL or missing references)
		//IL_0089: Unknown result type (might be due to invalid IL or missing references)
		//IL_008e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0092: Unknown result type (might be due to invalid IL or missing references)
		//IL_0020: Unknown result type (might be due to invalid IL or missing references)
		//IL_002d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0033: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_004b: Unknown result type (might be due to invalid IL or missing references)
		//IL_004f: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a5: Unknown result type (might be due to invalid IL or missing references)
		//IL_01aa: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ae: Unknown result type (might be due to invalid IL or missing references)
		//IL_018d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0192: Unknown result type (might be due to invalid IL or missing references)
		//IL_0196: Unknown result type (might be due to invalid IL or missing references)
		//IL_0127: Unknown result type (might be due to invalid IL or missing references)
		//IL_012c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0130: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0229: Unknown result type (might be due to invalid IL or missing references)
		//IL_022d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0203: Unknown result type (might be due to invalid IL or missing references)
		//IL_0208: Unknown result type (might be due to invalid IL or missing references)
		//IL_020c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0247: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01d6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01da: Unknown result type (might be due to invalid IL or missing references)
		//IL_0165: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_016e: Unknown result type (might be due to invalid IL or missing references)
		//IL_014d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0152: Unknown result type (might be due to invalid IL or missing references)
		//IL_0156: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02c6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02cb: Unknown result type (might be due to invalid IL or missing references)
		//IL_02d1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02da: Unknown result type (might be due to invalid IL or missing references)
		//IL_02df: Unknown result type (might be due to invalid IL or missing references)
		//IL_02e3: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f1: Unknown result type (might be due to invalid IL or missing references)
		//IL_02f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_02fa: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0283: Unknown result type (might be due to invalid IL or missing references)
		//IL_028a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0293: Unknown result type (might be due to invalid IL or missing references)
		//IL_0298: Unknown result type (might be due to invalid IL or missing references)
		//IL_029c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0378: Unknown result type (might be due to invalid IL or missing references)
		//IL_037d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0381: Unknown result type (might be due to invalid IL or missing references)
		//IL_0358: Unknown result type (might be due to invalid IL or missing references)
		//IL_035d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_033b: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		Vector3 val = c.ViewportToWorldPoint(Vector3.zero);
		Vector3 val2 = c.ViewportToWorldPoint(Vector3.one);
		float num;
		Bounds bounds;
		float num3;
		if (p.IsHorizontal)
		{
			num = val2.y + 1f;
			float num2 = val.x + val2.x;
			bounds = GameObjectRenderers[0].bounds;
			num3 = (num2 + ((Bounds)(ref bounds)).size.x) / 2f;
		}
		else
		{
			num = val2.x + 1f;
			float num4 = val.y + val2.y;
			bounds = GameObjectRenderers[0].bounds;
			num3 = (num4 + ((Bounds)(ref bounds)).size.y) / 2f;
		}
		for (int i = 0; i < GameObjects.Count; i++)
		{
			GameObject val3 = GameObjects[i];
			Renderer val4 = GameObjectRenderers[i];
			if (RepositionLogic.SortingOrder != 0)
			{
				val4.sortingOrder = RepositionLogic.SortingOrder;
			}
			if (RepositionLogic.PositionMode == FreeParallaxPositionMode.IndividualStartOffScreen || RepositionLogic.PositionMode == FreeParallaxPositionMode.IndividualStartOnScreen)
			{
				float x;
				float y;
				if (p.IsHorizontal)
				{
					float num5;
					if (RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOnScreen)
					{
						num5 = 0f;
					}
					else
					{
						bounds = val4.bounds;
						num5 = ((Bounds)(ref bounds)).min.x;
					}
					x = num5;
					float num7;
					if (RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOnScreen)
					{
						float num6 = num;
						bounds = val4.bounds;
						num7 = num6 + ((Bounds)(ref bounds)).size.y;
					}
					else
					{
						bounds = val4.bounds;
						num7 = ((Bounds)(ref bounds)).min.y;
					}
					y = num7;
				}
				else
				{
					float num9;
					if (RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOnScreen)
					{
						float num8 = num;
						bounds = val4.bounds;
						num9 = num8 + ((Bounds)(ref bounds)).size.x;
					}
					else
					{
						bounds = val4.bounds;
						num9 = ((Bounds)(ref bounds)).min.x;
					}
					x = num9;
					float num10;
					if (RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOnScreen)
					{
						num10 = 0f;
					}
					else
					{
						bounds = val4.bounds;
						num10 = ((Bounds)(ref bounds)).min.y;
					}
					y = num10;
				}
				FreeParallax.SetPosition(val3, val4, x, y);
				continue;
			}
			if (p.IsHorizontal)
			{
				float num11 = num3;
				bounds = val4.bounds;
				num3 = num11 - (((Bounds)(ref bounds)).size.x - p.WrapOverlap);
			}
			else
			{
				float num12 = num3;
				bounds = val4.bounds;
				num3 = num12 - (((Bounds)(ref bounds)).size.y - p.WrapOverlap);
			}
			val3.transform.rotation = Quaternion.identity;
			if (RepositionLogic.PositionMode == FreeParallaxPositionMode.WrapAnchorTop)
			{
				if (p.IsHorizontal)
				{
					Vector3 val5 = c.ViewportToWorldPoint(new Vector3(0f, 1f, 0f));
					float x2 = num3;
					float y2 = val5.y;
					bounds = val4.bounds;
					FreeParallax.SetPosition(val3, val4, x2, y2 - ((Bounds)(ref bounds)).size.y);
				}
				else
				{
					Vector3 val6 = c.ViewportToWorldPoint(new Vector3(1f, 0f, 0f));
					float x3 = val6.x;
					bounds = val4.bounds;
					float x4 = x3 - ((Bounds)(ref bounds)).size.x;
					float num13 = num3;
					bounds = val4.bounds;
					FreeParallax.SetPosition(val3, val4, x4, num13 + ((Bounds)(ref bounds)).size.y);
				}
			}
			else if (RepositionLogic.PositionMode == FreeParallaxPositionMode.WrapAnchorBottom)
			{
				if (p.IsHorizontal)
				{
					FreeParallax.SetPosition(val3, val4, num3, val.y);
				}
				else
				{
					FreeParallax.SetPosition(val3, val4, val.x, num3);
				}
			}
			else if (p.IsHorizontal)
			{
				float x5 = num3;
				bounds = val4.bounds;
				FreeParallax.SetPosition(val3, val4, x5, ((Bounds)(ref bounds)).min.y);
			}
			else
			{
				bounds = val4.bounds;
				FreeParallax.SetPosition(val3, val4, ((Bounds)(ref bounds)).min.x, num3);
			}
			GameObjects.RemoveAt(i);
			GameObjects.Insert(0, val3);
			GameObjectRenderers.RemoveAt(i);
			GameObjectRenderers.Insert(0, val4);
		}
	}

	public void Randomize(FreeParallax p, Camera c)
	{
		//IL_00ad: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b2: Unknown result type (might be due to invalid IL or missing references)
		//IL_00b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00c3: Unknown result type (might be due to invalid IL or missing references)
		//IL_0100: Unknown result type (might be due to invalid IL or missing references)
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0031: Unknown result type (might be due to invalid IL or missing references)
		//IL_0035: Unknown result type (might be due to invalid IL or missing references)
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_007b: Unknown result type (might be due to invalid IL or missing references)
		if (p.IsHorizontal)
		{
			if (SpeedRatioX != 0f)
			{
				float num = 0f;
				for (int i = 0; i < GameObjects.Count; i++)
				{
					Bounds bounds = GameObjectRenderers[i].bounds;
					num += Math.Abs(((Bounds)(ref bounds)).max.x - ((Bounds)(ref bounds)).min.x);
				}
				Update(p, new Vector2(Random.Range(0f - num, num) / SpeedRatioX, 0f), c);
			}
		}
		else if (SpeedRatioY != 0f)
		{
			float num2 = 0f;
			for (int j = 0; j < GameObjects.Count; j++)
			{
				Bounds bounds2 = GameObjectRenderers[j].bounds;
				num2 += Math.Abs(((Bounds)(ref bounds2)).max.y - ((Bounds)(ref bounds2)).min.y);
			}
			Update(p, new Vector2(0f, Random.Range(0f - num2, num2) / SpeedRatioY), c);
		}
	}

	public void Update(FreeParallax p, Vector2 delta, Camera c)
	{
		//IL_0041: Unknown result type (might be due to invalid IL or missing references)
		//IL_0046: Unknown result type (might be due to invalid IL or missing references)
		//IL_0047: Unknown result type (might be due to invalid IL or missing references)
		//IL_004c: Unknown result type (might be due to invalid IL or missing references)
		//IL_006c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0079: Unknown result type (might be due to invalid IL or missing references)
		//IL_010b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0110: Unknown result type (might be due to invalid IL or missing references)
		//IL_011d: Unknown result type (might be due to invalid IL or missing references)
		//IL_0122: Unknown result type (might be due to invalid IL or missing references)
		//IL_00e5: Unknown result type (might be due to invalid IL or missing references)
		//IL_00ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_00f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_00fc: Unknown result type (might be due to invalid IL or missing references)
		//IL_0163: Unknown result type (might be due to invalid IL or missing references)
		//IL_0168: Unknown result type (might be due to invalid IL or missing references)
		//IL_016a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0189: Unknown result type (might be due to invalid IL or missing references)
		//IL_018e: Unknown result type (might be due to invalid IL or missing references)
		//IL_017a: Unknown result type (might be due to invalid IL or missing references)
		//IL_017f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0193: Unknown result type (might be due to invalid IL or missing references)
		//IL_01a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_019d: Unknown result type (might be due to invalid IL or missing references)
		//IL_01b5: Unknown result type (might be due to invalid IL or missing references)
		//IL_058c: Unknown result type (might be due to invalid IL or missing references)
		//IL_03e7: Unknown result type (might be due to invalid IL or missing references)
		//IL_02ce: Unknown result type (might be due to invalid IL or missing references)
		//IL_065c: Unknown result type (might be due to invalid IL or missing references)
		//IL_059c: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a4: Unknown result type (might be due to invalid IL or missing references)
		//IL_05a9: Unknown result type (might be due to invalid IL or missing references)
		//IL_04b7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03f7: Unknown result type (might be due to invalid IL or missing references)
		//IL_03ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_0404: Unknown result type (might be due to invalid IL or missing references)
		//IL_066c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0674: Unknown result type (might be due to invalid IL or missing references)
		//IL_0679: Unknown result type (might be due to invalid IL or missing references)
		//IL_04c7: Unknown result type (might be due to invalid IL or missing references)
		//IL_04cf: Unknown result type (might be due to invalid IL or missing references)
		//IL_04d4: Unknown result type (might be due to invalid IL or missing references)
		//IL_0241: Unknown result type (might be due to invalid IL or missing references)
		//IL_0246: Unknown result type (might be due to invalid IL or missing references)
		//IL_024a: Unknown result type (might be due to invalid IL or missing references)
		//IL_0256: Unknown result type (might be due to invalid IL or missing references)
		//IL_025b: Unknown result type (might be due to invalid IL or missing references)
		//IL_025f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0279: Unknown result type (might be due to invalid IL or missing references)
		//IL_027e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0282: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_01e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ea: Unknown result type (might be due to invalid IL or missing references)
		//IL_01f6: Unknown result type (might be due to invalid IL or missing references)
		//IL_01fb: Unknown result type (might be due to invalid IL or missing references)
		//IL_01ff: Unknown result type (might be due to invalid IL or missing references)
		//IL_021b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0220: Unknown result type (might be due to invalid IL or missing references)
		//IL_0224: Unknown result type (might be due to invalid IL or missing references)
		//IL_0684: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e1: Unknown result type (might be due to invalid IL or missing references)
		//IL_05e6: Unknown result type (might be due to invalid IL or missing references)
		//IL_05eb: Unknown result type (might be due to invalid IL or missing references)
		//IL_062c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0631: Unknown result type (might be due to invalid IL or missing references)
		//IL_0636: Unknown result type (might be due to invalid IL or missing references)
		//IL_063c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0643: Unknown result type (might be due to invalid IL or missing references)
		//IL_064a: Unknown result type (might be due to invalid IL or missing references)
		//IL_05cc: Unknown result type (might be due to invalid IL or missing references)
		//IL_04df: Unknown result type (might be due to invalid IL or missing references)
		//IL_043c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0441: Unknown result type (might be due to invalid IL or missing references)
		//IL_0446: Unknown result type (might be due to invalid IL or missing references)
		//IL_0487: Unknown result type (might be due to invalid IL or missing references)
		//IL_048c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0491: Unknown result type (might be due to invalid IL or missing references)
		//IL_0497: Unknown result type (might be due to invalid IL or missing references)
		//IL_049e: Unknown result type (might be due to invalid IL or missing references)
		//IL_04a6: Unknown result type (might be due to invalid IL or missing references)
		//IL_0427: Unknown result type (might be due to invalid IL or missing references)
		//IL_035c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0361: Unknown result type (might be due to invalid IL or missing references)
		//IL_0365: Unknown result type (might be due to invalid IL or missing references)
		//IL_037e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0383: Unknown result type (might be due to invalid IL or missing references)
		//IL_0387: Unknown result type (might be due to invalid IL or missing references)
		//IL_0306: Unknown result type (might be due to invalid IL or missing references)
		//IL_030b: Unknown result type (might be due to invalid IL or missing references)
		//IL_030f: Unknown result type (might be due to invalid IL or missing references)
		//IL_032a: Unknown result type (might be due to invalid IL or missing references)
		//IL_032f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0333: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b3: Unknown result type (might be due to invalid IL or missing references)
		//IL_06b8: Unknown result type (might be due to invalid IL or missing references)
		//IL_06bd: Unknown result type (might be due to invalid IL or missing references)
		//IL_06fe: Unknown result type (might be due to invalid IL or missing references)
		//IL_0703: Unknown result type (might be due to invalid IL or missing references)
		//IL_0708: Unknown result type (might be due to invalid IL or missing references)
		//IL_070e: Unknown result type (might be due to invalid IL or missing references)
		//IL_0715: Unknown result type (might be due to invalid IL or missing references)
		//IL_071c: Unknown result type (might be due to invalid IL or missing references)
		//IL_06a1: Unknown result type (might be due to invalid IL or missing references)
		//IL_0511: Unknown result type (might be due to invalid IL or missing references)
		//IL_0516: Unknown result type (might be due to invalid IL or missing references)
		//IL_051b: Unknown result type (might be due to invalid IL or missing references)
		//IL_055c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0561: Unknown result type (might be due to invalid IL or missing references)
		//IL_0566: Unknown result type (might be due to invalid IL or missing references)
		//IL_056c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0573: Unknown result type (might be due to invalid IL or missing references)
		//IL_057b: Unknown result type (might be due to invalid IL or missing references)
		//IL_04fc: Unknown result type (might be due to invalid IL or missing references)
		if (GameObjects == null || GameObjects.Count == 0 || GameObjects.Count != GameObjectRenderers.Count)
		{
			return;
		}
		delta.x += AutoScrollX;
		Translated += delta;
		foreach (GameObject gameObject in GameObjects)
		{
			gameObject.transform.Translate(delta.x * SpeedRatioX, delta.y * SpeedRatioY, 0f);
		}
		bool flag = RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOffScreen && RepositionLogic.PositionMode != FreeParallaxPositionMode.IndividualStartOnScreen;
		float num = (flag ? 0f : 1f);
		Rect rect;
		float num2;
		float num3;
		if (p.IsHorizontal)
		{
			rect = c.rect;
			num2 = ((Rect)(ref rect)).x - num;
			rect = c.rect;
			num3 = ((Rect)(ref rect)).width + num;
		}
		else
		{
			rect = c.rect;
			num2 = ((Rect)(ref rect)).y - num;
			rect = c.rect;
			num3 = ((Rect)(ref rect)).height + num;
		}
		int num4 = GameObjects.Count;
		for (int i = 0; i < num4; i++)
		{
			GameObject val = GameObjects[i];
			Renderer val2 = GameObjectRenderers[i];
			Bounds bounds = val2.bounds;
			Vector3 val3 = ((delta.x > 0f) ? c.WorldToViewportPoint(((Bounds)(ref bounds)).min) : c.WorldToViewportPoint(((Bounds)(ref bounds)).max));
			float num5 = (p.IsHorizontal ? val3.x : val3.y);
			if (flag)
			{
				Bounds bounds2;
				if (delta.x > 0f && num5 >= num3)
				{
					if (p.IsHorizontal)
					{
						bounds2 = GameObjectRenderers[0].bounds;
						float x = ((Bounds)(ref bounds2)).min.x;
						bounds2 = val2.bounds;
						float x2 = x - ((Bounds)(ref bounds2)).size.x + p.WrapOverlap;
						bounds2 = val2.bounds;
						FreeParallax.SetPosition(val, val2, x2, ((Bounds)(ref bounds2)).min.y);
					}
					else
					{
						bounds2 = GameObjectRenderers[0].bounds;
						float y = ((Bounds)(ref bounds2)).min.y;
						bounds2 = val2.bounds;
						float y2 = y - ((Bounds)(ref bounds2)).size.y + p.WrapOverlap;
						bounds2 = val2.bounds;
						FreeParallax.SetPosition(val, val2, ((Bounds)(ref bounds2)).min.x, y2);
					}
					GameObjects.RemoveAt(i);
					GameObjects.Insert(0, val);
					GameObjectRenderers.RemoveAt(i);
					GameObjectRenderers.Insert(0, val2);
				}
				else if (delta.x < 0f && num5 <= num2)
				{
					if (p.IsHorizontal)
					{
						bounds2 = GameObjectRenderers[GameObjects.Count - 1].bounds;
						float x3 = ((Bounds)(ref bounds2)).max.x - p.WrapOverlap;
						bounds2 = val2.bounds;
						FreeParallax.SetPosition(val, val2, x3, ((Bounds)(ref bounds2)).min.y);
					}
					else
					{
						bounds2 = GameObjectRenderers[GameObjects.Count - 1].bounds;
						float y3 = ((Bounds)(ref bounds2)).max.y - p.WrapOverlap;
						bounds2 = val2.bounds;
						FreeParallax.SetPosition(val, val2, ((Bounds)(ref bounds2)).min.x, y3);
					}
					GameObjects.RemoveAt(i);
					GameObjects.Add(val);
					GameObjectRenderers.RemoveAt(i--);
					GameObjectRenderers.Add(val2);
					num4--;
				}
				continue;
			}
			if (p.IsHorizontal)
			{
				if (delta.x > 0f)
				{
					float y4 = val3.y;
					rect = c.rect;
					if (y4 >= ((Rect)(ref rect)).height || num5 >= num3)
					{
						if (RepositionLogicFunction != null)
						{
							RepositionLogicFunction(p, this, delta.x, val, val2);
							continue;
						}
						Vector3 val4 = c.ViewportToWorldPoint(Vector3.zero);
						float num6 = Random.Range(RepositionLogic.MinXPercent, RepositionLogic.MaxXPercent);
						float num7 = Random.Range(RepositionLogic.MinYPercent, RepositionLogic.MaxYPercent);
						Vector3 val5 = c.ViewportToWorldPoint(new Vector3(num6, num7));
						FreeParallax.SetPosition(val, val2, val4.x - val5.x, val5.y);
						continue;
					}
				}
				if (!(delta.x < 0f))
				{
					continue;
				}
				float y5 = val3.y;
				rect = c.rect;
				if (y5 >= ((Rect)(ref rect)).height || val3.x < num2)
				{
					if (RepositionLogicFunction != null)
					{
						RepositionLogicFunction(p, this, delta.x, val, val2);
						continue;
					}
					Vector3 val6 = c.ViewportToWorldPoint(Vector3.one);
					float num8 = Random.Range(RepositionLogic.MinXPercent, RepositionLogic.MaxXPercent);
					float num9 = Random.Range(RepositionLogic.MinYPercent, RepositionLogic.MaxYPercent);
					Vector3 val7 = c.ViewportToWorldPoint(new Vector3(num8, num9));
					FreeParallax.SetPosition(val, val2, val6.x + val7.x, val7.y);
				}
				continue;
			}
			if (delta.x > 0f)
			{
				float x4 = val3.x;
				rect = c.rect;
				if (x4 >= ((Rect)(ref rect)).width || num5 >= num3)
				{
					if (RepositionLogicFunction != null)
					{
						RepositionLogicFunction(p, this, delta.x, val, val2);
						continue;
					}
					Vector3 val8 = c.ViewportToWorldPoint(Vector3.zero);
					float num10 = Random.Range(RepositionLogic.MinXPercent, RepositionLogic.MaxXPercent);
					float num11 = Random.Range(RepositionLogic.MinYPercent, RepositionLogic.MaxYPercent);
					Vector3 val9 = c.ViewportToWorldPoint(new Vector3(num10, num11));
					FreeParallax.SetPosition(val, val2, val9.x, val8.y - val9.y);
					continue;
				}
			}
			if (!(delta.x < 0f))
			{
				continue;
			}
			float x5 = val3.x;
			rect = c.rect;
			if (x5 >= ((Rect)(ref rect)).width || val3.y < num2)
			{
				if (RepositionLogicFunction != null)
				{
					RepositionLogicFunction(p, this, delta.x, val, val2);
					continue;
				}
				Vector3 val10 = c.ViewportToWorldPoint(Vector3.one);
				float num12 = Random.Range(RepositionLogic.MinXPercent, RepositionLogic.MaxXPercent);
				float num13 = Random.Range(RepositionLogic.MinYPercent, RepositionLogic.MaxYPercent);
				Vector3 val11 = c.ViewportToWorldPoint(new Vector3(num12, num13));
				FreeParallax.SetPosition(val, val2, val11.x, val10.y + val11.y);
			}
		}
	}
}
