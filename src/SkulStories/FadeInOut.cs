using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace SkulStories;

public class FadeInOut : Sequence
{
	[SerializeField]
	private Color _target;

	[SerializeField]
	private float _duration;

	private Color _originColor;

	private Image _image;

	private void Start()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		//IL_001d: Unknown result type (might be due to invalid IL or missing references)
		_image = _narration.blackScreen;
		_originColor = ((Graphic)_image).color;
	}

	public override IEnumerator CRun()
	{
		Color startColor = ((Graphic)_image).color;
		Color different = _target - ((Graphic)_image).color;
		float elapsed = 0f;
		while (elapsed < _duration)
		{
			elapsed += ((ChronometerBase)Chronometer.global).deltaTime;
			((Graphic)_image).color = startColor + different * (elapsed / _duration);
			yield return null;
		}
		((Graphic)_image).color = _target;
	}

	public void OnDisable()
	{
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		((Graphic)_image).color = _originColor;
	}
}
