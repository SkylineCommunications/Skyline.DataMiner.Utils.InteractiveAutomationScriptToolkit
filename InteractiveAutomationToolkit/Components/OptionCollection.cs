namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections;
	using System.Collections.Generic;
	using System.Linq;

	internal class OptionCollection<T> : ICollection<Option<T>>
	{
		private readonly HashSet<Option<T>> options = new HashSet<Option<T>>();

		public int Count => options.Count;

		public bool IsReadOnly => false;

		public void Add(Option<T> item)
		{
			if (options.Any(x => string.Equals(x.DisplayValue, item.DisplayValue))) throw new InvalidOperationException($"The collection already contains an item with {item.DisplayValue} as displayed value");
			if (options.Any(x => Equals(x.Value, item.Value))) throw new InvalidOperationException($"The collection already contains an item with {item.Value} as value");

			options.Add(item);
		}

		public void Clear()
		{
			options.Clear();
		}

		public bool Contains(Option<T> item)
		{
			if (options.Any(x => string.Equals(x.DisplayValue, item.DisplayValue))) return true;
			if (options.Any(x => Equals(x.Value, item.Value))) return true;
			return false;
		}

		public void CopyTo(Option<T>[] array, int arrayIndex)
		{
			if (array == null) throw new ArgumentNullException("array");
			if (arrayIndex < 0) throw new ArgumentOutOfRangeException("arrayIndex");
			if (array.Length - arrayIndex < Count) throw new ArgumentException("Not enough elements after arrayIndex in the destination array.");

			int destinationIndex = arrayIndex;
			foreach (var option in options)
			{
				array[destinationIndex] = option;
				destinationIndex++;
			}
		}

		public IEnumerator<Option<T>> GetEnumerator()
		{
			return options.GetEnumerator();
		}

		public bool Remove(Option<T> item)
		{
			return options.Remove(item);
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return options.GetEnumerator();
		}
	}
}
