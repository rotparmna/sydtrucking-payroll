namespace sydtrucking_payroll_front.view
{
    using sydtrucking_payroll_front.business;
    using sydtrucking_payroll_front.model;
    using System.Windows;

    public static class PayrollDelete
    {
        public static void Delete<B>(IBusiness<B> business, string id, IReportPayrollView viewReportPayroll) where B : ModelBase
        {
            if (MessageBox.Show("Are you sure delete payroll?", "Delete", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                var payroll = business.Get(id) as ModelBase;

                payroll.IsDetele = true;
                business.Update((B)payroll);

                viewReportPayroll.SearchPayrolls();
            }
        }
    }
}
