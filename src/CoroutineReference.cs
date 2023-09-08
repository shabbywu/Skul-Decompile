using UnityEngine;

public struct CoroutineReference
{
	public Coroutine coroutine { get; private set; }

	public MonoBehaviour monoBehaviour { get; private set; }

	public CoroutineReference(MonoBehaviour monoBehaviour, Coroutine coroutine)
	{
		this.monoBehaviour = monoBehaviour;
		this.coroutine = coroutine;
	}

	public void Stop()
	{
		if (coroutine != null && (Object)(object)monoBehaviour != (Object)null)
		{
			monoBehaviour.StopCoroutine(coroutine);
			coroutine = null;
		}
	}

	public void Clear()
	{
		coroutine = null;
	}
}
