using Services;
using Singletons;
using UnityEngine;

public class FollowGameObejct : MonoBehaviour
{
	private enum Type
	{
		Camera,
		Player,
		CustomGameObject
	}

	[SerializeField]
	private Type _followObejct;

	[SerializeField]
	private GameObject _followGameObject;

	[SerializeField]
	private Vector3 _offset;

	public void Start()
	{
		switch (_followObejct)
		{
		case Type.Camera:
			_followGameObject = ((Component)Camera.main).gameObject;
			break;
		case Type.Player:
			_followGameObject = ((Component)Singleton<Service>.Instance.levelManager.player).gameObject;
			break;
		case Type.CustomGameObject:
			break;
		}
	}

	public void Update()
	{
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		//IL_0017: Unknown result type (might be due to invalid IL or missing references)
		//IL_001c: Unknown result type (might be due to invalid IL or missing references)
		((Component)this).transform.position = _followGameObject.transform.position + _offset;
	}
}
