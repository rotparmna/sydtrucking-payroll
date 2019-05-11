namespace sydtrucking_payroll_front.business
{
    using System.Collections.Generic;

    interface IBusiness<T>
    {
        List<T> GetAll();

        void Update(T model);
    }
}
