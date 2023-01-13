namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <summary>
	/// Represents a unique collection of option objects.
	/// </summary>
	/// <typeparam name="T">The type of the values of the options.</typeparam>
	public class OptionList<T> : IList<Option<T>>, IReadOnlyList<Option<T>>
	{
		private readonly IList<string> names;
		private readonly IList<T> values = new List<T>();

		/// <summary>
		/// Initializes a new instance of the <see cref="OptionList{T}"/> class.
		/// </summary>
		/// <param name="widgetOptionsCollection">The options list from the widget's <see cref="UIBlockDefinition"/>.</param>
		public OptionList(IList<string> widgetOptionsCollection) => names = widgetOptionsCollection;

		/// <inheritdoc cref="ICollection{T}.Count" />
		public int Count => names.Count;

		/// <inheritdoc/>
		public bool IsReadOnly => names.IsReadOnly;

		/// <inheritdoc cref="IList{T}.this" />
		public virtual Option<T> this[int index]
		{
			get => new Option<T>(names[index], values[index]);

			set
			{
				ThrowIfNameNull(value);
				ThrowIfDuplicate(value.Name, value.Value, this[index]);

				names[index] = value.Name;
				values[index] = value.Value;
			}
		}

		/// <summary>
		/// 	Adds an option to the <see cref="OptionList{T}"/>with the specified name and value.
		/// </summary>
		/// <param name="name">The name associated with the value.</param>
		/// <param name="value">The value in the name/value pair.</param>
		/// <exception cref="InvalidOperationException">
		/// if <paramref name="name"/> or <paramref name="value"/> is not unique.
		/// </exception>
		public virtual void Add(string name, T value)
		{
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			ThrowIfDuplicate(name, value);

			names.Add(name);
			values.Add(value);
		}

		/// <inheritdoc/>
		/// <exception cref="InvalidOperationException">
		/// if <see cref="Option{TValue}.Name"/> or <see cref="Option{TValue}.Value"/> of <paramref name="item"/> is not unique.
		/// </exception>
		public void Add(Option<T> item)
		{
			ThrowIfNameNull(item);

			// ReSharper disable once PossibleNullReferenceException
			// Check is performed in method above
			Add(item.Name, item.Value);
		}

		/// <summary>
		/// Adds the options of the specified collection to the end of the <see cref="OptionList{T}"/>.
		/// </summary>
		/// <param name="options">The collection whose options of the specified collection to the end of the <see cref="OptionList{T}"/>.</param>
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

		/// <inheritdoc/>
		public virtual void Clear()
		{
			names.Clear();
			values.Clear();
		}

		/// <summary>
		/// 	Determines whether an option with the specified name is in the <see cref="OptionList{T}"/>.
		/// </summary>
		/// <param name="name">The name to locate int the <see cref="OptionList{T}"/>.</param>
		/// <returns><c>true</c> if <paramref name="name"/> is found in the <see cref="OptionList{T}"/>; otherwise, <c>false</c>.</returns>
		public bool ContainsName(string name)
		{
			return names.Contains(name);
		}

		/// <summary>
		/// 	Determines whether an option with the specified value is in the <see cref="OptionList{T}"/>.
		/// </summary>
		/// <param name="value">The value to locate int the <see cref="OptionList{T}"/>.</param>
		/// <returns><c>true</c> if <paramref name="value"/> is found in the <see cref="OptionList{T}"/>; otherwise, <c>false</c>.</returns>
		public bool ContainsValue(T value)
		{
			return values.Contains(value);
		}

		/// <inheritdoc/>
		public bool Contains(Option<T> item)
		{
			if (item == null)
			{
				return false;
			}

			int indexOfName = names.IndexOf(item.Name);
			int indexOfValue = values.IndexOf(item.Value);

			return indexOfName == indexOfValue && indexOfName != -1;
		}

		/// <inheritdoc/>
		public void CopyTo(Option<T>[] array, int arrayIndex)
		{
			for (var i = 0; i < names.Count; i++)
			{
				array[i + arrayIndex] = new Option<T>(names[i], values[i]);
			}
		}

		/// <inheritdoc/>
		public IEnumerator<Option<T>> GetEnumerator()
		{
			return names.Zip(values, (name, value) => new Option<T>(name, value)).GetEnumerator();
		}

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// 	Searches for the specified name and returns the zero-based index of the first occurrence within the entire <see cref="OptionList{T}"/>.
		/// </summary>
		/// <param name="name">The name to locate in the <see cref="OptionList{T}"/>.</param>
		/// <returns>the zero-based index of the first occurrence within the entire <see cref="OptionList{T}"/>, if found; otherwise -1.</returns>
		public int IndexOfName(string name)
		{
			return names.IndexOf(name);
		}

		/// <summary>
		/// 	Searches for the specified value and returns the zero-based index of the first occurrence within the entire <see cref="OptionList{T}"/>.
		/// </summary>
		/// <param name="value">The value to locate in the <see cref="OptionList{T}"/>.</param>
		/// <returns>the zero-based index of the first occurrence within the entire <see cref="OptionList{T}"/>, if found; otherwise -1.</returns>
		public int IndexOfValue(T value)
		{
			return values.IndexOf(value);
		}

		/// <inheritdoc/>
		public int IndexOf(Option<T> item)
		{
			if (item == null)
			{
				return -1;
			}

			int indexOfName = names.IndexOf(item.Name);
			int indexOfValue = values.IndexOf(item.Value);

			return indexOfName == indexOfValue ? indexOfName : -1;
		}

		/// <summary>
		/// 	Inserts an option with the specified name and value to the <see cref="OptionList{T}"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which the option should be inserted.</param>
		/// <param name="name">The name associated with the value.</param>
		/// <param name="value">The value in the name/value pair.</param>
		public virtual void Insert(int index, string name, T value)
		{
			if (name == null)
			{
				throw new ArgumentNullException(nameof(name));
			}

			ThrowIfDuplicate(name, value);

			names.Insert(index, name);
			values.Insert(index, value);
		}

		/// <inheritdoc/>
		public void Insert(int index, Option<T> item)
		{
			if (item == null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			Insert(index, item.Name, item.Value);
		}

		/// <inheritdoc/>
		public virtual void RemoveAt(int index)
		{
			names.RemoveAt(index);
			values.RemoveAt(index);
		}

		/// <summary>
		/// 	Removes the first occurrence of an option with specified name from the <see cref="OptionList{T}"/>.
		/// </summary>
		/// <param name="name">The option with the specified name to remove from the <see cref="OptionList{T}"/>.</param>
		/// <returns><c>true</c> if the option is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if the option was not found.</returns>
		public bool RemoveName(string name)
		{
			int index = names.IndexOf(name);

			if (index == -1)
			{
				return false;
			}

			RemoveAt(index);
			return true;
		}

		/// <summary>
		/// 	Removes the first occurrence of an option with specified value from the <see cref="OptionList{T}"/>.
		/// </summary>
		/// <param name="value">The option with the specified value to remove from the <see cref="OptionList{T}"/>.</param>
		/// <returns><c>true</c> if the option is successfully removed; otherwise, <c>false</c>. This method also returns <c>false</c> if the option was not found.</returns>
		public bool RemoveValue(T value)
		{
			int index = values.IndexOf(value);
			if (index == -1)
			{
				return false;
			}

			RemoveAt(index);
			return true;
		}

		/// <inheritdoc/>
		public bool Remove(Option<T> item)
		{
			if (item == null)
			{
				return false;
			}

			int indexOfName = names.IndexOf(item.Name);
			int indexOfValue = values.IndexOf(item.Value);

			if (indexOfName != indexOfValue || indexOfName == -1)
			{
				return false;
			}

			RemoveAt(indexOfName);
			return true;
		}

		private static void ThrowIfNameNull(Option<T> item)
		{
			if (item == null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			if (item.Name == null)
			{
				throw new ArgumentException("Name cannot be null.");
			}
		}

		private void ThrowIfDuplicate(string name, T value, Option<T> replaced)
		{
			if (name != replaced.Name && names.Contains(name))
			{
				throw new ArgumentException($"Collection already contains name: {name}");
			}

			if (!EqualityComparer<T>.Default.Equals(value, replaced.Value) && values.Contains(value))
			{
				throw new ArgumentException($"Collection already contains value: {value}");
			}
		}

		private void ThrowIfDuplicate(string name, T value)
		{
			if (names.Contains(name))
			{
				throw new ArgumentException($"Collection already contains name: {name}");
			}

			if (values.Contains(value))
			{
				throw new ArgumentException($"Collection already contains value: {value}");
			}
		}
	}
}