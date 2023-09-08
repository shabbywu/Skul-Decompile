using Level;
using TMPro;
using UnityEngine;

namespace UI;

public class RemainEnemiesDisplay : MonoBehaviour
{
	[SerializeField]
	private TextMeshProUGUI _amount;

	[SerializeField]
	private GameObject _container;

	private int _count;

	private void Update()
	{
		if ((Object)(object)Map.Instance == (Object)null || (Object)(object)Map.Instance.waveContainer == (Object)null || (Map.Instance.waveContainer.enemyWaves.Length == 0 && Map.Instance.waveContainer.summonEnemyWave.characters.Count == 0))
		{
			if (_container.gameObject.activeSelf)
			{
				_container.gameObject.SetActive(false);
			}
			return;
		}
		if (!_container.gameObject.activeSelf)
		{
			_container.gameObject.SetActive(true);
		}
		int num = 0;
		EnemyWave[] enemyWaves = Map.Instance.waveContainer.enemyWaves;
		foreach (EnemyWave enemyWave in enemyWaves)
		{
			if (enemyWave.state == Wave.State.Spawned)
			{
				num += enemyWave.characters.Count;
			}
		}
		num += Map.Instance.waveContainer.summonEnemyWave.characters.Count;
		if (_count != num)
		{
			_count = num;
			((TMP_Text)_amount).text = _count.ToString();
		}
	}
}
