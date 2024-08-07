namespace Byndyusoft.ArchitectureTesting.Abstractions.Validation.Extensions
{
    using System;
    using System.ComponentModel;
    using System.Linq;
    using System.Reflection;

    public static class EnumExtension
    {
        /// <summary>
        /// Получает значение атрибута DescriptionAttribute для переданного значения перечисления <paramref name="member"/>
        /// </summary>
        /// <param name="member">Значение из перечисления</param>
        public static string GetDescription(this Enum member)
        {
            var memberTypeInfo = member.GetType().GetTypeInfo();
            if (memberTypeInfo.IsEnum == false)
                throw new ArgumentOutOfRangeException(nameof(member), "member is not enum");

            var memberString = member.ToString();
            var fieldInfo = memberTypeInfo.GetField(memberString);
            if (fieldInfo == null)
                throw new InvalidOperationException($"{memberString} is not a part of the enum declaration");

            var attributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>(false).ToArray();
            return attributes.Length == 0 ? memberString : attributes[0].Description;
        }
    }
}