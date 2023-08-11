using System.Collections;
using Characters.Actions;
using UnityEngine;

namespace Characters.Gear.Weapons;

public class Prisoner2 : MonoBehaviour
{
	[SerializeField]
	private Action _openCursedChest;

	[SerializeField]
	private Action _openChest;

	public IEnumerator COpenCursedChest()
	{
		_openCursedChest.TryStart();
		while (_openCursedChest.running)
		{
			yield return null;
		}
	}

	public IEnumerator COpenChest()
	{
		_openChest.TryStart();
		while (_openChest.running)
		{
			yield return null;
		}
	}
}
