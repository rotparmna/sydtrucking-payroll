namespace sydtrucking_payroll_front.print
{
    using sydtrucking_payroll_front.util;

    public abstract class PrintPayrollBase
    {
        public string Fullname { get; protected set; }
        public string Filename { get; protected set; }
        protected PrintToPdf ToPdf { get; set; }
        protected Pdf PdfFile { get; set; }

        public PrintPayrollBase()
        {
            
        }

        protected abstract string CreateFullname();

        public void Print(bool isOpen)
        {
            ToPdf.AddPage(PdfSharp.PageSize.Letter);

            PrintHeader();
            PrintInfo();
            PrintDetails();
            PrintTotals();

            ToPdf.Print();

            if (isOpen)
                PdfFile.Open();
        }

        protected abstract void PrintHeader();
        protected abstract void PrintInfo();
        protected abstract void PrintDetails();
        protected abstract void PrintTotals();
    }
}
