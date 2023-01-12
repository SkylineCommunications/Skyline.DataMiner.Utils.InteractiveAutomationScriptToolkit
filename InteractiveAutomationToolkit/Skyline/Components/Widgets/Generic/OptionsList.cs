namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <inheritdoc cref="IOptionsList{T}" />
	internal class OptionsList<T> : IOptionsList<T>
	{
		private readonly IList<string> names;
		private readonly IList<T> values = new List<T>();

		/// <summary>
		/// Initializes a new instance of the <see cref="OptionsList{T}"/> class.
		/// </summary>
		/// <param name="widgetOptionsCollection">The options list from the widget's <see cref="UIBlockDefinition"/>.</param>
		public OptionsList(IList<string> widgetOptionsCollection) => names = widgetOptionsCollection;

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

		/// <inheritdoc/>
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

			Add(item.Name, item.Value);
		}

		/// <inheritdoc/>
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

		/// <inheritdoc cref="IOptionsList{T}.ContainsName" />
		public bool ContainsName(string name)
		{
			return names.Contains(name);
		}

		/// <inheritdoc cref="IOptionsList{T}.ContainsValue" />
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

		/// <inheritdoc cref="IOptionsList{T}.IndexOfName" />
		public int IndexOfName(string name)
		{
			return names.IndexOf(name);
		}

		/// <inheritdoc cref="IOptionsList{T}.IndexOfValue" />
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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

		/// <inheritdoc/>
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
				throw new InvalidOperationException($"Collection already contains name: {name}");
			}

			if (!EqualityComparer<T>.Default.Equals(value, replaced.Value) && values.Contains(value))
			{
				throw new InvalidOperationException($"Collection already contains value: {value}");
			}
		}

		private void ThrowIfDuplicate(string name, T value)
		{
			if (names.Contains(name))
			{
				throw new InvalidOperationException($"Collection already contains name: {name}");
			}

			if (values.Contains(value))
			{
				throw new InvalidOperationException($"Collection already contains value: {value}");
			}
		}
	}
}