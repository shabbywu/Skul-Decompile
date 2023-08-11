using System.Collections;

namespace Level.Objects.DecorationCharacter.Command;

public interface ICommand
{
	IEnumerator CRun();
}
