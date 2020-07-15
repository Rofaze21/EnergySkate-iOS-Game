#region copyright
//------------------------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov - focus [http://codestage.net]
//------------------------------------------------------------------------
#endregion

namespace CodeStage.Maintainer
{
	/// <summary>
	/// Location of the record-based results item.
	/// </summary>
	public enum RecordLocation : byte
	{
		Unknown = 0,
		Scene = 5,
		Asset = 7,
		Prefab = 10
	}
}