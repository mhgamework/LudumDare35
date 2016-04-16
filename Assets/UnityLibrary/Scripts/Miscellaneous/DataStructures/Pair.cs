namespace Miscellaneous.DataStructures
{
    public struct Pair<KeyClass, ValueClass>
    {
        // .. ATTRIBUTES

        public KeyClass Key;
        public ValueClass Value;


        // .. INITIALIZATION

        public Pair(KeyClass key, ValueClass value)
        {
            Key = key;
            Value = value;
        }

    }
}
