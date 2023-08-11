using System.Collections.Generic;
using BehaviorDesigner.Runtime;
using Characters;
using UnityEngine;

namespace Level.Waves;

public sealed class Pin : MonoBehaviour
{
	[SerializeField]
	private Key _key;

	[SerializeField]
	private Enemy _enemy;

	[SerializeField]
	[ReadOnly(false)]
	private bool _lookLeft;

	private bool _spawned;

	private ICollection<Character> _characters;

	public bool spawned => _spawned;

	public ICollection<Character> characters => _characters;

	public ICollection<Character> Load()
	{
		//IL_002a: Unknown result type (might be due to invalid IL or missing references)
		//IL_002f: Unknown result type (might be due to invalid IL or missing references)
		Enemy enemy = _enemy;
		if ((Object)(object)enemy == (Object)null)
		{
			Debug.LogError((object)"Enemy is null");
			return null;
		}
		PinEnemySetting component = ((Component)this).GetComponent<PinEnemySetting>();
		Enemy enemy2 = Object.Instantiate<Enemy>(enemy, ((Component)this).transform.position, Quaternion.identity, ((Component)this).transform.parent);
		foreach (Behavior behavior in enemy2.behaviors)
		{
			behavior.Start();
			behavior.DisableBehavior(true);
		}
		foreach (Character character in enemy2.characters)
		{
			if (_lookLeft)
			{
				((Component)character).gameObject.AddComponent<LookLeft>();
			}
			if (Object.op_Implicit((Object)(object)component))
			{
				component.ApplyTo(character);
			}
		}
		_spawned = true;
		_characters = enemy2.characters;
		return enemy2.characters;
	}
}
