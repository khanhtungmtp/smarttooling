namespace SmartTooling_API.Models.BestLine
{
    public class Pilot_Line_Setup_Summary_Tracking
    {
        public string factory { get; set; }
        public string prod_season { get; set; }
        public decimal target_number_of_pilot_lines_setup { get; set; }
        public decimal actual_number_of_pilot_lines_setup { get; set; }
        public decimal pilot_lines_setup_percent { get; set; }
        public string pilot_line { get; set; }
        public int target_number_of_rollout_lines_setup_for_pilot_lines { get; set; }
        public int actual_number_of_rollout_lines_setup_for_pilot_lines { get; set; }
        public decimal rollout_lines_setup_percent_for_pilot_lines { get; set; }

    }

    public class Pilot_Line_Setup_Summary_Tracking_Report
    {
        public string Factory { get; set; }
        public List<Pilot_Line_Setup_Summary_Tracking> Data { get; set; }
    }
}