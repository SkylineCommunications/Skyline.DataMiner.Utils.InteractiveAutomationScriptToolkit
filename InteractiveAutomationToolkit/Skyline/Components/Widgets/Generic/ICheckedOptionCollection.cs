namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System.Collections.Generic;

	/// <summary>
	/// 	Represents a collection of options that are checked in a <see cref="CheckBoxList{T}"/>.
	/// </summary>
	/// <typeparam name="TValue">The type of the values of the options.</typeparam>
	public interface ICheckedOptionCollection<TValue> : ICollection<Option<TValue>>
	{
		/// <summary>
		/// 	Gets the collection of names that have been selected in the <see cref="CheckBoxList{T}"/>.
		/// </summary>
		ICheckedNameCollection Names { get; }

		/// <summary>
		/// 	Gets the collection of values that have been selected in the <see cref="CheckBoxList{T}"/>.
		/// </summary>
		ICheckedValueCollection<TValue> Values { get; }

		/// <summary>
		/// 	Adds all specified options to the collection of checked options.
		/// </summary>
		/// <param name="options">The collections of options to be checked.</param>
		void AddRange(IEnumerable<Option<TValue>> options);
	}

	/// <summary>
	/// 	Represents a collection of names that have been selected in a <see cref="CheckBoxList{T}"/>.
	/// </summary>
	public interface ICheckedNameCollection : ICollection<string>
	{
		/// <summary>
		/// 	Adds all options with specified names to the collection of checked options.
		/// </summary>
		/// <param name="names">The collections of names of options to be checked.</param>
		void AddRange(IEnumerable<string> names);
	}

	/// <summary>
	/// 	Represents a collection of values that have been selected in a <see cref="CheckBoxList{T}"/>.
	/// </summary>
	/// <typeparam name="TValue">The type of the values.</typeparam>
	public interface ICheckedValueCollection<TValue> : ICollection<TValue>
	{
		/// <summary>
		/// 	Adds all options with specified values to the collection of checked options.
		/// </summary>
		/// <param name="values">The collections of names of options to be checked.</param>
		void AddRange(IEnumerable<TValue> values);
	}
}