using System;
using System.Collections.Generic;
using ProductionPlanner.Models;

namespace ProductionPlanner.Interfaces
{
    public interface IDataService
    {
        public void SaveWeeksToDb(List<Week> weeks);
        public List<Week> GetAllWeeks();
    }
}