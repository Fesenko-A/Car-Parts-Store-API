namespace BL.Utility {
    public static class StringExtensions {
        public static string Capitalize(this string value) {
            return char.ToUpper(value[0]) + value.Substring(1);
        }
    }
}
