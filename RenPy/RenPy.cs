using UnityEngine;
using Exodrifter.Raconteur.RenPy;

public class RenPy : MonoBehaviour
{
	public RenPyScriptAsset script;

	void Start()
	{
		RenPyParser.Parse(script);
	}
}
