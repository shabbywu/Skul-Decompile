using System.Collections;
using UnityEngine;

namespace Characters.AI.Hero;

public class SaintSwordEmerge : MonoBehaviour
{
	[SerializeField]
	private Transform _body;

	[SerializeField]
	private float _duration;

	[SerializeField]
	private Transform _sourceTransfom;

	[SerializeField]
	private Transform _destTransfom;

	public void Emerge(Character owner)
	{
		((MonoBehaviour)this).StartCoroutine(CMove(owner));
	}

	private IEnumerator CMove(Character owner)
	{
		float elapsed = 0f;
		Vector3 source = _sourceTransfom.position;
		Vector3 dest = _destTransfom.position;
		((Component)this).transform.position = source;
		Show();
		while (elapsed < _duration)
		{
			yield return null;
			elapsed += owner.chronometer.master.deltaTime;
			((Component)this).transform.position = Vector2.op_Implicit(Vector2.Lerp(Vector2.op_Implicit(source), Vector2.op_Implicit(dest), elapsed / _duration));
		}
		((Component)this).transform.position = dest;
	}

	private void Show()
	{
		((Component)_body).gameObject.SetActive(true);
	}

	public void Hide()
	{
		((Component)_body).gameObject.SetActive(false);
	}
}
