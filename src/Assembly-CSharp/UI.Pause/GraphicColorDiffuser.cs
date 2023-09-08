using UnityEngine;
using UnityEngine.UI;

namespace UI.Pause;

public class GraphicColorDiffuser : Graphic
{
	[SerializeField]
	private Graphic[] _graphics;

	public override Color color
	{
		get
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			return ((Graphic)this).color;
		}
		set
		{
			//IL_0001: Unknown result type (might be due to invalid IL or missing references)
			//IL_0015: Unknown result type (might be due to invalid IL or missing references)
			((Graphic)this).color = value;
			Graphic[] graphics = _graphics;
			for (int i = 0; i < graphics.Length; i++)
			{
				graphics[i].color = value;
			}
		}
	}

	public override void CrossFadeColor(Color targetColor, float duration, bool ignoreTimeScale, bool useAlpha)
	{
		//IL_0001: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Unknown result type (might be due to invalid IL or missing references)
		((Graphic)this).CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
		Graphic[] graphics = _graphics;
		for (int i = 0; i < graphics.Length; i++)
		{
			graphics[i].CrossFadeColor(targetColor, duration, ignoreTimeScale, useAlpha);
		}
	}
}
