namespace sydtrucking_payroll_front.print
{
    using PdfSharp.Drawing;
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.util;
    using System;

    public class PrintPayrollEmployee
    {
        PrintToPdf _toPdf;
        IFile _pdfFile;

        public string Fullname { get; private set; }
        public string Filename { get; private set; }
        public PayrollEmployee Payroll { get; set; }

        public PrintPayrollEmployee(PayrollEmployee payroll)
        {
            Payroll = payroll;
            Fullname = CreateFullname();
            _toPdf = new PrintToPdf(Fullname);
            _pdfFile = new Pdf(Fullname);
        }

        private string CreateFullname()
        {
            Filename = Payroll.Employee.SocialSecurity.ToString() + "-" + DateTime.Now.Ticks.ToString() + ".pdf";
            return business.Constant.PathReportPayrollEmployee + "\\" + Filename;
        }

        public void Print(bool isOpen)
        {
            _toPdf.AddPage(PdfSharp.PageSize.Letter);

            PrintHeader();
            PrintInfo();
            PrintTotals();

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

        private void PrintInfo()
        {
            double text1X = 120;

            double payToY = 200;
            _toPdf.DrawString("Pay To: " + Payroll.Employee.Fullname, FormatText.Bold, text1X, payToY, 50, 200, XStringFormats.TopLeft);

            double paymentWeekY = 215;
            _toPdf.DrawString("Payment Week From: " + Payroll.From.Date.ToShortDateString() + " - " + Payroll.To.Date.ToShortDateString(), FormatText.Bold, text1X, paymentWeekY, 50, 200, XStringFormats.TopLeft);

            double paymentDateX = 370;
            double paymentDateY = 215;
            _toPdf.DrawString("Payment Date: " + Payroll.PaymentDate.Date.ToShortDateString(), FormatText.Regular, paymentDateX, paymentDateY, 50, 200, XStringFormats.TopLeft);
        }

        private void PrintTotals()
        {
            double totalTextX = 180;
            double totalValueX = 380;

            double totalWeekHoursY = 350;
            _toPdf.DrawString("Total Week Hours " + Payroll.TotalHours.ToString(), FormatText.Bold, totalTextX, totalWeekHoursY, 50, 200, XStringFormats.TopRight);

            double totalRegularHourY = 365;
            _toPdf.DrawString("Hours " + Payroll.TotalHours.ToString(), FormatText.Regular, totalTextX, totalRegularHourY, 50, 200, XStringFormats.TopRight);
            _toPdf.DrawString(Payroll.PaymentTotalHours.ToString("C"), FormatText.Regular, totalValueX, totalRegularHourY, 50, 200, XStringFormats.TopCenter);

            double totalDeductionsY = 380;
            _toPdf.DrawString("Deductions " + Payroll.DeductionsDetail, FormatText.Regular, totalTextX, totalDeductionsY, 50, 200, XStringFormats.TopRight);
            _toPdf.DrawString(Payroll.Deductions.ToString("C"), FormatText.Regular, totalValueX, totalDeductionsY, 50, 200, XStringFormats.TopCenter);

            double totalReimbursmentsY = 395;
            _toPdf.DrawString("Reimbursments " + Payroll.ReimbursmentsDetail, FormatText.Regular, totalTextX, totalReimbursmentsY, 50, 200, XStringFormats.TopRight);
            _toPdf.DrawString(Payroll.Reimbursements.ToString("C"), FormatText.Regular, totalValueX, totalReimbursmentsY, 50, 200, XStringFormats.TopCenter);

            double totalY = 410;
            _toPdf.DrawString("TOTAL", FormatText.Bold, totalTextX, totalY, 50, 200, XStringFormats.TopRight);
            _toPdf.DrawString(Payroll.TotalPayment.ToString("C"), FormatText.Bold, totalValueX, totalY, 50, 200, XStringFormats.TopCenter);
        }
    }
}
