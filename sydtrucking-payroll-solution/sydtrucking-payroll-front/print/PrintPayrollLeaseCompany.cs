namespace sydtrucking_payroll_front.print
{
    using PdfSharp.Drawing;
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.util;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    public class PrintPayrollLeaseCompany
    {
        PrintToPdf _toPdf;
        IFile _pdfFile;

        public string Fullname { get; private set; }
        public string Filename { get; private set; }
        public PayrollLeaseCompany Payroll { get; set; }

        public PrintPayrollLeaseCompany(PayrollLeaseCompany payroll)
        {
            Payroll = payroll;
            Fullname = CreateFullname();
            _toPdf = new PrintToPdf(Fullname);
            _pdfFile = new Pdf(Fullname);
        }

        private string CreateFullname()
        {
            Filename = Payroll.LeaseCompany.Name.Trim() + "-" + Payroll.Truck.Number.ToString() + "-" + DateTime.Now.Ticks.ToString() + ".pdf";
            return business.Constant.PathReportPayrollLeaseCompany + "\\" + Filename;
        }

        public void Print(bool isOpen)
        {
            _toPdf.AddPage(PdfSharp.PageSize.Letter);

            PrintHeader();
            //PrintInfo();
            //PrintDetails();
            //PrintTotals();

            _toPdf.Print();

            if (isOpen)
                _pdfFile.Open();
        }

        private void PrintHeader()
        {
            double logoX = 20;
            double logoY = 80;
            _toPdf.DrawImage("resources\\images\\logo-sdt.JPG", logoX, logoY, 0.2);

            double textX = 200;
            double titleY = 80;
            _toPdf.DrawString("S & D TRUCKING LLC", FormatText.Title, textX, titleY, 50, 200, XStringFormats.Center);

            double dir1Y = 95;
            _toPdf.DrawString("2620 Charway Rd", FormatText.Regular, textX, dir1Y, 50, 200, XStringFormats.Center);

            double dir2Y = 105;
            _toPdf.DrawString("Odessa, Tx 79766", FormatText.Regular, textX, dir2Y, 50, 200, XStringFormats.Center);
        }
    }
}
