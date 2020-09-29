using System;

namespace TransferObjects
{
    public abstract class Book : IEquatable<Book>
    {
        public string Isbn { get; set; }
        public string Name { get; set; }
        public int NumberOfCopies { get; set; }
        public string BookFormat { get; set; }
        public string BookMount { get; set; }
        public int Imposition { get; set; }
        public double PrintСoefficient { get; set; }
        public string FullPath { get; set; }

        public virtual bool Equals(Book other)
        {
            if (other == null)
                return false;

            if (Isbn == other.Isbn)
                return true;

            return false;
        }
        public override int GetHashCode()
        {
            return Isbn == null ? 0 : Isbn.GetHashCode();
        }
    }
}