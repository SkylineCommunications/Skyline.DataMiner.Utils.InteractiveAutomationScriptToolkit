namespace Skyline.DataMiner.Utils.InteractiveAutomationScript.Tools
{
	using System;
	using System.Collections.Generic;

	internal class OneToOneMapping<Ta, Tb>
	{
		#region Private Fields

		private readonly IEqualityComparer<Ta> _forwardComparer;
		private readonly IEqualityComparer<Tb> _reverseComparer;

		private readonly Dictionary<Ta, Tb> _forwardMapping;
		private readonly Dictionary<Tb, Ta> _reverseMapping;

		#endregion

		public OneToOneMapping(
			IEqualityComparer<Ta> forwardComparer = null,
			IEqualityComparer<Tb> reverseComparer = null)
		{
			_forwardComparer = forwardComparer ?? EqualityComparer<Ta>.Default;
			_reverseComparer = reverseComparer ?? EqualityComparer<Tb>.Default;

			_forwardMapping = new Dictionary<Ta, Tb>(_forwardComparer);
			_reverseMapping = new Dictionary<Tb, Ta>(_reverseComparer);
		}

		#region Public Properties

		public IReadOnlyDictionary<Ta, Tb> Forward
		{
			get { return _forwardMapping; }
		}

		public IReadOnlyDictionary<Tb, Ta> Reverse
		{
			get { return _reverseMapping; }
		}

		public int Count => _forwardMapping.Count;

		#endregion

		#region Public Methods

		public Tb GetForward(Ta a)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a), "Key cannot be null");

			if (!_forwardMapping.TryGetValue(a, out var value))
				throw new ArgumentException("Key does not exist", nameof(a));

			return value;
		}

		public bool TryGetForward(Ta a, out Tb value)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));

			return _forwardMapping.TryGetValue(a, out value);
		}

		public Ta GetReverse(Tb b)
		{
			if (b == null)
				throw new ArgumentNullException(nameof(b), "Key cannot be null");

			if (!_reverseMapping.TryGetValue(b, out var value))
				throw new ArgumentException("Key does not exist", nameof(b));

			return value;
		}

		public bool TryGetReverse(Tb b, out Ta value)
		{
			if (b == null)
				throw new ArgumentNullException(nameof(b));

			return _reverseMapping.TryGetValue(b, out value);
		}

		public void Add(Ta a, Tb b)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a), "Key cannot be null");

			if (b == null)
				throw new ArgumentNullException(nameof(b), "Key cannot be null");

			// Check if either key already exists
			if (_forwardMapping.ContainsKey(a))
				throw new ArgumentException("Key already exists", nameof(a));

			if (_reverseMapping.ContainsKey(b))
				throw new ArgumentException("Key already exists", nameof(b));

			_forwardMapping[a] = b;
			_reverseMapping[b] = a;
		}

		public bool TryAdd(Ta a, Tb b)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));

			if (b == null)
				throw new ArgumentNullException(nameof(b));

			if (_forwardMapping.ContainsKey(a) || _reverseMapping.ContainsKey(b))
			{
				return false;
			}

			_forwardMapping[a] = b;
			_reverseMapping[b] = a;

			return true;
		}

		public void AddOrUpdate(Ta a, Tb b)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));

			if (b == null)
				throw new ArgumentNullException(nameof(b));

			// Remove any existing mappings for these keys to maintain one-to-one relationship
			if (_forwardMapping.TryGetValue(a, out var existingB))
			{
				_reverseMapping.Remove(existingB);
			}

			if (_reverseMapping.TryGetValue(b, out var existingA))
			{
				_forwardMapping.Remove(existingA);
			}

			_forwardMapping[a] = b;
			_reverseMapping[b] = a;
		}

		public void Remove(Ta a, Tb b)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a), "Key cannot be null");

			if (b == null)
				throw new ArgumentNullException(nameof(b), "Key cannot be null");

			if (!Contains(a, b))
				throw new ArgumentException("Mapping does not exist");

			_forwardMapping.Remove(a);
			_reverseMapping.Remove(b);
		}

		public bool TryRemove(Ta a, Tb b)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));

			if (b == null)
				throw new ArgumentNullException(nameof(b));

			if (!Contains(a, b))
			{
				return false;
			}

			Remove(a, b);
			return true;
		}

		public void RemoveForward(Ta a)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a), "Key cannot be null");

			if (!_forwardMapping.TryGetValue(a, out var b))
				throw new ArgumentException("Key does not exist", nameof(a));

			_forwardMapping.Remove(a);
			_reverseMapping.Remove(b);
		}

		public bool TryRemoveForward(Ta a)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));

			if (!_forwardMapping.ContainsKey(a))
			{
				return false;
			}

			RemoveForward(a);
			return true;
		}

		public void RemoveReverse(Tb b)
		{
			if (b == null)
				throw new ArgumentNullException(nameof(b), "Key cannot be null");

			if (!_reverseMapping.TryGetValue(b, out var a))
				throw new ArgumentException("Key does not exist", nameof(b));

			_reverseMapping.Remove(b);
			_forwardMapping.Remove(a);
		}

		public bool TryRemoveReverse(Tb b)
		{
			if (b == null)
				throw new ArgumentNullException(nameof(b));

			if (!_reverseMapping.ContainsKey(b))
			{
				return false;
			}

			RemoveReverse(b);
			return true;
		}

		public void Clear()
		{
			_forwardMapping.Clear();
			_reverseMapping.Clear();
		}

		public bool ContainsForward(Ta a)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a));

			return _forwardMapping.ContainsKey(a);
		}

		public bool ContainsReverse(Tb b)
		{
			if (b == null)
				throw new ArgumentNullException(nameof(b));

			return _reverseMapping.ContainsKey(b);
		}

		public bool Contains(Ta a, Tb b)
		{
			if (a == null)
				throw new ArgumentNullException(nameof(a), "Key cannot be null");

			if (b == null)
				throw new ArgumentNullException(nameof(b), "Key cannot be null");

			return _forwardMapping.TryGetValue(a, out var mappedB) &&
				_reverseComparer.Equals(mappedB, b);
		}

		public override string ToString()
		{
			return $"OneToOneMapping<{typeof(Ta).Name}, {typeof(Tb).Name}> [Count: {Count}]";
		}

		#endregion
	}
}
