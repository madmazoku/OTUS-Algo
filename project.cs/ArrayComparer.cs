using System.Collections.Generic;

namespace project.cs
{
    class ArrayComparer<T> : IEqualityComparer<T[]>
    {
        public int GetHashCode(T[] x)
        {
            if (x != null)
            {
                unchecked
                {
                    int h = 7907;
                    foreach (var e in x)
                        h = h * 7919 + ((e != null) ? e.GetHashCode() : 0);
                    return h;
                }
            }
            return 0;
        }

        public bool Equals(T[] a, T[] b)
        {
            if (object.ReferenceEquals(a, b))
                return true;

            if (a != null && b != null && (a.Length == b.Length))
            {
                for (int i = 0; i < a.Length; i++)
                    if (!object.Equals(a[i], b[i]))
                        return false;
                return true;
            }

            return false;
        }
    }
}
