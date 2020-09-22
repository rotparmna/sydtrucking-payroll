namespace sydtrucking_payroll_front.business
{
    using System;
    using System.Collections.Generic;

    public interface IPayroll<T, K>
    {
        List<T> GetListPayroll(DateTime? from, DateTime? to, K entity);
    }
}
