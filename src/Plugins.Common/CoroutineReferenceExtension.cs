using System.Collections;
using UnityEngine;

public static class CoroutineReferenceExtension
{
	public static CoroutineReference StartCoroutineWithReference(this MonoBehaviour monoBehaviour, IEnumerator routine)
	{
		return new CoroutineReference(monoBehaviour, monoBehaviour.StartCoroutine(routine));
	}

	public static CoroutineReference StartCoroutineWithReference(this MonoBehaviour monoBehaviour, string methodName)
	{
		return new CoroutineReference(monoBehaviour, monoBehaviour.StartCoroutine(methodName));
	}

	public static CoroutineReference StartCoroutineWithReference(this MonoBehaviour monoBehaviour, string methodName, object value)
	{
		return new CoroutineReference(monoBehaviour, monoBehaviour.StartCoroutine(methodName, value));
	}
}
