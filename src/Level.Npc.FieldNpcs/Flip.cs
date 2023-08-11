using System.Collections;
using UnityEngine;

namespace Level.Npc.FieldNpcs;

[ExecuteAlways]
public class Flip : MonoBehaviour
{
	[SerializeField]
	private Transform _body;

	private void Awake()
	{
		if (Application.isPlaying && ((Behaviour)this).isActiveAndEnabled)
		{
			((MonoBehaviour)this).StartCoroutine(CRun());
		}
		IEnumerator CRun()
		{
			yield return null;
			((Component)this).GetComponentInChildren<FieldNpc>()?.Flip();
			Object.Destroy((Object)(object)this);
		}
	}
}
