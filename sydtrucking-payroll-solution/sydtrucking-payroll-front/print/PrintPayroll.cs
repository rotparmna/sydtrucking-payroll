namespace sydtrucking_payroll_front.print
{
    using PdfSharp.Drawing;
    using sydtrucking_payroll_front.model;
    using sydtrucking_payroll_front.util;
    using System;

    public class PrintPayroll : PrintPayrollBase
    {
        public Payroll Payroll { get; set; }

        public PrintPayroll(Payroll payroll)
        {
            Payroll = payroll;
            Fullname = CreateFullname();
            ToPdf = new PrintToPdf(Fullname);
            PdfFile = new Pdf(Fullname);
        }

        protected override string CreateFullname()
        {
            Filename = Payroll.Driver.SocialSecurity.ToString() + "-" + DateTime.Now.Ticks.ToString() + ".pdf";
            return business.Constant.PathReportPayroll + "\\" + Filename;
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
            ToPdf.DrawString("Pay To: " + Payroll.Driver.Fullname, FormatText.Bold, text1X, payToY, 50, 200, XStringFormats.TopLeft);

            double truckY = 215;
            ToPdf.DrawString("TRUCK No. " + Payroll.Driver.Truck.Number, FormatText.Bold, text1X, truckY, 50, 200, XStringFormats.TopLeft);

            double paymentWeekY = 230;
            ToPdf.DrawString("Payment Week From: " + Payroll.From.Date.ToShortDateString() + " - " + Payroll.To.Date.ToShortDateString(), FormatText.Bold, text1X, paymentWeekY, 50, 200, XStringFormats.TopLeft);

            double paymentDateX = 370;
            double paymentDateY = 230;
            ToPdf.DrawString("Payment Date: " + Payroll.PaymentDate.Date.ToShortDateString(), FormatText.Regular, paymentDateX, paymentDateY, 50, 200, XStringFormats.TopLeft);
        }

        protected override void PrintDetails()
        {
            double headerDateX = 10;
            double headerY = 250;
            ToPdf.DrawString("DATE OF TICKET", FormatText.Bold, headerDateX, headerY, 50, 200, XStringFormats.Center);

            double headerCompanyX = 150;
            ToPdf.DrawString("COMPANY", FormatText.Bold, headerCompanyX, headerY, 50, 200, XStringFormats.Center);

            double headerTicketX = 300;
            ToPdf.DrawString("No TICKET", FormatText.Bold, headerTicketX, headerY, 50, 200, XStringFormats.Center);

            double headerHoursX = 380;
            ToPdf.DrawString("HOURS", FormatText.Bold, headerHoursX, headerY, 50, 200, XStringFormats.Center);

            double itemY = headerY;
            foreach (var item in Payroll.Details)
            {
                itemY += 15;
                ToPdf.DrawString(item.Ticket.Date.Date.ToShortDateString(), FormatText.Regular, headerDateX, itemY, 50, 200, XStringFormats.Center);
                ToPdf.DrawString(item.OilCompany.Name, FormatText.Regular, headerCompanyX, itemY, 50, 200, XStringFormats.Center);
                ToPdf.DrawString(item.Ticket.Number.ToString(), FormatText.Regular, headerTicketX, itemY, 50, 200, XStringFormats.Center);
                ToPdf.DrawString(item.Hours.ToString(), FormatText.Regular, headerHoursX, itemY, 50, 200, XStringFormats.Center);
            }
        }

        protected override void PrintTotals()
        {
            double totalTextX = 180;
            double totalValueX = 380;
            double totalInitY = 600;

            double totalWeekHoursY = totalInitY;
            ToPdf.DrawString("Total Week Hours " + Payroll.TotalHours.ToString(), FormatText.Bold, totalTextX, totalWeekHoursY, 50, 200, XStringFormats.TopRight);

            double totalRegularHourY = totalInitY + 15;
            ToPdf.DrawString("Regular Hours " + Payroll.RegularHour.ToString(), FormatText.Regular, totalTextX, totalRegularHourY, 50, 200, XStringFormats.TopRight);
            ToPdf.DrawString(Payroll.PaymentRegularHour.ToString("C"), FormatText.Regular, totalValueX, totalRegularHourY, 50, 200, XStringFormats.TopCenter);

            double totalOvertimeHourY = totalInitY + 30;
            ToPdf.DrawString("Overtime Hours " + Payroll.OvertimeHour.ToString(), FormatText.Regular, totalTextX, totalOvertimeHourY, 50, 200, XStringFormats.TopRight);
            ToPdf.DrawString(Payroll.PaymentOvertimeHour.ToString("C"), FormatText.Regular, totalValueX, totalOvertimeHourY, 50, 200, XStringFormats.TopCenter);

            double totalDeductionsY = totalInitY + 45;
            ToPdf.DrawString("Deductions " + Payroll.DeductionsDetail, FormatText.Regular, totalTextX, totalDeductionsY, 50, 200, XStringFormats.TopRight);
            ToPdf.DrawString(Payroll.Deductions.ToString("C"), FormatText.Regular, totalValueX, totalDeductionsY, 50, 200, XStringFormats.TopCenter);

            double totalReimbursmentsY = totalInitY + 60;
            ToPdf.DrawString("Reimbursments " + Payroll.ReimbursmentsDetail, FormatText.Regular, totalTextX, totalReimbursmentsY, 50, 200, XStringFormats.TopRight);
            ToPdf.DrawString(Payroll.Reimbursements.ToString("C"), FormatText.Regular, totalValueX, totalReimbursmentsY, 50, 200, XStringFormats.TopCenter);

            double totalY = totalInitY + 90;
            ToPdf.DrawString("TOTAL", FormatText.Bold, totalTextX, totalY, 50, 200, XStringFormats.TopRight);
            ToPdf.DrawString(Payroll.TotalPayment.ToString("C"), FormatText.Bold, totalValueX, totalY, 50, 200, XStringFormats.TopCenter);
        }
    }
}
