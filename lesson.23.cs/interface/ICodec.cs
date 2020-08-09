namespace lesson._23.cs
{
    interface ICodec
    {
        public void Encode(IData input, IData output);
        public void Decode(IData input, IData output);
    }

}
