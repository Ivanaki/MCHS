#if UNITY_EDITOR

namespace CodeGen
{
    public static class StringExtensions
    {
        /// <summary>
        /// Возвращает копию исходной строки, в которой первая буква преобразована в нижний регистр.
        /// Если строка пуста или null, возвращается исходная строка.
        /// </summary>
        public static string ToLowerFirst(this string s)
        {
            if (string.IsNullOrEmpty(s))
                return s;

            // Если строка состоит всего из одного символа, просто преобразуем его.
            if (s.Length == 1)
                return s.ToLower();

            // Преобразуем первую букву и сохраняем остаток строки без изменений.
            return char.ToLower(s[0]) + s.Substring(1);
        }
    }
}

#endif