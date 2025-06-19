using System.ComponentModel;

namespace HqNotenverwaltung.Tools
{
    public static class ToolsEnum
    {
        public static string GetDescription<T>(int value) where T : Enum
        {
            var enumValue = (T)Enum.ToObject(typeof(T), value);
            var fieldInfo = enumValue.GetType().GetField(enumValue.ToString());
            if (fieldInfo != null)
            {
                var attributes = fieldInfo.GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attributes.Length > 0) 
                {
                    return ((DescriptionAttribute)attributes[0]).Description;
                }
            }
            return enumValue.ToString(); // Fallback to the enum name if no description is found
        }
    }
}
