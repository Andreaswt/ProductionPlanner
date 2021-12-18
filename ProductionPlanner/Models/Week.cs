using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using Microsoft.EntityFrameworkCore.Migrations.Operations;

namespace ProductionPlanner.Models
{
    public class Week
    {
        [Key]
        public int Id { get; set; }
        public List<Day> Days { get; set; }
        public bool Ferie { get; set; }
        public int WeekNo { get; set; }
        public int Year { get; set; }
    }
}