namespace sydtrucking_payroll_front.print
{
    using PdfSharp.Drawing;
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.util;
    using System;

    public class PrintPayrollLeaseCompany : PrintPayrollBase
    {
        double _initialYTotals;

        public PayrollLeaseCompany Payroll { get; set; }

        public PrintPayrollLeaseCompany(PayrollLeaseCompany payroll)
        {
            Payroll = payroll;
            Fullname = CreateFullname();
            ToPdf = new PrintToPdf(Fullname);
            PdfFile = new Pdf(Fullname);
        }

        protected override string CreateFullname()
        {
            Filename = Payroll.LeaseCompany.Name.Replace(" ","").Trim() + Payroll.Date.ToString("ddMMyy") + ".pdf";
            return business.Constant.PathReportPayrollLeaseCompany + "\\" + Filename;
        }

        public new void Print(bool isOpen)
        {
            ToPdf.AddPage(PdfSharp.PageSize.Letter);

            PrintHeader();
            PrintInfo();
            double lastItemY = PrintPayrolls();
            lastItemY = PrintDetails(lastItemY);
            lastItemY = PrintDeductions(lastItemY);
            lastItemY = PrintReinmburstments(lastItemY);
            _initialYTotals = lastItemY;
            PrintTotals();

            ToPdf.Print();

            if (isOpen)
                PdfFile.Open();
        }

        protected override void PrintHeader()
        {
            double logoX = 40;
            double logoY = 70;
            ToPdf.DrawImage("resources\\images\\logo-sdt.JPG", logoX, logoY, 0.2);

            double y = 70;

            double textX = 200;
            double titleY = y;
            ToPdf.DrawString("S & D TRUCKING LLC", FormatText.Title, textX, titleY, 50, 200, XStringFormats.Center);

            double dir1Y = y + 15;
            ToPdf.DrawString("2620 Charway Rd", FormatText.Regular, textX, dir1Y, 50, 200, XStringFormats.Center);

            double dir2Y = y + 25;
            ToPdf.DrawString("Odessa, Tx 79766", FormatText.Regular, textX, dir2Y, 50, 200, XStringFormats.Center);
        }

        protected override void PrintInfo()
        {
            double text1X = 100;
            double y = 150;

            double payToY = y;
            ToPdf.DrawString("Pay To: " + Payroll.LeaseCompany.Name, FormatText.Bold, text1X, payToY, 50, 200, XStringFormats.TopLeft);

            double truckY = y + 15;
            ToPdf.DrawString("TRUCK No. " + Payroll.Truck.Number, FormatText.Bold, text1X, truckY, 50, 200, XStringFormats.TopLeft);

            double paymentWeekY = y + 30;
            ToPdf.DrawString("FROM: " + Payroll.From.Date.ToShortDateString() + " TO " + Payroll.To.Date.ToShortDateString(), FormatText.Bold, text1X, paymentWeekY, 50, 200, XStringFormats.TopLeft);

            double paymentDateX = 290;
            double paymentDateY = y + 30;
            ToPdf.DrawString("Date: " + Payroll.Date.ToShortDateString(), FormatText.Regular, paymentDateX, paymentDateY, 50, 200, XStringFormats.TopRight);
        }

        private double PrintPayrolls()
        {
            double text1X = 100;
            double text2X = 290;
            double y = 220;

            foreach (var item in Payroll.Rates)
            {
                ToPdf.DrawString(item.Companies, FormatText.Bold, text1X, y, 50, 200, XStringFormats.TopLeft);
                ToPdf.DrawString("Rate", FormatText.Regular, text1X, y + 15, 50, 200, XStringFormats.TopLeft);
                ToPdf.DrawString("Sub-Total", FormatText.Regular, text1X, y + 30, 50, 200, XStringFormats.TopLeft);

                ToPdf.DrawString(item.Hours.ToString(), FormatText.Italic, text2X, y, 50, 200, XStringFormats.TopRight);
                ToPdf.DrawString(item.Rate.ToString("C"), FormatText.Regular, text2X, y + 15, 50, 200, XStringFormats.TopRight);
                ToPdf.DrawString(item.Subtotal.ToString("C"), FormatText.BoldItalic, text2X, y + 30, 50, 200, XStringFormats.TopRight);

                y += 60;
            }

            return y;
        }

        private double PrintDetails(double initialY)
        {
            double text1X = 100;
            double text2X = 290;
            double y = initialY;

            foreach (var item in Payroll.Details)
            {
                ToPdf.DrawString(item.Item, FormatText.Bold, text1X, y, 50, 200, XStringFormats.TopLeft);
                ToPdf.DrawString(item.Value.ToString("C"), FormatText.Regular, text2X, y, 50, 200, XStringFormats.TopRight);

                y += 15;
            }

            return y;
        }

        private double PrintDeductions(double initialY)
        {
            double text1X = 100;
            double text2X = 290;
            double y = initialY + 40;

            ToPdf.DrawString("Deductions", FormatText.Bold, text1X, y, 50, 200, XStringFormats.TopLeft);
            y += 15;

            foreach (var item in Payroll.Deductions)
            {
                ToPdf.DrawString(item.Item, FormatText.Regular, text1X, y, 50, 200, XStringFormats.TopLeft);
                ToPdf.DrawString(item.Value.ToString("C"), FormatText.Regular, text2X, y, 50, 200, XStringFormats.TopRight);

                y += 15;
            }

            return y;
        }

        private double PrintReinmburstments(double initialY)
        {
            double text1X = 100;
            double text2X = 290;
            double y = initialY + 40;
            
            ToPdf.DrawString("Reinmburstments", FormatText.Bold, text1X, y, 50, 200, XStringFormats.TopLeft);
            y += 15;

            foreach (var item in Payroll.Reimbursements)
            {
                ToPdf.DrawString(item.Item, FormatText.Regular, text1X, y, 50, 200, XStringFormats.TopLeft);
                ToPdf.DrawString(item.Value.ToString("C"), FormatText.Regular, text2X, y, 50, 200, XStringFormats.TopRight);

                y += 15;
            }

            return y;
        }

        protected override void PrintTotals()
        {
            double text1X = 100;
            double text2X = 290;
            double y = _initialYTotals + 50;

            ToPdf.DrawString("Total Payment", FormatText.Bold, text1X, y, 50, 200, XStringFormats.TopLeft);
            ToPdf.DrawString(Payroll.Total.ToString("C"), FormatText.Bold, text2X, y, 50, 200, XStringFormats.TopRight);
        }

        protected override void PrintDetails()
        {
        }
    }
}
