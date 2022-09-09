namespace Skyline.DataMiner.InteractiveAutomationToolkit
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.ComponentModel;
	using System.Linq;

	using Skyline.DataMiner.Automation;

	/// <inheritdoc cref="Skyline.DataMiner.InteractiveAutomationToolkit.IDropDown{T}" />
	public class DropDown<T> : InteractiveWidget, IDropDown<T>
	{
		private readonly OptionsCollection optionsCollection;
		private bool changed;
		private string previous;

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown{T}" /> class.
		/// </summary>
		public DropDown()
			: this(Enumerable.Empty<KeyValuePair<string, T>>())
		{
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="DropDown{T}" /> class.
		/// </summary>
		/// <param name="options">Options to be displayed in the list.</param>
		/// <exception cref="ArgumentNullException">When options is null.</exception>
		public DropDown(IEnumerable<KeyValuePair<string, T>> options)
		{
			if (options == null)
			{
				throw new ArgumentNullException(nameof(options));
			}

			Type = UIBlockType.DropDown;
			optionsCollection = new OptionsCollection(this);

			Options.AddRange(options);

			ValidationText = "Invalid Input";
			ValidationState = UIValidationState.NotValidated;
		}

		/// <inheritdoc />
		public event EventHandler<ChangedEventArgs> Changed
		{
			add
			{
				OnChanged += value;
				WantsOnChange = true;
			}

			remove
			{
				OnChanged -= value;
				if (OnChanged == null || !OnChanged.GetInvocationList().Any())
				{
					WantsOnChange = false;
				}
			}
		}

		private event EventHandler<ChangedEventArgs> OnChanged;

		/// <inheritdoc />
		public IOptionsList<T> Options => optionsCollection;

		/// <inheritdoc />
		public bool IsDisplayFilterShown
		{
			get => BlockDefinition.DisplayFilter;
			set => BlockDefinition.DisplayFilter = value;
		}

		/// <inheritdoc />
		public bool IsSorted
		{
			get => BlockDefinition.IsSorted;
			set => BlockDefinition.IsSorted = value;
		}

		/// <inheritdoc />
		public string SelectedText
		{
			get => BlockDefinition.InitialValue;

			set
			{
				// Prevent setting a value that is not part of the options
				if (Options.ContainsText(value ?? String.Empty))
				{
					BlockDefinition.InitialValue = value ?? String.Empty;
				}
			}
		}

		/// <inheritdoc />
		public T SelectedValue
		{
			get
			{
				int index = optionsCollection.IndexOfText(SelectedText);
				return index == -1 ? default : optionsCollection[index].Value;
			}

			set
			{
				int index = optionsCollection.IndexOfValue(value);
				if (index == -1)
				{
					return;
				}

				SelectedText = optionsCollection[index].Key;
			}
		}

		/// <inheritdoc />
		public string Tooltip
		{
			get => BlockDefinition.TooltipText;
			set => BlockDefinition.TooltipText = value ?? String.Empty;
		}

		/// <inheritdoc />
		public UIValidationState ValidationState
		{
			get => BlockDefinition.ValidationState;

			set
			{
				if (!Enum.IsDefined(typeof(UIValidationState), value))
				{
					throw new InvalidEnumArgumentException(nameof(value), (int)value, typeof(UIValidationState));
				}

				BlockDefinition.ValidationState = value;
			}
		}

		/// <inheritdoc />
		public string ValidationText
		{
			get => BlockDefinition.ValidationText;
			set => BlockDefinition.ValidationText = value ?? String.Empty;
		}

		/// <inheritdoc />
		public void ForceSelected(string selected)
		{
			BlockDefinition.InitialValue = selected ?? String.Empty;
		}

		/// <inheritdoc />
		protected internal override void LoadResult(UIResults results)
		{
			string selectedValue = results.GetString(this);
			if (WantsOnChange)
			{
				changed = selectedValue != SelectedText;
			}

			previous = SelectedText;

			// Write to BlockDefinition instead of Selected property so a force selected option does not reset after every interaction
			BlockDefinition.InitialValue = selectedValue;
		}

		/// <inheritdoc />
		protected internal override void RaiseResultEvents()
		{
			if (changed && OnChanged != null)
			{
				int index = optionsCollection.IndexOfText(previous);
				T previousValue = default;
				if (index != -1)
				{
					previousValue = optionsCollection[index].Value;
				}

				OnChanged(this, new ChangedEventArgs(SelectedText, SelectedValue, previous, previousValue));
			}

			changed = false;
		}

		/// <summary>
		///     Provides data for the <see cref="Changed" /> event.
		/// </summary>
		public class ChangedEventArgs : EventArgs
		{
			/// <summary>
			///     Initializes a new instance of the <see cref="ChangedEventArgs" /> class.
			/// </summary>
			/// <param name="selectedText">The displayed text of the option that has been selected.</param>
			/// <param name="selectedValue">The value of the option that has been selected.</param>
			/// <param name="previousText">The displayed text of the previously selected option.</param>
			/// <param name="previousValue">The value of the previously selected option.</param>
			public ChangedEventArgs(string selectedText, T selectedValue, string previousText, T previousValue)
			{
				SelectedText = selectedText;
				SelectedValue = selectedValue;
				PreviousText = previousText;
				PreviousValue = previousValue;
			}

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public string PreviousText { get; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public string SelectedText { get; }

			/// <summary>
			///     Gets the previously selected option.
			/// </summary>
			public T PreviousValue { get; }

			/// <summary>
			///     Gets the option that has been selected.
			/// </summary>
			public T SelectedValue { get; }
		}

		private class OptionsCollection : IOptionsList<T>
		{
			private readonly IList<string> texts;
			private readonly IList<T> values = new List<T>();
			private readonly IDropDown<T> owner;

			public OptionsCollection(IDropDown<T> owner)
			{
				this.owner = owner;
				texts = owner.BlockDefinition.GetOptionsCollection();
			}

			public int Count => texts.Count;

			public bool IsReadOnly => texts.IsReadOnly;

			public KeyValuePair<string, T> this[int index]
			{
				get => new KeyValuePair<string, T>(texts[index], values[index]);

				set
				{
					value = new KeyValuePair<string, T>(value.Key ?? String.Empty, value.Value);
					if (texts.Contains(value.Key))
					{
						throw new InvalidOperationException($"{nameof(DropDown<T>)} already contains text: {value}");
					}

					if (values.Contains(value.Value))
					{
						throw new InvalidOperationException($"{nameof(DropDown<T>)} already contains value: {value}");
					}

					string option = texts[index];
					texts[index] = value.Key;
					values[index] = value.Value;

					if (owner.SelectedText == option)
					{
						owner.SelectedText = this.FirstOrDefault().Key;
					}

					// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
					// But I believe this behavior should be reflected by the Selected property.
					// Selected should never be null if options is not empty.
					if (owner.SelectedText == null)
					{
						owner.SelectedText = value.Key;
					}
				}
			}

			public void Add(string text, T value)
			{
				text = text ?? String.Empty;
				if (texts.Contains(text))
				{
					throw new InvalidOperationException($"{nameof(DropDown<T>)} already contains text: {text}");
				}

				if (values.Contains(value))
				{
					throw new InvalidOperationException($"{nameof(DropDown<T>)} already contains value: {value}");
				}

				texts.Add(text);
				values.Add(value);

				// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
				// But I believe this behavior should be reflected by the Selected property.
				// Selected should never be null if options is not empty.
				if (owner.SelectedText == null)
				{
					owner.SelectedText = text;
				}
			}

			public void Add(KeyValuePair<string, T> item)
			{
				Add(item.Key, item.Value);
			}

			public void AddRange(IEnumerable<KeyValuePair<string, T>> options)
			{
				if (options == null)
				{
					throw new ArgumentNullException(nameof(options));
				}

				foreach (KeyValuePair<string, T> option in options)
				{
					Add(option);
				}
			}

			public void Clear()
			{
				texts.Clear();
				values.Clear();
				owner.BlockDefinition.InitialValue = null;
			}

			public bool ContainsText(string text)
			{
				return texts.Contains(text ?? String.Empty);
			}

			public bool ContainsValue(T value)
			{
				return values.Contains(value);
			}

			public bool Contains(KeyValuePair<string, T> item)
			{
				int indexOfText = texts.IndexOf(item.Key ?? String.Empty);
				int indexOfValue = values.IndexOf(item.Value);

				return indexOfText == indexOfValue && indexOfText != -1;
			}

			public void CopyTo(KeyValuePair<string, T>[] array, int arrayIndex)
			{
				for (var i = 0; i < texts.Count; i++)
				{
					array[i + arrayIndex] = new KeyValuePair<string, T>(texts[i], values[i]);
				}
			}

			public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
			{
				return texts.Zip(values, (text, value) => new KeyValuePair<string, T>(text, value)).GetEnumerator();
			}

			IEnumerator IEnumerable.GetEnumerator()
			{
				return GetEnumerator();
			}

			public int IndexOfText(string text)
			{
				return texts.IndexOf(text ?? String.Empty);
			}

			public int IndexOfValue(T value)
			{
				return values.IndexOf(value);
			}

			public int IndexOf(KeyValuePair<string, T> item)
			{
				int indexOfText = texts.IndexOf(item.Key ?? String.Empty);
				int indexOfValue = values.IndexOf(item.Value);

				return indexOfText == indexOfValue ? indexOfText : -1;
			}

			public void Insert(int index, string text, T value)
			{
				text = text ?? String.Empty;

				if (texts.Contains(text))
				{
					throw new InvalidOperationException($"{nameof(DropDown<T>)} already contains text: {text}");
				}

				if (values.Contains(value))
				{
					throw new InvalidOperationException($"{nameof(DropDown<T>)} already contains value: {value}");
				}

				texts.Insert(index, text);
				values.Insert(index, value);

				// Cube will select the first option even if UIBlockDefinition.InitialValue is null.
				// But I believe this behavior should be reflected by the Selected property.
				// Selected should never be null if options is not empty.
				if (owner.SelectedText == null)
				{
					owner.SelectedText = text;
				}
			}

			public void Insert(int index, KeyValuePair<string, T> item)
			{
				Insert(index, item.Key, item.Value);
			}

			public void RemoveAt(int index)
			{
				string option = texts[index];
				texts.RemoveAt(index);
				values.RemoveAt(index);
				if (owner.SelectedText == option)
				{
					owner.SelectedText = texts.FirstOrDefault();
				}
			}

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

			public bool Remove(KeyValuePair<string, T> item)
			{
				int indexOfText = texts.IndexOf(item.Key ?? String.Empty);
				int indexOfValue = values.IndexOf(item.Value);

				if (indexOfText != indexOfValue || indexOfText == -1)
				{
					return false;
				}

				texts.RemoveAt(indexOfText);
				values.RemoveAt(indexOfText);
				return true;
			}
		}
	}
}