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

            var fieldInfo = memberTypeInfo.GetField(member.ToString());
            if (fieldInfo == null)
                throw new InvalidOperationException($"{nameof(member)} is not a part of the enum declaration");

            var attributes = fieldInfo.GetCustomAttributes<DescriptionAttribute>(false).ToArray();
            if (attributes.Length == 0)
                throw new InvalidOperationException($"{nameof(DescriptionAttribute)} was not declared for {nameof(member)}");

            return attributes[0].Description;
        }
    }
}