using System.Collections.Generic;

namespace SmartTooling_API.DTO.BestLine
{
    public class C2BLayoutByProcessDetailDTO
    {
        public List<decimal> listDataBefore { get; set; }
        public List<decimal> listTaktTimeBefore { get; set; }
        public List<decimal> listEmployeeBefore { get; set; }
        public List<string> listNodeNameBefore { get; set; }
        public List<decimal> listDataAfter { get; set; }
        public List<decimal> listTaktTimeAfter { get; set; }
        public List<decimal> listEmployeeAfter { get; set; }
        public List<string> listNodeNameAfter { get; set; }
        public string typeName { get; set; }
    }
}