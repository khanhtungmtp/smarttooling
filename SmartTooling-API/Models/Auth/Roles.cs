using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SmartTooling_API.Models.Auth
{
    public class Roles
    {
        [Key]
        public string role_unique { get; set; }

        public string role_name { get; set; }

        public string role_note { get; set; }

        public double role_sequence { get; set; }

        public string update_by { get; set; }

        public DateTime update_time { get; set; }
    }
}