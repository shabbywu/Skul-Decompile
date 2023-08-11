using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public sealed class UnlockNotice : MonoBehaviour
{
	[SerializeField]
	private Image _icon;

	[SerializeField]
	private TextMeshProUGUI _name;

	[SerializeField]
	private Animator _animator;

	public void Show(Sprite icon, string name)
	{
		_icon.sprite = icon;
		((Graphic)_icon).SetNativeSize();
		((TMP_Text)_name).text = name;
		((Component)this).gameObject.SetActive(true);
		if (((Component)this).gameObject.activeInHierarchy)
		{
			((MonoBehaviour)this).StopAllCoroutines();
			((MonoBehaviour)this).StartCoroutine(CFadeInOut());
		}
	}

	private IEnumerator CFadeInOut()
	{
		if ((Object)(object)_animator.runtimeAnimatorController != (Object)null)
		{
			if (!((Behaviour)_animator).enabled)
			{
				((Behaviour)_animator).enabled = true;
			}
			_animator.Play(0, 0, 0f);
		}
		AnimatorStateInfo currentAnimatorStateInfo = _animator.GetCurrentAnimatorStateInfo(0);
		float remain = ((AnimatorStateInfo)(ref currentAnimatorStateInfo)).length;
		((Behaviour)_animator).enabled = false;
		while (remain > float.Epsilon)
		{
			yield return null;
			float unscaledDeltaTime = Time.unscaledDeltaTime;
			_animator.Update(unscaledDeltaTime);
			remain -= unscaledDeltaTime;
		}
		((Component)this).gameObject.SetActive(false);
	}
}
