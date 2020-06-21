namespace lesson._12.cs
{
    interface IHashTable
    {

        public string Insert(string key, string value);
        public string Find(string key);
        public string Remove(string key);

    }
}
