namespace lesson._21.cs
{
    static class Util
    {
        static public string Left(this string text, int length)
        {
            return text.Substring(0, length);
        }

        static public string Right(this string text, int length)
        {
            return text.Substring(text.Length - length, length);
        }
    }
}
