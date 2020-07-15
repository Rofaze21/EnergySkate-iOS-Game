#region copyright
//------------------------------------------------------------------------
// Copyright (C) Dmitriy Yukhanov - focus [http://codestage.net]
//------------------------------------------------------------------------
#endregion

namespace CodeStage.Maintainer.Issues
{
	/// <summary>
	/// Describes the level of severity for the found issue.
	/// </summary>
	public enum RecordSeverity : byte
	{
		Info = 5,
		Warning = 10,
		Error = 15
	}
}