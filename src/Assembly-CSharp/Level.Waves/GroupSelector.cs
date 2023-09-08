using System.Collections.Generic;
using Characters;
using UnityEngine;

namespace Level.Waves;

public abstract class GroupSelector : MonoBehaviour
{
	public abstract ICollection<Character> Load();
}
