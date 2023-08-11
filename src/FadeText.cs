using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FadeText : MonoBehaviour
{
	private Text textRef;

	public float alpha;

	private void Start()
	{
		textRef = ((Component)this).GetComponent<Text>();
	}

	private void Update()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		alpha = ((Graphic)textRef).color.a;
		if (alpha > 0f)
		{
			alpha -= 0.01f;
			Color color = default(Color);
			((Color)(ref color))._002Ector(1f, 1f, 1f, alpha);
			((Graphic)textRef).color = color;
		}
	}
}
