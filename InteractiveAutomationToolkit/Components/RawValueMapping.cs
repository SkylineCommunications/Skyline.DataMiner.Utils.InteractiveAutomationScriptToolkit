namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;

	using Skyline.DataMiner.Utils.InteractiveAutomationScript.Tools;

	internal class RawValueMapping<T>
	{
		private readonly OneToOneMapping<string, T> mapping = new OneToOneMapping<string, T>();

		public bool TryGetByRawValue(string rawValue, out T value)
		{
			if (String.IsNullOrEmpty(rawValue))
			{
				value = default;
				return false;
			}

			return mapping.TryGetForward(rawValue, out value);
		}

		public string GetRawValue(T value)
		{
			if (!mapping.TryGetReverse(value, out var rawValue))
			{
				throw new KeyNotFoundException("The specified value was not found in the collection.");
			}
			return rawValue;
		}

		public void Add(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			if (!mapping.ContainsReverse(value))
			{
				var rawValue = Guid.NewGuid().ToString();
				mapping.Add(rawValue, value);
			}
		}

		public bool Remove(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			return mapping.TryRemoveReverse(value);
		}

		public void Clear()
		{
			mapping.Clear();
		}
	}
}
