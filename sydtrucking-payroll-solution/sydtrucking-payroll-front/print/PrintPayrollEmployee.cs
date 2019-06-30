namespace sydtrucking_payroll_front.print
{
    using PdfSharp.Drawing;
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.util;
    using System;

    public class PrintPayrollEmployee : PrintPayrollBase
    {
        public PayrollEmployee Payroll { get; set; }

        public PrintPayrollEmployee(PayrollEmployee payroll)
        {
            Payroll = payroll;
            Fullname = CreateFullname();
            ToPdf = new PrintToPdf(Fullname);
            PdfFile = new Pdf(Fullname);
        }

        protected override string CreateFullname()
        {
            Filename = Payroll.Employee.SocialSecurity.ToString() + "-" + DateTime.Now.Ticks.ToString() + ".pdf";
            return business.Constant.PathReportPayrollEmployee + "\\" + Filename;
        }

        protected override void PrintHeader()
        {
            double logoX = 20;
            double logoY = 80;
            ToPdf.DrawImage("resources\\images\\logo-sdt.JPG", logoX, logoY, 0.2);

            double textX = 200;
            double titleY = 80;
            ToPdf.DrawString("S & D TRUCKING LLC", FormatText.Title, textX, titleY, 50, 200, XStringFormats.Center);

            double dir1Y = 95;
            ToPdf.DrawString("2620 Charway Rd", FormatText.Regular, textX, dir1Y, 50, 200, XStringFormats.Center);

            double dir2Y = 105;
            ToPdf.DrawString("Odessa, Tx 79766", FormatText.Regular, textX, dir2Y, 50, 200, XStringFormats.Center);
        }

        protected override void PrintInfo()
        {
            double text1X = 120;

            double payToY = 200;
            ToPdf.DrawString("Pay To: " + Payroll.Employee.Fullname, FormatText.Bold, text1X, payToY, 50, 200, XStringFormats.TopLeft);

            double paymentWeekY = 215;
            ToPdf.DrawString("Payment Week From: " + Payroll.From.Date.ToShortDateString() + " - " + Payroll.To.Date.ToShortDateString(), FormatText.Bold, text1X, paymentWeekY, 50, 200, XStringFormats.TopLeft);

            double paymentDateX = 370;
            double paymentDateY = 215;
            ToPdf.DrawString("Payment Date: " + Payroll.PaymentDate.Date.ToShortDateString(), FormatText.Regular, paymentDateX, paymentDateY, 50, 200, XStringFormats.TopLeft);
        }

        protected override void PrintTotals()
        {
            double totalTextX = 180;
            double totalValueX = 380;

            double totalWeekHoursY = 350;
            ToPdf.DrawString("Total Week Hours " + Payroll.TotalHours.ToString(), FormatText.Bold, totalTextX, totalWeekHoursY, 50, 200, XStringFormats.TopRight);

            double totalRegularHourY = 365;
            ToPdf.DrawString("Hours " + Payroll.TotalHours.ToString(), FormatText.Regular, totalTextX, totalRegularHourY, 50, 200, XStringFormats.TopRight);
            ToPdf.DrawString(Payroll.PaymentTotalHours.ToString("C"), FormatText.Regular, totalValueX, totalRegularHourY, 50, 200, XStringFormats.TopCenter);

            double totalDeductionsY = 380;
            ToPdf.DrawString("Deductions " + Payroll.DeductionsDetail, FormatText.Regular, totalTextX, totalDeductionsY, 50, 200, XStringFormats.TopRight);
            ToPdf.DrawString(Payroll.Deductions.ToString("C"), FormatText.Regular, totalValueX, totalDeductionsY, 50, 200, XStringFormats.TopCenter);

            double totalReimbursmentsY = 395;
            ToPdf.DrawString("Reimbursments " + Payroll.ReimbursmentsDetail, FormatText.Regular, totalTextX, totalReimbursmentsY, 50, 200, XStringFormats.TopRight);
            ToPdf.DrawString(Payroll.Reimbursements.ToString("C"), FormatText.Regular, totalValueX, totalReimbursmentsY, 50, 200, XStringFormats.TopCenter);

            double totalY = 410;
            ToPdf.DrawString("TOTAL", FormatText.Bold, totalTextX, totalY, 50, 200, XStringFormats.TopRight);
            ToPdf.DrawString(Payroll.TotalPayment.ToString("C"), FormatText.Bold, totalValueX, totalY, 50, 200, XStringFormats.TopCenter);
        }

        protected override void PrintDetails()
        {
            
        }
    }
}
