using System;

namespace TransferObjects
{
    public class Cover : Book, IEquatable<Cover>
    {
        public string Lamination { get; set; }

        public bool Equals(Cover other)
        {
            if (other == null)
                return false;

            if (Lamination == other.Lamination
                && BookFormat == other.BookFormat
                && BookMount == other.BookMount
                && Imposition == other.Imposition)
                return true;

            return false;
        }

        public override bool Equals(Book other)
        {
            return Equals(other as Cover);
        }

        public override int GetHashCode()
        {
            var hashLamination = Lamination == null ? 0 : Lamination.GetHashCode();
            var hashBookFormat = BookFormat == null ? 0 : BookFormat.GetHashCode();
            var hashBookMount = BookMount == null ? 0 : BookMount.GetHashCode();
            var hashImposition = Imposition.GetHashCode();

            return hashLamination ^ hashBookFormat ^ hashBookMount ^ hashImposition;
        }
    }
}