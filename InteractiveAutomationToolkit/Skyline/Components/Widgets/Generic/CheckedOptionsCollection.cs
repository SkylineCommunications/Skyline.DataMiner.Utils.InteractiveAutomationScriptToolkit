namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	/// <summary>
	///     Collection of checked options in a <see cref="ICheckBoxList{T}" />.
	/// </summary>
	/// <typeparam name="T">The type of the value of the options.</typeparam>
	public class CheckedOptionCollection<T> : ICheckedOptionCollection<T>
	{
		private readonly HashSet<Option<T>> checkedOptions = new HashSet<Option<T>>();
		private readonly IOptionsList<T> optionsList;

		/// <summary>
		///     Initializes a new instance of the <see cref="CheckedOptionCollection{T}" /> class.
		/// </summary>
		/// <param name="optionsList">The options list of the checkbox list.</param>
		public CheckedOptionCollection(IOptionsList<T> optionsList)
		{
			this.optionsList = optionsList;
			Names = new CheckedNameCollection<T>(this);
			Values = new CheckedValueCollection<T>(this);
		}

		/// <inheritdoc cref="ICollection{T}.Count" />
		public int Count => checkedOptions.Count;

		/// <inheritdoc />
		public bool IsReadOnly => false;

		/// <inheritdoc/>
		public ICheckedNameCollection Names { get; }

		/// <inheritdoc/>
		public ICheckedValueCollection<T> Values { get; }

		/// <inheritdoc />
		public void Add(Option<T> item)
		{
			if (optionsList.Contains(item))
			{
				checkedOptions.Add(item);
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
			return checkedOptions.Remove(item);
		}

		/// <inheritdoc />
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)checkedOptions).GetEnumerator();
		}

		private class CheckedNameCollection<TValue> : ICheckedNameCollection
		{
			private readonly CheckedOptionCollection<TValue> @checked;

			public CheckedNameCollection(CheckedOptionCollection<TValue> @checked) => this.@checked = @checked;

			/// <inheritdoc/>
			public int Count => @checked.Count;

			/// <inheritdoc/>
			public bool IsReadOnly => @checked.IsReadOnly;

			/// <inheritdoc/>
			public void Add(string item)
			{
				int index = @checked.optionsList.IndexOfName(item);
				if (index == -1)
				{
					return;
				}

				@checked.checkedOptions.Add(@checked.optionsList[index]);
			}

			/// <inheritdoc/>
			public void AddRange(IEnumerable<string> names)
			{
				if (names == null)
				{
					throw new ArgumentNullException(nameof(names));
				}

				foreach (string text in names)
				{
					int index = @checked.optionsList.IndexOfName(text);
					if (index == -1)
					{
						continue;
					}

					@checked.checkedOptions.Add(@checked.optionsList[index]);
				}
			}

			/// <inheritdoc/>
			public void Clear()
			{
				@checked.Clear();
			}

			/// <inheritdoc/>
			public bool Contains(string item)
			{
				return @checked.optionsList.Any(option => option.Name == item);
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
				return @checked.Select(option => option.Name).GetEnumerator();
			}

			/// <inheritdoc/>
			public bool Remove(string item)
			{
				int index = @checked.optionsList.IndexOfName(item);
				if (index == -1)
				{
					return false;
				}

				return @checked.Remove(@checked.optionsList[index]);
			}

			/// <inheritdoc/>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}

		private class CheckedValueCollection<TValue> : ICheckedValueCollection<TValue>
		{
			private readonly CheckedOptionCollection<TValue> @checked;

			public CheckedValueCollection(CheckedOptionCollection<TValue> @checked) => this.@checked = @checked;

			/// <inheritdoc/>
			public int Count => @checked.Count;

			/// <inheritdoc/>
			public bool IsReadOnly => @checked.IsReadOnly;

			/// <inheritdoc/>
			public void Add(TValue item)
			{
				int index = @checked.optionsList.IndexOfValue(item);
				if (index == -1)
				{
					return;
				}

				@checked.checkedOptions.Add(@checked.optionsList[index]);
			}

			/// <inheritdoc/>
			public void AddRange(IEnumerable<TValue> values)
			{
				if (values == null)
				{
					throw new ArgumentNullException(nameof(values));
				}

				foreach (TValue value in values)
				{
					int index = @checked.optionsList.IndexOfValue(value);
					if (index == -1)
					{
						continue;
					}

					@checked.checkedOptions.Add(@checked.optionsList[index]);
				}
			}

			/// <inheritdoc/>
			public void Clear()
			{
				@checked.Clear();
			}

			/// <inheritdoc/>
			public bool Contains(TValue item)
			{
				return @checked.optionsList.Any(option => EqualityComparer<TValue>.Default.Equals(option.Value, item));
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
				return @checked.Select(option => option.Value).GetEnumerator();
			}

			/// <inheritdoc/>
			public bool Remove(TValue item)
			{
				int index = @checked.optionsList.IndexOfValue(item);
				if (index == -1)
				{
					return false;
				}

				return @checked.Remove(@checked.optionsList[index]);
			}

			/// <inheritdoc/>
			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}
		}
	}
}