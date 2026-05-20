namespace Skyline.DataMiner.Utils.InteractiveAutomationScript
{
	using System;
	using System.Collections.Generic;
	using System.Text;

	using Skyline.DataMiner.Utils.InteractiveAutomationScript.Tools;

	internal class RawValueMapping<T>
	{
		private readonly object lockObject = new object();
		private readonly OneToOneMapping<string, T> mapping = new OneToOneMapping<string, T>();

		public bool TryGetByRawValue(string rawValue, out T value)
		{
			if (rawValue == null)
			{
				value = default;
				return false;
			}

			lock (lockObject)
			{
				return mapping.TryGetForward(rawValue, out value);
			}
		}

		public bool TryGetRawValue(T value, out string rawValue)
		{
			lock (lockObject)
			{
				if (!mapping.TryGetReverse(value, out rawValue))
				{
					return false;
				}

				return true;
			}
		}

		public string GetRawValue(T value)
		{
			if (!TryGetRawValue(value, out var rawValue))
			{
				throw new KeyNotFoundException("The specified value was not found in the collection.");
			}

			return rawValue;
		}

		public void Add(T value, string displayValue)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			lock (lockObject)
			{
				if (!mapping.ContainsReverse(value))
				{
					string rawValue = GenerateUniqueRawValue(displayValue);
					mapping.Add(rawValue, value);
				}
			}
		}

		public bool Remove(T value)
		{
			if (value == null)
			{
				throw new ArgumentNullException(nameof(value));
			}

			lock (lockObject)
			{
				return mapping.TryRemoveReverse(value);
			}
		}

		public void Clear()
		{
			lock (lockObject)
			{
				mapping.Clear();
			}
		}

		public override string ToString()
		{
			return $"RawValueMapping<{typeof(T).Name}> (Count = {mapping.Count})";
		}

		private string GenerateUniqueRawValue(string displayValue)
		{
			var cleanDisplayValue = CleanDisplayValue(displayValue);

			// Already unique
			if (!mapping.ContainsForward(cleanDisplayValue))
			{
				return cleanDisplayValue;
			}

			// Add -1, -2, -3...
			int counter = 1;
			string candidate;

			do
			{
				candidate = $"{cleanDisplayValue}-{counter++}";
			}
			while (mapping.ContainsForward(candidate));

			return candidate;
		}

		private static string CleanDisplayValue(string displayValue)
		{
			if (String.IsNullOrEmpty(displayValue))
			{
				return String.Empty;
			}

			var sb = new StringBuilder();
			bool lastWasDash = false;

			foreach (char c in displayValue)
			{
				if (Char.IsLetterOrDigit(c))
				{
					sb.Append(c);
					lastWasDash = false;
				}
				else if (!lastWasDash)
				{
					sb.Append('-');
					lastWasDash = true;
				}
			}

			return sb.ToString();
		}
	}
}
