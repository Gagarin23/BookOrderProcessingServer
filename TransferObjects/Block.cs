using System;

namespace TransferObjects
{
    public class Block : Book, IEquatable<Block>
    {
        public string SheetFormat { get; set; }

        public bool Equals(Block other)
        {
            if (other == null)
                return false;

            if (BookFormat == other.BookFormat
                && BookMount == other.BookMount
                && SheetFormat == other.SheetFormat)
                return true;

            return false;
        }

        public override bool Equals(Book other)
        {
            return Equals(other as Block);
        }

        public override int GetHashCode()
        {
            var hashBookFormat = BookFormat == null ? 0 : BookFormat.GetHashCode();
            var hashBookMount = BookMount == null ? 0 : BookMount.GetHashCode();
            var hashSheetFormat = BookMount == null ? 0 : BookMount.GetHashCode();

            return hashBookFormat ^ hashBookMount ^ hashSheetFormat;
        }

        public override string ToString()
        {
            return Name;
        }

        public string SetSheetFormat(string bookMount, string bookFormat)
        {
            if (bookMount.Equals("кбс"))
            {
                if (bookFormat.Equals("А5"))
                {
                    if (NumberOfCopies <= 2)
                    {
                        Imposition = 2;
                        return "A4";
                    }

                    if (NumberOfCopies >= 3 && NumberOfCopies <= 4)
                    {
                        Imposition = 4;
                        return "A3";
                    }

                    if (NumberOfCopies >= 5 && NumberOfCopies <= 6)
                    {
                        Imposition = 2;
                        return "A4";
                    }

                    Imposition = 4;
                    return "A3";
                }

                if (bookFormat.Equals("А4"))
                {
                    Imposition = 2;
                    return "A3";
                }

                return null;
            }

            if (bookMount.Contains("скр", StringComparison.OrdinalIgnoreCase) || bookMount.Contains("скрепка", StringComparison.OrdinalIgnoreCase))
            {
                if (bookFormat.Equals("А5"))
                {
                    Imposition = 1;
                    return "A4";
                }

                if (bookFormat.Equals("А4"))
                {
                    Imposition = 1;
                    return "A3";
                }

                return null;
            }

            Console.WriteLine($"Крепление не определено. Значение: {BookMount}");
            return null;
        }
    }
}