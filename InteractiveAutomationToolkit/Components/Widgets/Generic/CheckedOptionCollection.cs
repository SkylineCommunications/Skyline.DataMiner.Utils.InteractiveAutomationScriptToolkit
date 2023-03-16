namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	///     Collection of checked options in a <see cref="ICheckBoxList{T}" />.
	/// </summary>
	/// <typeparam name="T">The type of the value of the options.</typeparam>
	public class CheckedOptionCollection<T> : ICollection<Option<T>>, IReadOnlyCollection<Option<T>>
	{
		private readonly HashSet<Option<T>> checkedOptions = new HashSet<Option<T>>();
		private readonly CheckBoxList<T> checkBoxList;

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckedOptionCollection{T}" /> class.
		/// </summary>
		/// <param name="checkBoxList">The checkbox list widget for this collection.</param>
		public CheckedOptionCollection(CheckBoxList<T> checkBoxList)
		{
			this.checkBoxList = checkBoxList;
			Names = new CheckedNameCollection<T>(checkBoxList);
			Values = new CheckedValueCollection<T>(checkBoxList);
		}

		/// <inheritdoc cref="ICollection{T}.Count" />
		public int Count => checkedOptions.Count;

		/// <inheritdoc />
		public bool IsReadOnly => false;

		/// <summary>
		/// 	Gets the collection of names that have been selected in the <see cref="CheckBoxList{T}"/>.
		/// </summary>
		public CheckedNameCollection<T> Names { get; }

		/// <summary>
		/// 	Gets the collection of values that have been selected in the <see cref="CheckBoxList{T}"/>.
		/// </summary>
		public CheckedValueCollection<T> Values { get; }

		/// <inheritdoc />
		public void Add(Option<T> item)
		{
			if (checkBoxList.Options.Contains(item) && checkedOptions.Add(item))
			{
				checkBoxList.BlockDefinition.InitialValue = String.Join(";", Names);
			}
		}

		/// <summary>
		///     Checks all options of the specified collection.
		/// </summary>
		/// <param name="options">The collections of options to be checked.</param>
		public void AddRange(IEnumerable<Option<T>> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			foreach (Option<T> option in options)
			{
				Add(option);
			}
		}

		/// <inheritdoc />
		public void Clear()
		{
			checkedOptions.Clear();
			checkBoxList.BlockDefinition.InitialValue = String.Empty;
		}

		/// <inheritdoc />
		public bool Contains(Option<T> item)
		{
			return checkedOptions.Contains(item);
		}

		/// <inheritdoc />
		public void CopyTo(Option<T>[] array, int arrayIndex)
		{
			checkedOptions.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc />
		public IEnumerator<Option<T>> GetEnumerator()
		{
			return checkedOptions.GetEnumerator();
		}

		/// <inheritdoc />
		public bool Remove(Option<T> item)
		{
			if (checkedOptions.Remove(item))
			{
				checkBoxList.BlockDefinition.InitialValue = String.Join(";", Names);
				return true;
			}

			return false;
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)checkedOptions).GetEnumerator();
		}

		/// <summary>
		/// 	Represents a collection of names that have been selected in a <see cref="CheckBoxList{T}"/>.
		/// </summary>
		/// <typeparam name="TValue">The type of the values of the options.</typeparam>
		public class CheckedNameCollection<TValue> : ICollection<string>, IReadOnlyCollection<string>
		{
			private readonly CheckBoxList<TValue> checkBoxList;

			/// <summary>
			/// Initializes a new instance of the <see cref="CheckedNameCollection{TValue}"/> class.
			/// </summary>
			/// <param name="checkBoxList">The checkbox list widget for this collection.</param>
			public CheckedNameCollection(CheckBoxList<TValue> checkBoxList) => this.checkBoxList = checkBoxList;

			/// <inheritdoc cref="ICollection{T}.Count" />
			public int Count => checkBoxList.CheckedOptions.Count;

			/// <inheritdoc/>
			public bool IsReadOnly => checkBoxList.CheckedOptions.IsReadOnly;

			/// <inheritdoc/>
			public void Add(string item)
			{
				int index = checkBoxList.Options.IndexOfName(item);
				if (index == -1)
				{
					return;
				}

				checkBoxList.CheckedOptions.Add(checkBoxList.Options[index]);
			}

			/// <summary>
			/// 	Adds all options with specified names to the collection of checked options.
			/// </summary>
			/// <param name="names">The collections of names of options to be checked.</param>
			public void AddRange(IEnumerable<string> names)
			{
				if (names == null)
				{
					throw new ArgumentNullException(nameof(names));
				}

				foreach (string text in names)
				{
					int index = checkBoxList.Options.IndexOfName(text);
					if (index == -1)
					{
						continue;
					}

					checkBoxList.CheckedOptions.Add(checkBoxList.Options[index]);
				}
			}

			/// <inheritdoc/>
			public void Clear()
			{
				checkBoxList.CheckedOptions.Clear();
			}

			/// <inheritdoc/>
			public bool Contains(string item)
			{
				return checkBoxList.CheckedOptions.Any(option => option.Name == item);
			}

			/// <inheritdoc/>
			public void CopyTo(string[] array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException(nameof(array));
				}

				if (arrayIndex < 0 || arrayIndex > array.Length)
				{
					throw new ArgumentOutOfRangeException(nameof(arrayIndex));
				}

				if (array.Length - arrayIndex < Count)
				{
					throw new ArgumentException("arrayIndex cannot be greater or equal than the Count of the source.");
				}

				foreach (string name in this)
				{
					array[arrayIndex++] = name;
				}
			}

			/// <inheritdoc/>
			public IEnumerator<string> GetEnumerator()
			{
				return checkBoxList.CheckedOptions.Select(option => option.Name).GetEnumerator();
			}

			/// <inheritdoc/>
			public bool Remove(string item)
			{
				int index = checkBoxList.Options.IndexOfName(item);
				if (index == -1)
				{
					return false;
				}

				return checkBoxList.CheckedOptions.Remove(checkBoxList.Options[index]);
			}

			/// <inheritdoc/>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		/// <summary>
		/// 	Represents a collection of values that have been selected in a <see cref="CheckBoxList{T}"/>.
		/// </summary>
		/// <typeparam name="TValue">The type of the values.</typeparam>
		public class CheckedValueCollection<TValue> : ICollection<TValue>
		{
			private readonly CheckBoxList<TValue> checkBoxList;

			/// <summary>
			/// Initializes a new instance of the <see cref="CheckedValueCollection{TValue}"/> class.
			/// </summary>
			/// <param name="checkBoxList">The checkbox list widget for this collection.</param>
			public CheckedValueCollection(CheckBoxList<TValue> checkBoxList) => this.checkBoxList = checkBoxList;

			/// <inheritdoc/>
			public int Count => checkBoxList.CheckedOptions.Count;

			/// <inheritdoc/>
			public bool IsReadOnly => checkBoxList.CheckedOptions.IsReadOnly;

			/// <inheritdoc/>
			public void Add(TValue item)
			{
				int index = checkBoxList.Options.IndexOfValue(item);
				if (index == -1)
				{
					return;
				}

				checkBoxList.CheckedOptions.Add(checkBoxList.Options[index]);
			}

			/// <summary>
			/// 	Adds all options with specified values to the collection of checked options.
			/// </summary>
			/// <param name="values">The collections of names of options to be checked.</param>
			public void AddRange(IEnumerable<TValue> values)
			{
				if (values == null)
				{
					throw new ArgumentNullException(nameof(values));
				}

				foreach (TValue value in values)
				{
					int index = checkBoxList.Options.IndexOfValue(value);
					if (index == -1)
					{
						continue;
					}

					checkBoxList.CheckedOptions.Add(checkBoxList.Options[index]);
				}
			}

			/// <inheritdoc/>
			public void Clear()
			{
				checkBoxList.CheckedOptions.Clear();
			}

			/// <inheritdoc/>
			public bool Contains(TValue item)
			{
				return checkBoxList.CheckedOptions.Any(option => EqualityComparer<TValue>.Default.Equals(option.Value, item));
			}

			/// <inheritdoc/>
			public void CopyTo(TValue[] array, int arrayIndex)
			{
				if (array == null)
				{
					throw new ArgumentNullException(nameof(array));
				}

				if (arrayIndex < 0 || arrayIndex > array.Length)
				{
					throw new ArgumentOutOfRangeException(nameof(arrayIndex));
				}

				if (array.Length - arrayIndex < Count)
				{
					throw new ArgumentException("arrayIndex cannot be greater or equal than the Count of the source.");
				}

				foreach (TValue value in this)
				{
					array[arrayIndex++] = value;
				}
			}

			/// <inheritdoc/>
			public IEnumerator<TValue> GetEnumerator()
			{
				return checkBoxList.CheckedOptions.Select(option => option.Value).GetEnumerator();
			}

			/// <inheritdoc/>
			public bool Remove(TValue item)
			{
				int index = checkBoxList.Options.IndexOfValue(item);
				if (index == -1)
				{
					return false;
				}

				return checkBoxList.CheckedOptions.Remove(checkBoxList.Options[index]);
			}

			/// <inheritdoc/>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}