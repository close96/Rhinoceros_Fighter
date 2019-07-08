// http://www.luispedrofonseca.com/unity-quick-tips-enum-description-extension-method/
// このソースコードは私が書いたものではありません

using System;
using System.ComponentModel;

public static class EnumsHelperExtension
{
	public static string ToDescription(this Enum value)
	{
		DescriptionAttribute[] da = (DescriptionAttribute[])(value.GetType().GetField(value.ToString())).GetCustomAttributes(typeof(DescriptionAttribute), false);
		return da.Length > 0 ? da[0].Description : value.ToString();
	}
}