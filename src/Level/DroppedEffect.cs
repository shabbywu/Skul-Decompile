using UnityEngine;

namespace Level;

public class DroppedEffect : MonoBehaviour
{
	[SerializeField]
	private GameObject[] _legendary;

	[SerializeField]
	private GameObject[] _omen;

	public void SpawnLegendaryEffect()
	{
		for (int i = 0; i < _legendary.Length; i++)
		{
			_legendary[i].SetActive(true);
		}
	}

	public void SpawnOmenEffect()
	{
		for (int i = 0; i < _omen.Length; i++)
		{
			_omen[i].SetActive(true);
		}
	}

	public void Despawn()
	{
		for (int i = 0; i < _legendary.Length; i++)
		{
			_legendary[i].SetActive(false);
		}
		for (int j = 0; j < _omen.Length; j++)
		{
			_omen[j].SetActive(false);
		}
	}
}
