using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EndingCredit;

public class CreditList : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _maker;

	[SerializeField]
	private Transform _destination;

	[SerializeField]
	private ContentSizeFitter _contentSizeFitter;

	private List<GameObject> _creditList = new List<GameObject>();

	public void Add(GameObject[] supporter)
	{
		AddList(_maker);
		AddList(supporter);
		((MonoBehaviour)this).StartCoroutine(CRun());
	}

	private void AddList(GameObject[] list)
	{
		foreach (GameObject item in list)
		{
			_creditList.Add(item);
		}
	}

	private IEnumerator CRun()
	{
		Refresh();
		int currentCreditListIndex = 1;
		int listCount = _creditList.Count - 1;
		while (currentCreditListIndex < listCount)
		{
			Vector3 val = ((Component)_destination).transform.position - _creditList[currentCreditListIndex].transform.position;
			if (((Vector3)(ref val)).normalized.y < 0f)
			{
				Activate(currentCreditListIndex + 1);
				Deactivate(currentCreditListIndex - 1);
				currentCreditListIndex++;
			}
			yield return null;
		}
	}

	private void Activate(int index)
	{
		_creditList[index].SetActive(true);
	}

	private void Deactivate(int index)
	{
		_creditList[index].SetActive(false);
	}

	private void Refresh()
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_0015: Expected O, but got Unknown
		LayoutRebuilder.ForceRebuildLayoutImmediate((RectTransform)((Component)_contentSizeFitter).transform);
	}
}
