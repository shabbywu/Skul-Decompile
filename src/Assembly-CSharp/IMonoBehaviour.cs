using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMonoBehaviour
{
	string name { get; }

	GameObject gameObject { get; }

	Transform transform { get; }

	bool enabled { get; set; }

	bool isActiveAndEnabled { get; }

	T GetComponent<T>();

	T GetComponentInChildren<T>(bool includeInactive = false);

	T GetComponentInChildren<T>();

	T GetComponentInParent<T>();

	T[] GetComponents<T>();

	void GetComponents<T>(List<T> results);

	void GetComponentsInChildren<T>(List<T> results);

	T[] GetComponentsInChildren<T>(bool includeInactive);

	void GetComponentsInChildren<T>(bool includeInactive, List<T> result);

	T[] GetComponentsInChildren<T>();

	T[] GetComponentsInParent<T>();

	T[] GetComponentsInParent<T>(bool includeInactive);

	void GetComponentsInParent<T>(bool includeInactive, List<T> results);

	Coroutine StartCoroutine(IEnumerator routine);

	void StopAllCoroutines();

	void StopCoroutine(IEnumerator routine);

	void StopCoroutine(Coroutine routine);
}
