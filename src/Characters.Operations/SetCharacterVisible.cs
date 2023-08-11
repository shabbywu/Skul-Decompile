using System.Collections;
using UnityEngine;

namespace Characters.Operations;

public class SetCharacterVisible : CharacterOperation
{
	[SerializeField]
	private bool _visible;

	[SerializeField]
	private Renderer _render;

	[SerializeField]
	private Collider2D _collider;

	[SerializeField]
	private bool _findComponentInOwner;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private GameObject[] _extras;

	public override void Run(Character owner)
	{
		if (_findComponentInOwner)
		{
			_collider = (Collider2D)(object)owner.collider;
			_render = (Renderer)(object)owner.spriteEffectStack.mainRenderer;
		}
		if (_duration <= 0f)
		{
			SetVisible(_visible);
		}
		else
		{
			((MonoBehaviour)this).StartCoroutine(CRun(owner.chronometer.master));
		}
	}

	private void SetVisible(bool visible)
	{
		if ((Object)(object)_collider != (Object)null)
		{
			((Behaviour)_collider).enabled = visible;
		}
		if ((Object)(object)_render != (Object)null)
		{
			_render.enabled = visible;
		}
		if (_extras != null)
		{
			GameObject[] extras = _extras;
			for (int i = 0; i < extras.Length; i++)
			{
				extras[i].SetActive(visible);
			}
		}
	}

	private IEnumerator CRun(Chronometer chronometer)
	{
		SetVisible(_visible);
		if (_duration != 0f)
		{
			yield return ChronometerExtension.WaitForSeconds((ChronometerBase)(object)chronometer, _duration);
		}
		SetVisible(!_visible);
	}
}
