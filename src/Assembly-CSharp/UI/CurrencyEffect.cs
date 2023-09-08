using System.Collections;
using UnityEngine;

namespace UI;

public class CurrencyEffect : MonoBehaviour
{
	[SerializeField]
	private Animator _animator;

	private float _animationLength;

	private float _remainTime;

	private void Awake()
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
		_animationLength = ((AnimatorStateInfo)(ref currentAnimatorStateInfo)).length;
	}

	public void Play()
	{
		((Behaviour)_animator).enabled = true;
		_animator.Play(0, 0, 0f);
		((Behaviour)_animator).enabled = false;
		_remainTime = _animationLength;
		if (!((Component)this).gameObject.activeSelf)
		{
			((Component)this).gameObject.SetActive(true);
			((MonoBehaviour)CoroutineProxy.instance).StartCoroutine(CPlay());
		}
	}

	private IEnumerator CPlay()
	{
		while (_remainTime > 0f)
		{
			yield return null;
			float deltaTime = Chronometer.global.deltaTime;
			_animator.Update(deltaTime);
			_remainTime -= deltaTime;
		}
		((Component)this).gameObject.SetActive(false);
	}
}
