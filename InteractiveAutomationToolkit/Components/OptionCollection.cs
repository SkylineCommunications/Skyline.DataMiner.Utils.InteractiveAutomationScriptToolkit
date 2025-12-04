namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections;
	using System.Collections.Generic;

	internal class OptionCollection<T> : ICollection<Option<T>>
	{
		private readonly HashSet<Option<T>> options = new HashSet<Option<T>>();
		private readonly RawValueMapping<Option<T>> rawValueMapping = new RawValueMapping<Option<T>>();

		public int Count => options.Count;

		public bool IsReadOnly => false;

		public void Add(Option<T> item)
		{
			if (!options.Add(item)) throw new InvalidOperationException($"The collection already contains an item with {item.DisplayValue} as displayed value");

			rawValueMapping.Add(item);
		}

		public bool Remove(Option<T> item)
		{
			if (options.Remove(item))
			{
				rawValueMapping.Remove(item);
				return true;
			}

			return false;
		}

		public void Clear()
		{
			options.Clear();
			rawValueMapping.Clear();
		}

		public bool Contains(Option<T> item)
		{
			return options.Contains(item);
		}

		public bool TryGetByRawValue(string rawValue, out Option<T> option)
		{
			return rawValueMapping.TryGetByRawValue(rawValue, out option);
		}

		public string GetRawValue(Option<T> option)
		{
			return rawValueMapping.GetRawValue(option);
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

		IEnumerator IEnumerable.GetEnumerator()
		{
			return options.GetEnumerator();
		}
	}
}
