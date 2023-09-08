using System.Collections;
using UnityEngine;

namespace Characters.Operations.Fx;

public sealed class SetOwnerColor : CharacterOperation
{
	[SerializeField]
	private Color _color;

	[SerializeField]
	private Curve _curve;

	private Character _owner;

	private Color _originColor;

	private CoroutineReference _changeReference;

	public override void Run(Character owner)
	{
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0034: Unknown result type (might be due to invalid IL or missing references)
		//IL_0057: Unknown result type (might be due to invalid IL or missing references)
		if (owner.spriteEffectStack != null && !((Object)(object)owner.spriteEffectStack.mainRenderer == (Object)null))
		{
			_owner = owner;
			_originColor = owner.spriteEffectStack.mainRenderer.color;
			if (_curve.duration == 0f)
			{
				owner.spriteEffectStack.mainRenderer.color = _color;
			}
			else
			{
				_changeReference = ((MonoBehaviour)(object)this).StartCoroutineWithReference(CChangeColor());
			}
		}
	}

	private IEnumerator CChangeColor()
	{
		float elapsed = 0f;
		SpriteRenderer renderer = _owner.spriteEffectStack.mainRenderer;
		while (elapsed <= _curve.duration)
		{
			renderer.color = Color.Lerp(_originColor, _color, _curve.Evaluate(elapsed / _curve.duration));
			elapsed += _owner.chronometer.master.deltaTime;
			yield return null;
		}
		renderer.color = _originColor;
	}

	public override void Stop()
	{
		//IL_0022: Unknown result type (might be due to invalid IL or missing references)
		base.Stop();
		_changeReference.Stop();
		_owner.spriteEffectStack.mainRenderer.color = _originColor;
	}

	private void OnDisable()
	{
		Stop();
	}
}
