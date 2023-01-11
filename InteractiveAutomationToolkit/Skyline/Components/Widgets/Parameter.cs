namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;

	using Skyline.DataMiner.Automation;

	/// <summary>
	///     Displays the value of a protocol parameter.
	/// </summary>
	public class Parameter : Widget, IParameter
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
		public Parameter(IActionableElement element, int parameterId, string index = null)
			: this(
				element.DmaId,
				element.ElementId,
				parameterId,
				index)
		{
		}

		/// <inheritdoc />
		public int DmaId
		{
			get => dmaId;

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				dmaId = value;
				BlockDefinition.Extra = GenerateExtra();
			}
		}

		/// <inheritdoc />
		public int ElementId
		{
			get => elementId;

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				elementId = value;
				BlockDefinition.Extra = GenerateExtra();
			}
		}

		/// <inheritdoc />
		public string Index
		{
			get => index;

			set
			{
				index = value;
				BlockDefinition.Extra = GenerateExtra();
			}
		}

		/// <inheritdoc />
		public int ParameterId
		{
			get => parameterId;

			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException(nameof(value));
				}

				parameterId = value;
				BlockDefinition.Extra = GenerateExtra();
			}
		}

		private string GenerateExtra()
		{
			return $"{dmaId}/{elementId}:{parameterId}:{index}";
		}
	}
}