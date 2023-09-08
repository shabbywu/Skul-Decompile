using Level;
using UnityEngine;

namespace Characters.Operations;

public sealed class Instantiate : CharacterOperation
{
	[SerializeField]
	private GameObject _prefab;

	[SerializeField]
	private Transform _spawnPosition;

	[SerializeField]
	private float _lifeTime;

	public override void Run(Character owner)
	{
		//IL_000c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0011: Unknown result type (might be due to invalid IL or missing references)
		GameObject obj = Object.Instantiate<GameObject>(_prefab, _spawnPosition.position, Quaternion.identity);
		obj.transform.SetParent(((Component)Map.Instance).transform);
		Object.Destroy((Object)(object)obj, _lifeTime);
	}
}
