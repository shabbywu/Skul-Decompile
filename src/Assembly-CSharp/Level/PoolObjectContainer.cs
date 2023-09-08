using UnityEngine;

namespace Level;

public sealed class PoolObjectContainer : MonoBehaviour
{
	public void Push(PoolObject poolObject)
	{
		((Component)poolObject).transform.SetParent(((Component)this).transform);
	}

	public void DespawnAll()
	{
		for (int num = ((Component)this).transform.childCount - 1; num >= 0; num--)
		{
			PoolObject component = ((Component)((Component)this).transform.GetChild(num)).GetComponent<PoolObject>();
			if ((Object)(object)component != (Object)null)
			{
				component.Despawn();
			}
		}
	}
}
