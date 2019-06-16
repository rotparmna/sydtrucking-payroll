namespace sydtrucking_payroll_front.util
{
    using System;

    public class Pdf : IFile
    {
        public string Fullname { get; set; }

        public Pdf(string fullname)
        {
            Fullname = fullname;
        }

        public void Open()
        {
            System.Diagnostics.Process process = new System.Diagnostics.Process();
            string path = AppDomain.CurrentDomain.BaseDirectory + Fullname;
            Uri pdf = new Uri(path, UriKind.RelativeOrAbsolute);
            process.StartInfo.FileName = pdf.LocalPath;
            process.Start();
        }
    }
}
