namespace DPek.Raconteur.RenPy.Util
{
	/// <summary>
	/// Defines the alignment of an item in RenPy.
	/// </summary>
	public enum RenPyAlignment
	{
		BottomCenter, BottomLeft, BottomRight, Center, LeftCenter, RightCenter,
		TopCenter, TopLeft, TopRight
	}

	public class RenPyAlignmentConverter
	{
		public static RenPyAlignment FromString(string str)
		{
			switch (str)
			{
				case "center":
					return RenPyAlignment.BottomCenter;
				case "left":
					return RenPyAlignment.BottomLeft;
				case "right":
					return RenPyAlignment.BottomRight;
				case "truecenter":
					return RenPyAlignment.Center;
			}
			UnityEngine.Debug.LogError("Unrecognized alignment string \"" + str + "\"");
			return RenPyAlignment.BottomCenter;
		}
	}
}
