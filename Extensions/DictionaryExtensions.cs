namespace ktsu.io.Extensions;

using System.Collections.Generic;

/// <summary>
/// Extension methods for dictionaries.
/// </summary>
public static class DictionaryExtensions
{
	/// <summary>
	/// Method that gets a value from a dictionary if it exists, otherwise creates a new value and adds it to the dictionary.
	/// </summary>
	/// <typeparam name="TKey">The type of the keys in the dictionary.</typeparam>
	/// <typeparam name="TVal">The type of the values in the dictionary.</typeparam>
	/// <param name="dict">The dictionary to get the value from.</param>
	/// <param name="key">The key to get the value for.</param>
	/// <returns>The value for the key if it exists, otherwise a new value.</returns>
	public static TVal? GetOrCreate<TKey, TVal>(this Dictionary<TKey, TVal> dict, TKey key) where TKey : notnull where TVal : new()
	{
		ArgumentNullException.ThrowIfNull(dict, nameof(dict));
		ArgumentNullException.ThrowIfNull(key, nameof(key));
		if (dict.TryGetValue(key, out var val))
		{
			return val;
		}

		var newVal = new TVal();
		dict.Add(key, newVal);
		return newVal;
	}
}
