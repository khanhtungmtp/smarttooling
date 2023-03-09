namespace SmartTooling_API.Helpers.Params.ProductionBP
{
    public class BondingReportParam
    {
        public string factory { get; set; }
        public string model { get; set; }
        public string prod_season { get; set; }
        public string chemical_process_type_id { get; set; }
        public string auto_tech_id { get; set; }
    }
}