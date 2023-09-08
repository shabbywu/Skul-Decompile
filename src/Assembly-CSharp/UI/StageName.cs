using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI;

public sealed class StageName : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _chapterName;

	[SerializeField]
	private TextMeshProUGUI _stageNumber;

	[SerializeField]
	private TextMeshProUGUI _stageName;

	[SerializeField]
	private Image _background;

	[SerializeField]
	private HangingPanelAnimator _animator;

	public void Show(string chapterName, string stageNumber, string stageName)
	{
		((Component)this).gameObject.SetActive(true);
		((TMP_Text)_chapterName).text = chapterName;
		((TMP_Text)_stageNumber).text = stageNumber;
		((TMP_Text)_stageName).text = stageName;
		((MonoBehaviour)this).StartCoroutine(CShow());
	}

	private IEnumerator CShow()
	{
		_animator.Appear();
		yield return (object)new WaitForSecondsRealtime(4f);
		_animator.Disappear();
	}
}
