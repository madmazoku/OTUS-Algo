

namespace lesson._04.cs
{
    class Utils
    {
        public static void MoveForward<T>(T[] data, int index, int length)
        {
            for (int i = index + length; i > index; --i)
                data[i] = data[i - 1];
        }
        public static void MoveBackward<T>(T[] data, int index, int length)
        {
            for (int i = index; i < index + length - 1; ++i)
                data[i] = data[i + 1];
        }
    }
}
