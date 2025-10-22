namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Linq;

	using Skyline.DataMiner.Net.AutomationUI.Objects;

	/// <summary>
	///     Represents a TreeViewItem with an associated value of type <typeparamref name="T"/>.
	///     This allows attaching custom metadata to tree view items.
	/// </summary>
	/// <typeparam name="T">The type of the value associated with this tree view item.</typeparam>
	public sealed class TreeViewItemOption<T> : IEquatable<TreeViewItemOption<T>>
	{
		/// <summary>
		///     Initializes a new instance of the <see cref="TreeViewItemOption{T}"/> class.
		/// </summary>
		/// <param name="item">The underlying TreeViewItem.</param>
		/// <param name="value">The value to associate with this tree view item.</param>
		/// <exception cref="ArgumentNullException">When item is null.</exception>
		public TreeViewItemOption(TreeViewItem item, T value)
		{
			Item = item ?? throw new ArgumentNullException(nameof(item));
			Value = value;
		}

		/// <summary>
		///     Initializes a new instance of the <see cref="TreeViewItemOption{T}"/> class.
		///     Creates a TreeViewItem with the specified parameters.
		/// </summary>
		/// <param name="keyValue">The unique key for the tree view item.</param>
		/// <param name="displayValue">The text to display for the tree view item.</param>
		/// <param name="value">The value to associate with this tree view item.</param>
		/// <param name="itemType">The type of the tree view item.</param>
		/// <param name="childItems">The child items of this tree view item.</param>
		/// <exception cref="ArgumentNullException">When keyValue or displayValue is null.</exception>
		public TreeViewItemOption(
			string keyValue,
			string displayValue,
			T value,
			TreeViewItem.TreeViewItemType itemType = TreeViewItem.TreeViewItemType.CheckBox,
			IEnumerable<TreeViewItemOption<T>> childItems = null)
		{
			if (keyValue == null)
			{
				throw new ArgumentNullException(nameof(keyValue));
			}

			if (displayValue == null)
			{
				throw new ArgumentNullException(nameof(displayValue));
			}

			var children = childItems?.Select(x => x.Item).ToList() ?? new List<TreeViewItem>();
			Item = new TreeViewItem(displayValue, keyValue, children)
			{
				ItemType = itemType
			};
			Value = value;
		}

		/// <summary>
		///     Gets the underlying TreeViewItem.
		/// </summary>
		public TreeViewItem Item { get; }

		/// <summary>
		///     Gets the value associated with this tree view item.
		/// </summary>
		public T Value { get; }

		/// <summary>
		///     Gets or sets the unique key for the tree view item.
		/// </summary>
		public string KeyValue
		{
			get => Item.KeyValue;
			set => Item.KeyValue = value;
		}

		/// <summary>
		///     Gets or sets the text to display for the tree view item.
		/// </summary>
		public string DisplayValue
		{
			get => Item.DisplayValue;
			set => Item.DisplayValue = value;
		}

		/// <summary>
		///     Gets or sets the type of the tree view item.
		/// </summary>
		public TreeViewItem.TreeViewItemType ItemType
		{
			get => Item.ItemType;
			set => Item.ItemType = value;
		}

		/// <summary>
		///     Gets or sets a value indicating whether the tree view item is checked.
		/// </summary>
		public bool IsChecked
		{
			get => Item.IsChecked;
			set => Item.IsChecked = value;
		}

		/// <summary>
		///     Gets or sets a value indicating whether the tree view item is collapsed.
		/// </summary>
		public bool IsCollapsed
		{
			get => Item.IsCollapsed;
			set => Item.IsCollapsed = value;
		}

		/// <summary>
		///     Gets or sets a value indicating whether the tree view item supports lazy loading.
		/// </summary>
		public bool SupportsLazyLoading
		{
			get => Item.SupportsLazyLoading;
			set => Item.SupportsLazyLoading = value;
		}

		/// <summary>
		///     Gets the child items of this tree view item.
		/// </summary>
		public IEnumerable<TreeViewItem> ChildItems => Item.ChildItems;

		/// <summary>
		///     Determines whether the specified object is equal to the current object.
		/// </summary>
		/// <param name="obj">The object to compare with the current object.</param>
		/// <returns>true if the specified object is equal to the current object; otherwise, false.</returns>
		public override bool Equals(object obj)
		{
			return obj is TreeViewItemOption<T> option && Equals(option);
		}

		/// <summary>
		///     Determines whether the specified TreeViewItemOption is equal to the current TreeViewItemOption.
		/// </summary>
		/// <param name="other">The TreeViewItemOption to compare with the current TreeViewItemOption.</param>
		/// <returns>true if the specified TreeViewItemOption is equal to the current TreeViewItemOption; otherwise, false.</returns>
		public bool Equals(TreeViewItemOption<T> other)
		{
			if (ReferenceEquals(this, other)) return true;
			if (ReferenceEquals(null, other)) return false;

			return Item.KeyValue.Equals(other.Item.KeyValue) &&
				EqualityComparer<T>.Default.Equals(Value, other.Value);
		}

		/// <summary>
		///     Serves as the default hash function.
		/// </summary>
		/// <returns>A hash code for the current object.</returns>
		public override int GetHashCode()
		{
			int hashCode = 11;
			hashCode ^= 13 * Item.KeyValue.GetHashCode();
			hashCode ^= 13 * (Value != null ? Value.GetHashCode() : 0);
			return hashCode;
		}

		/// <summary>
		///     Returns a string that represents the current object.
		/// </summary>
		/// <returns>A string that represents the current object.</returns>
		public override string ToString()
		{
			return $"{DisplayValue} ({KeyValue}) => {Value}";
		}

		/// <summary>
		///     Determines whether two TreeViewItemOption objects are equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>true if the objects are equal; otherwise, false.</returns>
		public static bool operator ==(TreeViewItemOption<T> left, TreeViewItemOption<T> right)
		{
			return Equals(left, right);
		}

		/// <summary>
		///     Determines whether two TreeViewItemOption objects are not equal.
		/// </summary>
		/// <param name="left">The first object to compare.</param>
		/// <param name="right">The second object to compare.</param>
		/// <returns>true if the objects are not equal; otherwise, false.</returns>
		public static bool operator !=(TreeViewItemOption<T> left, TreeViewItemOption<T> right)
		{
			return !Equals(left, right);
		}
	}
}
