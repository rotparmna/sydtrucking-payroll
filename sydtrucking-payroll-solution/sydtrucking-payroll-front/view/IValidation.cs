namespace sydtrucking_payroll_front.view
{
    public interface IValidation
    {
        string ValidationMessage { get; set; }
        bool IsViewValid();
    }
}
