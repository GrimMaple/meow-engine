using System;

namespace Meow.Framework.Util
{
    class RLEElement : IComparable
    {
        private byte[] data;

        public int Length
        {
            get
            {
                return data.Length;
            }
        }

        public byte[] Data
        {
            get
            {
                return data;
            }
        }

        public RLEElement(byte[] element)
        {
            data = element;
        }

        public int CompareTo(object obj)
        {
            if (!(obj is RLEElement))
                throw new ArgumentException();
            RLEElement second = obj as RLEElement;
            if (second.Length != Length)
                return Length.CompareTo(second.Length);
            for(int i=0; i<Length; i++)
            {
                if (data[i] != second.data[i])
                    return data[i].CompareTo(second.data[i]);
            }
            return 0;
        }

        public override bool Equals(object other)
        {
            if (!(other is RLEElement))
                throw new ArgumentException();
            return (other as RLEElement).CompareTo(this) == 0;
        }

        public static bool operator ==(RLEElement a, RLEElement b)
        {
            if ((a as object) == null || (b as object) == null)
                return false;
            return a.Equals(b);
        }

        public static bool operator !=(RLEElement a, RLEElement b)
        {
            return !(a == b);
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
