namespace sydtrucking_payroll_front.print
{
    using PdfSharp.Drawing;
    using sydtrucking_payroll_front.model;
    using System;

    public class PrintPayroll
    {
        PrintToPdf _toPdf;

        public string FileName { get; private set; }
        public Payroll Payroll { get; set; }

        public PrintPayroll(Payroll payroll)
        {
            Payroll = payroll;
            FileName = CreateFileName();
            _toPdf = new PrintToPdf(FileName);
        }

        private string CreateFileName()
        {
            return business.Constant.PathReportPayroll + "\\" + 
                    Payroll.Employee.SocialSecurity.ToString() + "-" + 
                    DateTime.Now.Ticks.ToString() + ".pdf";
        }

        public void Print()
        {
            _toPdf.AddPage(PdfSharp.PageSize.Letter);

            PrintHeader();
            PrintInfo();
            PrintDetails();
            PrintTotals();

            _toPdf.Print();
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

            double truckY = 215;
            _toPdf.DrawString("TRUCK No. " + Payroll.Employee.Truck.Number, FormatText.Bold, text1X, truckY, 50, 200, XStringFormats.TopLeft);

            double paymentWeekY = 230;
            _toPdf.DrawString("Payment Week From: " + Payroll.From.Date.ToShortDateString() + " - " + Payroll.To.Date.ToShortDateString(), FormatText.Bold, text1X, paymentWeekY, 50, 200, XStringFormats.TopLeft);

            double paymentDateX = 370;
            double paymentDateY = 230;
            _toPdf.DrawString("Payment Date: " + Payroll.PaymentDate.Date.ToShortDateString(), FormatText.Regular, paymentDateX, paymentDateY, 50, 200, XStringFormats.TopLeft);
        }

        private void PrintDetails()
        {
            double headerDateX = 10;
            double headerY = 350;
            _toPdf.DrawString("DATE OF TICKET", FormatText.Bold, headerDateX, headerY, 50, 200, XStringFormats.Center);

            double headerCompanyX = 150;
            _toPdf.DrawString("COMPANY", FormatText.Bold, headerCompanyX, headerY, 50, 200, XStringFormats.Center);

            double headerTicketX = 300;
            _toPdf.DrawString("No TICKET", FormatText.Bold, headerTicketX, headerY, 50, 200, XStringFormats.Center);

            double headerHoursX = 380;
            _toPdf.DrawString("HOURS", FormatText.Bold, headerHoursX, headerY, 50, 200, XStringFormats.Center);

            double itemY = headerY;
            foreach (var item in Payroll.Details)
            {
                itemY += 15;
                _toPdf.DrawString(item.Ticket.Date.Date.ToShortDateString(), FormatText.Regular, headerDateX, itemY, 50, 200, XStringFormats.Center);
                _toPdf.DrawString(item.Company, FormatText.Regular, headerCompanyX, itemY, 50, 200, XStringFormats.Center);
                _toPdf.DrawString(item.Ticket.Number.ToString(), FormatText.Regular, headerTicketX, itemY, 50, 200, XStringFormats.Center);
                _toPdf.DrawString(item.Hours.ToString(), FormatText.Regular, headerHoursX, itemY, 50, 200, XStringFormats.Center);
            }
        }

        private void PrintTotals()
        {
            double totalTextX = 180;
            double totalValueX = 380;

            double totalWeekHoursY = 530;
            _toPdf.DrawString("Total Week Hours " + Payroll.TotalHours.ToString(), FormatText.Bold, totalTextX, totalWeekHoursY, 50, 200, XStringFormats.TopRight);

            double totalRegularHourY = 545;
            _toPdf.DrawString("Regular Hours " + Payroll.RegularHour.ToString(), FormatText.Regular, totalTextX, totalRegularHourY, 50, 200, XStringFormats.TopRight);
            _toPdf.DrawString(Payroll.PaymentRegularHour.ToString("C"), FormatText.Regular, totalValueX, totalRegularHourY, 50, 200, XStringFormats.TopCenter);

            double totalOvertimeHourY = 560;
            _toPdf.DrawString("Overtime Hours " + Payroll.OvertimeHour.ToString(), FormatText.Regular, totalTextX, totalOvertimeHourY, 50, 200, XStringFormats.TopRight);
            _toPdf.DrawString(Payroll.PaymentOvertimeHour.ToString("C"), FormatText.Regular, totalValueX, totalOvertimeHourY, 50, 200, XStringFormats.TopCenter);

            double totalDeductionsY = 575;
            _toPdf.DrawString("Deductions " + Payroll.DeductionsDetail, FormatText.Regular, totalTextX, totalDeductionsY, 50, 200, XStringFormats.TopRight);
            _toPdf.DrawString(Payroll.Deductions.ToString("C"), FormatText.Regular, totalValueX, totalDeductionsY, 50, 200, XStringFormats.TopCenter);

            double totalReimbursmentsY = 590;
            _toPdf.DrawString("Reimbursments " + Payroll.ReimbursmentsDetail, FormatText.Regular, totalTextX, totalReimbursmentsY, 50, 200, XStringFormats.TopRight);
            _toPdf.DrawString(Payroll.Reimbursements.ToString("C"), FormatText.Regular, totalValueX, totalReimbursmentsY, 50, 200, XStringFormats.TopCenter);

            double totalY = 620;
            _toPdf.DrawString("TOTAL", FormatText.Bold, totalTextX, totalY, 50, 200, XStringFormats.TopRight);
            _toPdf.DrawString(Payroll.TotalPayment.ToString("C"), FormatText.Bold, totalValueX, totalY, 50, 200, XStringFormats.TopCenter);
        }
    }
}
