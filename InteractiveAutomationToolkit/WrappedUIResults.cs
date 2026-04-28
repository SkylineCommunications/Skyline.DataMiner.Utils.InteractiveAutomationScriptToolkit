namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Runtime.InteropServices;

	using Skyline.DataMiner.Automation;

	/// <summary>
	/// A class wrapping <see cref="UIResults"/> in order to expose an interface to allow better unit testing.
	/// </summary>
	public class WrappedUIResults : IUIResults
	{
		private readonly UIResults results;

		/// <summary>
		/// Creates a new instance of <see cref="WrappedUIResults"/> wrapping the given UIResults.
		/// </summary>
		/// <param name="results"></param>
		/// <exception cref="ArgumentNullException"></exception>
		public WrappedUIResults(UIResults results)
		{
			this.results = results ?? throw new ArgumentNullException(nameof(results));
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public string GetString(string key)
		{
			return results.GetString(key);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public string GetUploadedFilePath(string key)
		{
			return results.GetUploadedFilePath(key);
		}

		public string[] GetUploadedFilePaths(string key)
		{
			return results.GetUploadedFilePaths(key);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		public bool WasButtonPressed(string key)
		{
			return results.WasButtonPressed(key);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		//     mode" checkbox.
		[return: MarshalAs(UnmanagedType.U1)]
		public bool WasBack()
		{
			return results.WasBack();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		public bool WasForward()
		{
			return results.WasForward();
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		public bool WasOnChange(string key)
		{
			return results.WasOnChange(key);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		public bool WasOnFocusLost(string key)
		{
			return results.WasOnFocusLost(key);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		public bool WasOnDownloadStarted(string key)
		{
			return results.WasOnDownloadStarted(key);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		public bool GetChecked(string key)
		{
			return results.GetChecked(key);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		[return: MarshalAs(UnmanagedType.U1)]
		public bool GetChecked(string key, string value)
		{
			return results.GetChecked(key, value);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public DateTime GetDateTime(string key)
		{
			return results.GetDateTime(key);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public string[] GetExpanded(string key)
		{
			return results.GetExpanded(key);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public TimeZoneInfo GetClientTimeZoneInfo(string key)
		{
			return results.GetClientTimeZoneInfo(key);
		}

		/// <summary>
		/// <inheritdoc/>
		/// </summary>
		public DateTimeOffset GetClientDateTime(string key)
		{
			return results.GetClientDateTime(key);
		}
	}
}
