using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ReleaseAddressableHandleOnDestroy : MonoBehaviour
{
	private AsyncOperationHandle<GameObject> _handle;

	public static void Reserve(GameObject gameObject, AsyncOperationHandle<GameObject> handle)
	{
		//IL_0006: Unknown result type (might be due to invalid IL or missing references)
		//IL_0007: Unknown result type (might be due to invalid IL or missing references)
		gameObject.AddComponent<ReleaseAddressableHandleOnDestroy>()._handle = handle;
	}

	private void OnDestroy()
	{
		//IL_000f: Unknown result type (might be due to invalid IL or missing references)
		if (_handle.IsValid())
		{
			Addressables.Release<GameObject>(_handle);
		}
	}
}
