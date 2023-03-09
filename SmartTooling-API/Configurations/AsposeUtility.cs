namespace SmartTooling_API.Configurations
{
    public static class AsposeUtility
    {
        public static void Install()
        {
            Aspose.Cells.License cellLicense = new Aspose.Cells.License();
            string filePath = Directory.GetCurrentDirectory() + "\\Resources\\" + "Aspose.Total.lic";
            cellLicense.SetLicense(filePath);
        }
    }
}