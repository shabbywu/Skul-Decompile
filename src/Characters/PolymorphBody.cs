using System;
using System.Collections;
using Characters.Operations;
using UnityEngine;

namespace Characters;

public class PolymorphBody : MonoBehaviour
{
	[NonSerialized]
	public Character character;

	[SerializeField]
	private CharacterAnimation _characterAnimation;

	[SerializeField]
	private GameObject _originalBody;

	[SerializeField]
	private GameObject _body;

	[SerializeField]
	[Space]
	private RuntimeAnimatorController _baseAnimator;

	[SerializeField]
	private AnimationClip _idleClip;

	[SerializeField]
	private AnimationClip _walkClip;

	[SerializeField]
	private AnimationClip _jumpClip;

	[SerializeField]
	private AnimationClip _fallClip;

	[SerializeField]
	private AnimationClip _fallRepeatClip;

	[SerializeField]
	[Space]
	[CharacterOperation.Subcomponent]
	private CharacterOperation.Subcomponents _operationOnEnd;

	protected AnimationClipOverrider _overrider;

	private void Awake()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		//IL_0019: Expected O, but got Unknown
		if (_overrider == null)
		{
			_overrider = new AnimationClipOverrider(_baseAnimator);
		}
		_overrider.Override("EmptyIdle", _idleClip);
		_overrider.Override("EmptyWalk", _walkClip);
		_overrider.Override("EmptyJumpUp", _jumpClip);
		_overrider.Override("EmptyJumpDown", _fallClip);
		_overrider.Override("EmptyJumpDownLoop", _fallRepeatClip);
		_operationOnEnd.Initialize();
	}

	private void OnDestroy()
	{
		_originalBody = null;
		_body = null;
		_baseAnimator = null;
		if (_overrider != null)
		{
			_overrider.Dispose();
			_overrider = null;
		}
		_idleClip = null;
		_walkClip = null;
		_jumpClip = null;
		_fallClip = null;
		_fallRepeatClip = null;
	}

	public void StartPolymorph(float duration)
	{
		StartPolymorph();
		((MonoBehaviour)this).StartCoroutine(CEndPolymorph(duration));
	}

	public void StartPolymorph()
	{
		if (!_body.activeSelf)
		{
			_originalBody.gameObject.SetActive(false);
			_body.SetActive(true);
			character.CancelAction();
			_characterAnimation.AttachOverrider(_overrider);
			character.animationController.ForceUpdate();
		}
	}

	public void EndPolymorph()
	{
		if (_body.activeSelf)
		{
			_operationOnEnd.Run(character);
			_originalBody.gameObject.SetActive(true);
			_body.SetActive(false);
			character.CancelAction();
			_characterAnimation.DetachOverrider(_overrider);
			character.animationController.ForceUpdate();
		}
	}

	private IEnumerator CEndPolymorph(float duration)
	{
		yield return character.chronometer.master.WaitForSeconds(duration);
		EndPolymorph();
	}

	private void OnDisable()
	{
		EndPolymorph();
	}
}
