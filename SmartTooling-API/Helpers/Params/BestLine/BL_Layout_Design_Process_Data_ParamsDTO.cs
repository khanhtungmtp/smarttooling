using System;

namespace SmartTooling_API.Helpers.Params.BestLine
{
    public class BL_Layout_Design_Process_Data_Params_DTO
    {

        public string line_id { get; set; }
        public string line_type_id { get; set; }
        public string model_no { get; set; }
        public string process_type_id { get; set; }
        public string before_or_after { get; set; }
    }
}