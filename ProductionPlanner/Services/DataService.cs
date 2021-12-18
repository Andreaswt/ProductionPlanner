using System.Collections.Generic;
using System.Linq;
using ProductionPlanner.Data;
using ProductionPlanner.Interfaces;
using ProductionPlanner.Models;

namespace ProductionPlanner.Services
{
    public class DataService : IDataService
    {
        private readonly ApplicationDbContext _db;

        public DataService(ApplicationDbContext applicationDbContext)
        {
            _db = applicationDbContext;
        }
        
        public void SaveWeeks(List<Week> weeks)
        {
            _db.Weeks.AddRange(weeks);
            _db.SaveChanges();
        }

        public List<Week> GetWeeks()
        {
            return _db.Weeks.ToList();
        }
    }
}