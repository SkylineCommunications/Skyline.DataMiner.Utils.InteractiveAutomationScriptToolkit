namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <inheritdoc cref="Skyline.DataMiner.InteractiveAutomationToolkit.IOptionsList{T}" />
	internal class OptionsList<T> : IOptionsList<T>, IReadonlyOptionsList<T>
	{
		private readonly IList<string> texts;
		private readonly IList<T> values = new List<T>();

		/// <summary>
		/// Initializes a new instance of the <see cref="OptionsList{T}"/> class.
		/// </summary>
		/// <param name="widgetOptionsCollection">The options list from the widget's <see cref="UIBlockDefinition"/>.</param>
		public OptionsList(IList<string> widgetOptionsCollection) => texts = widgetOptionsCollection;

		/// <inheritdoc cref="ICollection{T}.Count" />
		public int Count => texts.Count;

		/// <inheritdoc/>
		public bool IsReadOnly => texts.IsReadOnly;

		/// <inheritdoc cref="IList{T}.this" />
		public virtual Option<T> this[int index]
		{
			get => new Option<T>(texts[index], values[index]);

			set
			{
				value = new Option<T>(value.Text ?? String.Empty, value.Value);
				ThrowIfDuplicate(value.Text, value.Value);

				texts[index] = value.Text;
				values[index] = value.Value;
			}
		}

		/// <inheritdoc/>
		/// <exception cref="InvalidOperationException">
		/// if <paramref name="text"/> or <paramref name="value"/> is not unique.
		/// </exception>
		public virtual void Add(string text, T value)
		{
			text = text ?? String.Empty;
			ThrowIfDuplicate(text, value);

			texts.Add(text);
			values.Add(value);
		}

		/// <inheritdoc/>
		/// <exception cref="InvalidOperationException">
		/// if <see cref="Option{TValue}.Text"/> or <see cref="Option{TValue}.Value"/> of <paramref name="item"/> is not unique.
		/// </exception>
		public void Add(Option<T> item)
		{
			Add(item.Text, item.Value);
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
			texts.Clear();
			values.Clear();
		}

		/// <inheritdoc cref="IOptionsList{T}.ContainsText" />
		public bool ContainsText(string text)
		{
			return texts.Contains(text ?? String.Empty);
		}

		/// <inheritdoc cref="IOptionsList{T}.ContainsValue" />
		public bool ContainsValue(T value)
		{
			return values.Contains(value);
		}

		/// <inheritdoc/>
		public bool Contains(Option<T> item)
		{
			int indexOfText = texts.IndexOf(item.Text ?? String.Empty);
			int indexOfValue = values.IndexOf(item.Value);

			return indexOfText == indexOfValue && indexOfText != -1;
		}

		/// <inheritdoc/>
		public void CopyTo(Option<T>[] array, int arrayIndex)
		{
			for (var i = 0; i < texts.Count; i++)
			{
				array[i + arrayIndex] = new Option<T>(texts[i], values[i]);
			}
		}

		/// <inheritdoc/>
		public IEnumerator<Option<T>> GetEnumerator()
		{
			return texts.Zip(values, (text, value) => new Option<T>(text, value)).GetEnumerator();
		}

		/// <inheritdoc/>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <inheritdoc cref="IOptionsList{T}.IndexOfText" />
		public int IndexOfText(string text)
		{
			return texts.IndexOf(text ?? String.Empty);
		}

		/// <inheritdoc cref="IOptionsList{T}.IndexOfValue" />
		public int IndexOfValue(T value)
		{
			return values.IndexOf(value);
		}

		/// <inheritdoc/>
		public int IndexOf(Option<T> item)
		{
			int indexOfText = texts.IndexOf(item.Text ?? String.Empty);
			int indexOfValue = values.IndexOf(item.Value);

			return indexOfText == indexOfValue ? indexOfText : -1;
		}

		/// <inheritdoc/>
		public virtual void Insert(int index, string text, T value)
		{
			text = text ?? String.Empty;

			ThrowIfDuplicate(text, value);

			texts.Insert(index, text);
			values.Insert(index, value);
		}

		/// <inheritdoc/>
		public void Insert(int index, Option<T> item)
		{
			Insert(index, item.Text, item.Value);
		}

		/// <inheritdoc/>
		public virtual void RemoveAt(int index)
		{
			texts.RemoveAt(index);
			values.RemoveAt(index);
		}

		/// <inheritdoc/>
		public bool RemoveText(string text)
		{
			int index = texts.IndexOf(text ?? String.Empty);

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
			int indexOfText = texts.IndexOf(item.Text ?? String.Empty);
			int indexOfValue = values.IndexOf(item.Value);

			if (indexOfText != indexOfValue || indexOfText == -1)
			{
				return false;
			}

			RemoveAt(indexOfText);
			return true;
		}

		private void ThrowIfDuplicate(string text, T value)
		{
			if (texts.Contains(text))
			{
				throw new InvalidOperationException($"Collection already contains text: {text}");
			}

			if (values.Contains(value))
			{
				throw new InvalidOperationException($"Collection already contains value: {value}");
			}
		}
	}
}