namespace Skyline.DataMiner.Utils.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	using Skyline.DataMiner.Automation;

	/// <summary>
	/// Represents a unique collection of options.
	/// </summary>
	public class OptionList : IList<string>, IReadOnlyList<string>
	{
		private readonly IList<string> options;

		/// <summary>
		/// Initializes a new instance of the <see cref="OptionList"/> class.
		/// </summary>
		/// <param name="widgetOptionsCollection">The options list from the widget's <see cref="UIBlockDefinition"/>.</param>
		public OptionList(IList<string> widgetOptionsCollection) => options = widgetOptionsCollection;

		/// <inheritdoc cref="ICollection{T}.Count" />
		public int Count => options.Count;

		/// <inheritdoc/>
		public bool IsReadOnly => options.IsReadOnly;

		/// <inheritdoc cref="IList{T}.this" />
		public virtual string this[int index]
		{
			get => options[index];

			set
			{
				if (value == null)
				{
					throw new ArgumentNullException(nameof(value));
				}

				if (this[index] == value)
				{
					return;
				}

				if (Contains(value))
				{
					throw new ArgumentException("This option already exists in the list.", nameof(value));
				}

				options[index] = value;
			}
		}

		/// <inheritdoc/>
		public virtual void Add(string item)
		{
			if (item == null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			if (Contains(item))
			{
				throw new ArgumentException("This option already exists in the list.", nameof(item));
			}

			options.Add(item);
		}

		/// <summary>
		/// 	Adds the elements of the specified collection to the end of the <see cref="OptionList"/>.
		/// </summary>
		/// <param name="items">The collection whose elements should be added to the end of the <see cref="OptionList"/>.</param>
		/// <exception cref="ArgumentNullException">When <paramref name="items"/> is <c>null</c>.</exception>
		public void AddRange(IEnumerable<string> items)
		{
			if (items == null)
			{
				throw new ArgumentNullException(nameof(items));
			}

			foreach (string item in items)
			{
				Add(item);
			}
		}

		/// <inheritdoc/>
		public virtual void Clear()
		{
			options.Clear();
		}

		/// <inheritdoc/>
		public bool Contains(string item)
		{
			return options.Contains(item);
		}

		/// <inheritdoc/>
		public void CopyTo(string[] array, int arrayIndex)
		{
			options.CopyTo(array, arrayIndex);
		}

		/// <inheritdoc/>
		public IEnumerator<string> GetEnumerator()
		{
			return options.GetEnumerator();
		}

		/// <inheritdoc/>
		public int IndexOf(string item)
		{
			return options.IndexOf(item);
		}

		/// <inheritdoc/>
		public virtual void Insert(int index, string item)
		{
			if (item == null)
			{
				throw new ArgumentNullException(nameof(item));
			}

			if (Contains(item))
			{
				throw new ArgumentException("This option already exists in the list.", nameof(item));
			}

			options.Insert(index, item);
		}

		/// <inheritdoc/>
		public virtual bool Remove(string item)
		{
			return options.Remove(item);
		}

		/// <inheritdoc/>
		public virtual void RemoveAt(int index)
		{
			options.RemoveAt(index);
		}

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return ((IEnumerable)options).GetEnumerator();
		}
	}
}