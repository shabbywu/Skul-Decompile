using Services;
using Singletons;
using UnityEngine;

public class StickPlayerPosition : MonoBehaviour
{
	[SerializeField]
	private Vector3 _offset;

	private GameObject _playerObject;

	public void Start()
	{
		_playerObject = ((Component)Singleton<Service>.Instance.levelManager.player).gameObject;
	}

	public void Update()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.position = _playerObject.transform.position + _offset;
	}
}
