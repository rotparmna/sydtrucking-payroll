namespace sydtrucking_payroll_front.print
{
    using PdfSharp.Drawing;
    using sydtrucking_payroll_front.model;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

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

            double logoX = 20;
            double logoY = 80;
            _toPdf.DrawImage("resources\\images\\logo-sdt.JPG", logoX, logoY, 0.2);

            double titleX = 200;
            double titleY = 80;
            _toPdf.DrawString("S & D TRUCKING LLC", FormatText.Title, titleX, titleY, 50, 200, XStringFormats.Center);

            double dir1X = 200;
            double dir1Y = 95;
            _toPdf.DrawString("2620 Charway Rd", FormatText.Regular, dir1X, dir1Y, 50, 200, XStringFormats.Center);

            double dir2X = 200;
            double dir2Y = 105;
            _toPdf.DrawString("Odessa, Tx 79766", FormatText.Regular, dir2X, dir2Y, 50, 200, XStringFormats.Center);

            double payToX = 120;
            double payToY = 200;
            _toPdf.DrawString("Pay To: "+ Payroll.Employee.Fullname, FormatText.Bold, payToX, payToY, 50, 200, XStringFormats.TopLeft);

            double truckX = 120;
            double truckY = 215;
            _toPdf.DrawString("TRUCK No. " + Payroll.Employee.Truck.Number, FormatText.Bold, truckX, truckY, 50, 200, XStringFormats.TopLeft);

            double paymentWeekX = 120;
            double paymentWeekY = 230;
            _toPdf.DrawString("Payment Week From: " + Payroll.From.Date.ToShortDateString() +" - " +Payroll.To.Date.ToShortDateString(), FormatText.Bold, paymentWeekX, paymentWeekY, 50, 200, XStringFormats.TopLeft);

            double paymentDateX = 370;
            double paymentDateY = 230;
            _toPdf.DrawString("Payment Date: " + Payroll.PaymentDate.Date.ToShortDateString(), FormatText.Regular, paymentDateX, paymentDateY, 50, 200, XStringFormats.TopLeft);

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

            double totalWeekHoursX = 180;
            double totalWeekHoursY = 530;
            _toPdf.DrawString("Total Week Hours", FormatText.Regular, totalWeekHoursX, totalWeekHoursY, 50, 200, XStringFormats.TopRight);

            double totalWeekHours2X = 380;
            double totalWeekHours2Y = 530;
            _toPdf.DrawString(Payroll.Details.Sum(x=>x.Hours).ToString(), FormatText.Bold, totalWeekHours2X, totalWeekHours2Y, 50, 100, XStringFormats.Center);

            double totalRegularHourX = 180;
            double totalRegularHourY = 545;
            _toPdf.DrawString("Regular Hours", FormatText.Regular, totalRegularHourX, totalRegularHourY, 50, 200, XStringFormats.TopRight);

            double totalRegularHour2X = 300;
            double totalRegularHour2Y = 545;
            _toPdf.DrawString(Payroll.RegularHour.ToString(), FormatText.Regular, totalRegularHour2X, totalRegularHour2Y, 50, 100, XStringFormats.Center);

            double totalOvertimeHourX = 180;
            double totalOvertimeHourY = 560;
            _toPdf.DrawString("Overtime Hours", FormatText.Regular, totalOvertimeHourX, totalOvertimeHourY, 50, 200, XStringFormats.TopRight);

            double totalOvertimeHour2X = 300;
            double totalOvertimeHour2Y = 560;
            _toPdf.DrawString(Payroll.OvertimeHour.ToString(), FormatText.Regular, totalOvertimeHour2X, totalOvertimeHour2Y, 50, 100, XStringFormats.Center);

            double totalOvertimeHour3X = 380;
            double totalOvertimeHour3Y = 560;
            _toPdf.DrawString(Payroll.PaymentOvertimeHour.ToString("$"), FormatText.Regular, totalOvertimeHour3X, totalOvertimeHour3Y, 50, 100, XStringFormats.TopRight);

            _toPdf.Print();
        }
    }
}
