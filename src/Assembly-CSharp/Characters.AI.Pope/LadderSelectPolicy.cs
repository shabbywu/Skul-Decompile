using System.Collections.Generic;
using UnityEngine;

namespace Characters.AI.Pope;

public abstract class LadderSelectPolicy : MonoBehaviour
{
	[SerializeField]
	private Transform _spawnPointContainer;

	private FanaticLadder[] _fanaticLadders;

	private void Awake()
	{
		//IL_002c: Unknown result type (might be due to invalid IL or missing references)
		//IL_0032: Expected O, but got Unknown
		_fanaticLadders = new FanaticLadder[_spawnPointContainer.childCount];
		int num = 0;
		foreach (Transform item in _spawnPointContainer)
		{
			Transform val = item;
			_fanaticLadders[num++] = ((Component)val).GetComponent<FanaticLadder>();
		}
	}

	public IEnumerator<FanaticLadder> GetLadders()
	{
		return SelectLadders().GetEnumerator() as IEnumerator<FanaticLadder>;
	}

	public abstract FanaticLadder[] SelectLadders();
}
