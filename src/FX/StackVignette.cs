using UnityEngine;
using UnityEngine.UI;

namespace FX;

public class StackVignette : MonoBehaviour
{
	[SerializeField]
	[GetComponent]
	private Image _image;

	[SerializeField]
	[MinMaxSlider(0f, 100f)]
	private Vector2Int _stackRange;

	public void UpdateStack(float stack)
	{
		if (stack <= (float)((Vector2Int)(ref _stackRange)).x)
		{
			UpdateAlpha(0f);
		}
		else
		{
			UpdateAlpha(stack / (float)((Vector2Int)(ref _stackRange)).y);
		}
	}

	private void UpdateAlpha(float a)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_001a: Unknown result type (might be due to invalid IL or missing references)
		Color color = ((Graphic)_image).color;
		color.a = a;
		((Graphic)_image).color = color;
	}

	public void Hide()
	{
		UpdateAlpha(0f);
	}
}
