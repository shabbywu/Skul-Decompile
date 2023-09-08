using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace GameResources;

public abstract class Request<T> where T : Component
{
	private AsyncOperationHandle<GameObject> _handle;

	public AsyncOperationHandle<GameObject> handle => _handle;

	public T asset => _handle.Result.GetComponent<T>();

	public bool isDone => _handle.IsDone;

	public bool releaseReserved { get; private set; }

	public Request(string path)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		_handle = Addressables.LoadAssetAsync<GameObject>((object)path);
	}

	public Request(AssetReference reference)
	{
		//IL_0008: Unknown result type (might be due to invalid IL or missing references)
		//IL_000d: Unknown result type (might be due to invalid IL or missing references)
		_handle = reference.LoadAssetAsync<GameObject>();
	}

	public T Instantiate(Vector3 position, Quaternion rotation)
	{
		//IL_000b: Unknown result type (might be due to invalid IL or missing references)
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0014: Unknown result type (might be due to invalid IL or missing references)
		GameObject obj = Object.Instantiate<GameObject>(_handle.Result, position, rotation);
		ReleaseAddressableHandleOnDestroy.Reserve(obj, _handle);
		releaseReserved = true;
		return obj.GetComponent<T>();
	}

	public void WaitForCompletion()
	{
		_handle.WaitForCompletion();
	}

	public void SetReleaseReserved()
	{
		releaseReserved = true;
	}

	public void Release()
	{
		//IL_0018: Unknown result type (might be due to invalid IL or missing references)
		if (!releaseReserved && _handle.IsValid())
		{
			Addressables.Release<GameObject>(_handle);
		}
	}
}
