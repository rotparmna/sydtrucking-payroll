namespace sydtrucking_payroll_front.business
{
    using System.Collections.Generic;

    public interface IBusiness<T>
    {
        List<T> GetAll();

        void Update(T model);

        T Get(string id);
    }
}
