using UnityEngine;

namespace Level;

public class EntSapling : MonoBehaviour
{
	[SerializeField]
	private PoolObject _pool;

	[SerializeField]
	private GameObject _introInvoker;

	public void RunIntro(bool activate)
	{
		_introInvoker.SetActive(activate);
	}

	public PoolObject Spawn(Vector3 position, bool intro)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		PoolObject obj = _pool.Spawn(position, Quaternion.identity, true);
		((Component)obj).GetComponent<EntSapling>().RunIntro(intro);
		return obj;
	}

	public void Despawn()
	{
		_introInvoker.SetActive(false);
		_pool.Despawn();
	}
}
