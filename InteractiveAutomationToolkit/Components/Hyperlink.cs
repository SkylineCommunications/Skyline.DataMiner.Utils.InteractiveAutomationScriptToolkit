namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Linq;
	using Microsoft.Extensions.Logging;
	using Skyline.DataMiner.Automation;
	using Skyline.DataMiner.Utils.InteractiveAutomationScript.Extensions;

	/// <summary>
	///		Represents a button that when clicked forwards the user to a web site.
	/// </summary>
	public class Hyperlink : InteractiveWidget
	{
		private AutomationDownloadButtonOptions downloadButtonOptions;
		private bool downloadStarted;
		private ButtonStyle style;

		/// <summary>
		///		Creates a new instance of a <see cref="Hyperlink" /> button where the link is displayed.
		/// </summary>
		/// <param name="url">Link to which the user is forwarded to upon clicking the button.</param>
		public Hyperlink(Uri url) : this(url.ToString(), url)
		{
		}

		/// <summary>
		///		Creates a new instance of a <see cref="Hyperlink" /> button where the displayedText is displayed.
		/// </summary>
		/// <param name="displayedText">Text to be displayed on the button.</param>
		/// <param name="url">Link to which the user is forwarded to upon clicking the button.</param>
		public Hyperlink(string displayedText, Uri url)
		{
			Type = UIBlockType.DownloadButton;
			DownloadButtonOptions = new AutomationDownloadButtonOptions
			{
				FileNameToSave = String.Empty,
				StartDownloadImmediately = false
			};

			DisplayedText = displayedText;
			Url = url;
		}

		/// <summary>
		///		Triggered when the <see cref="Hyperlink" /> is clicked.
		/// </summary>
		public event EventHandler<EventArgs> LinkClicked
		{
			add
			{
				OnLinkClicked += value;
				DownloadButtonOptions.ReturnWhenDownloadIsStarted = true;
			}

			remove
			{
				OnLinkClicked -= value;
				if (OnLinkClicked == null || !OnLinkClicked.GetInvocationList().Any())
				{
					DownloadButtonOptions.ReturnWhenDownloadIsStarted = false;
				}
			}
		}

		private event EventHandler<EventArgs> OnLinkClicked;

		/// <summary>
		///     Gets or sets the <see cref="ButtonStyle"/> of the <see cref="Hyperlink" />.
		/// </summary>
		public ButtonStyle Style
		{
			get
			{
				return style;
			}

			set
			{
				style = value;
				BlockDefinition.Style = ButtonStyleConverter.StyleToUiString(value);
			}
		}

		/// <summary>
		///     Gets or sets the text displayed in the <see cref="Hyperlink" />.
		/// </summary>
		public string DisplayedText
		{
			get
			{
				return BlockDefinition.Text;
			}

			set
			{
				BlockDefinition.Text = value;
			}
		}

		/// <summary>
		///		Gets or sets the url to which the user is forwarded to upon clicking the <see cref="Hyperlink" />.
		///		This url can be either absolute or relative.
		///		Absolute: "http://www.contoso.com/index.html"
		///		Relative: "/index.html"
		/// </summary>
		public Uri Url
		{
			get
			{
				if (Uri.TryCreate(DownloadButtonOptions.Url, UriKind.RelativeOrAbsolute, out Uri result)) return result;
				return null;
			}

			set
			{
				if (value == null) throw new ArgumentNullException("value");
				DownloadButtonOptions.Url = value.ToString();
			}
		}

		/// <summary>
		///		Gets or sets the configuration of this <see cref="AutomationDownloadButtonOptions" /> instance.
		/// </summary>
		private AutomationDownloadButtonOptions DownloadButtonOptions
		{
			get
			{
				return downloadButtonOptions;
			}

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}

				downloadButtonOptions = value;
				BlockDefinition.ConfigOptions = value;
			}
		}

		/// <inheritdoc	/>
		protected internal override void LoadResult(IUIResults uiResults, ILogger logger = null)
		{
			if (DownloadButtonOptions.ReturnWhenDownloadIsStarted)
			{
				downloadStarted = uiResults.WasHyperlinkOpened(this);
			}
		}

		/// <inheritdoc	/>
		protected internal override void RaiseResultEvents(ILogger logger = null)
		{
			if (downloadStarted)
			{
				logger?.Debug(nameof(Hyperlink), nameof(RaiseResultEvents), $"OnLinkClicked");
				OnLinkClicked?.Invoke(this, EventArgs.Empty);
			}

			downloadStarted = false;
		}
	}
}
