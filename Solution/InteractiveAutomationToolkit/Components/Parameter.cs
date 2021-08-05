namespace Skyline.DataMiner.DeveloperCommunityLibrary.InteractiveAutomationToolkit
{
	using System;
	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Displays the value of a protocol parameter.
	/// </summary>
	public class Parameter : Widget
	{
		private int dmaId;
		private int elementId;
		private string index;
		private int parameterId;

		/// <summary>
		///     Initializes a new instance of the <see cref="Parameter" /> class.
		/// </summary>
		/// <param name="dmaId">ID of the DataMiner Agent.</param>
		/// <param name="elementId">ID of the element.</param>
		/// <param name="parameterId">ID of the parameter.</param>
		/// <param name="index">Primary key of the table entry. Is null for standalone parameters.</param>
		public Parameter(int dmaId, int elementId, int parameterId, string index = null)
		{
			BlockDefinition.Type = UIBlockType.Parameter;
			DmaId = dmaId;
			ElementId = elementId;
			ParameterId = parameterId;
			Index = index;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="Parameter" /> class.
		/// </summary>
		/// <param name="element">Element that has the parameter.</param>
		/// <param name="parameterId">ID of the parameter.</param>
		/// <param name="index">Primary key of the table entry. Is null for standalone parameters.</param>
		public Parameter(Element element, int parameterId, string index = null) : this(
			element.DmaId,
			element.ElementId,
			parameterId,
			index)
		{
		}

		/// <summary>
		///     Gets or sets the ID of the DataMiner Agent that has the parameter.
		/// </summary>
		public int DmaId
		{
			get
			{
				return dmaId;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				dmaId = value;
				BlockDefinition.Extra = GenerateExtra();
			}
		}

		/// <summary>
		///     Gets or sets the ID of the element that has the parameter.
		/// </summary>
		public int ElementId
		{
			get
			{
				return elementId;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				elementId = value;
				BlockDefinition.Extra = GenerateExtra();
			}
		}

		/// <summary>
		///     Gets or sets the primary key of the table entry.
		/// </summary>
		/// <remarks>Should be <c>null</c> for standalone parameters.</remarks>
		public string Index
		{
			get
			{
				return index;
			}

			set
			{
				index = value;
				BlockDefinition.Extra = GenerateExtra();
			}
		}

		/// <summary>
		///     Gets or sets the ID of the parameter.
		/// </summary>
		public int ParameterId
		{
			get
			{
				return parameterId;
			}

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}

				parameterId = value;
				BlockDefinition.Extra = GenerateExtra();
			}
		}

		private string GenerateExtra()
		{
			return String.Format("{0}/{1}:{2}:{3}", dmaId, elementId, parameterId, index);
		}
	}
}
