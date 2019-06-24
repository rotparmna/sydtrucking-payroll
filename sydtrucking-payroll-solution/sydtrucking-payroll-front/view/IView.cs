namespace sydtrucking_payroll_front.view
{
    public interface IView<T>
    {
        void ClearView();
        void CreateView();
        void SaveView();
        void FillGrid();
        void EditView();
        void DeleteView(T data);
        void LoadDataBySelectedRow(T data);
        void ChangeControlsEnabled(bool isEnable);
    }
}
