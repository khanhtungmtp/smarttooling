using System;

namespace SmartTooling_API.DTO.BestLine
{
    public class BL_Layout_Design_OverallDTO
    {
        public string factory_id { get; set; }
        public string line_id { get; set; }
        public string line_type_id { get; set; }
        public string model_no { get; set; }
        public string prod_season { get; set; }
        public decimal? no_of_process_before { get; set; }
        public decimal? no_of_process_after { get; set; }
        public decimal tct_before { get; set; }
        public decimal tct_after { get; set; }
        public decimal cps_mp_before { get; set; }
        public decimal cps_mp_after { get; set; }
        public decimal assembly_mp_before { get; set; }
        public decimal assembly_mp_after { get; set; }
        public decimal eolr_before { get; set; }
        public decimal eolr_after { get; set; }
        public decimal ller_before_percent { get; set; }
        public decimal ller_after_percent { get; set; }
        public decimal tentative_pph_before { get; set; }
        public decimal tentative_pph_after { get; set; }
        public decimal? tentative_efficiency_before_percent { get; set; }
        public decimal? tentative_efficiency_after_percent { get; set; }
        public string c2b_overall_image { get; set; }
        public string create_by { get; set; }
        public DateTime create_time { get; set; }
        public string update_by { get; set; }
        public DateTime update_time { get; set; }
        public Guid guid { get; set; }
    }
}