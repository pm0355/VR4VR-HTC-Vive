namespace ExtensionSystem
{
	using System;

	/// <summary>
	/// System.Enum extensions
	/// </summary>
	public static class EEnum
	{
		public static object rawValue(Enum _enum)
		{
			return Convert.ChangeType(_enum, _enum.GetTypeCode());
		}
	}

	/// <summary>
	/// System.Array extensions
	/// </summary>
	public static class EArray
	{
		public static bool TryGetElement<TValue>(TValue[] array, int index, out TValue element)
		{
			if (array != null && index < array.Length)
			{
				element = array[index];
				return true;
			}
			element = default(TValue);
			return false;
		}

		public static bool TryGetEnumElement<TValue, TEnum>(TValue[] array, int index, out TEnum element)
		{
			TValue value;
			if (TryGetElement(array, index, out value) && Enum.IsDefined(typeof(TEnum), value))
			{
				element = (TEnum)Enum.ToObject(typeof(TEnum), value);
				return true;
			}
			element = default(TEnum);
			return false;
		}
	}


	namespace Collections.Generic
	{
		using System.Collections.Generic;
		/// <summary>
		/// System.Collections.Generic.IDictionary<> extensions
		/// </summary>
		public static class EIDictionary
		{
			public static bool TryGetTypedValue<TKey, TValue, TActual>(IDictionary<TKey, TValue> dictionary, TKey key, out TActual value) where TActual : TValue
			{
				TValue tvalue;
				if (dictionary.TryGetValue(key, out tvalue) && (tvalue is TActual || typeof(TActual).IsEnum))
				{
					value = (TActual)tvalue;
					return true;
				}
				value = default(TActual);
				return false;
			}
		}
	}
}