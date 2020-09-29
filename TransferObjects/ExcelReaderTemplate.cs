using System;

namespace TransferObjects
{
    public class ExcelReaderTemplate
    {
        public ushort Isbn { get; set; }
        public ushort Name { get; set; }
        public ushort NumberOfCopies { get; set; }
        public ushort BookFormat { get; set; }
        public ushort BookMount { get; set; }
        public ushort Lamination { get; set; }
        public ushort Imposition { get; set; }
    }
}