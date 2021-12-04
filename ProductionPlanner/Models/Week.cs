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
        public Week()
        {
            Days = new List<Day>
            {
                new Day { DayName = "Monday", Priority = 1, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 1), Tasks = new List<ProjectTask>()},
                new Day { DayName = "Tuesday", Priority = 2, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 2), Tasks = new List<ProjectTask>() },
                new Day { DayName = "Wednesday", Priority = 3, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 3), Tasks = new List<ProjectTask>() },
                new Day { DayName = "Thursday", Priority = 4, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 4), Tasks = new List<ProjectTask>() },
                new Day { DayName = "Friday", Priority = 5, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 5), Tasks = new List<ProjectTask>() },
                /*new Day { DayName = "Saturday", Priority = 6, AvailableHours = 0, HoursLeftToBook = 0, Date = new DateTime(2021, 12, 6) },
                new Day { DayName = "Sunday", Priority = 7, AvailableHours = 0, HoursLeftToBook = 0, Date = new DateTime(2021, 12, 7) },*/
            };
        }
        
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public Guid Guid { get; set; } = Guid.NewGuid();
        public List<Day> Days { get; set; }
        public List<Day> SortedDays
        {
            get
            {
                return Days.OrderBy(d => d.Priority).ToList();
            }
        }
        public int TotalHoursLeftToBook
        {
            get
            {
                var totalHours = 0;
                foreach (Day day in Days)
                {
                    totalHours += day.HoursLeftToBook;
                }

                return totalHours;
            }
        }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public bool Ferie { get; set; }
        public int WeekNo { get; set; }
        public int Year { get; set; }
    }
}