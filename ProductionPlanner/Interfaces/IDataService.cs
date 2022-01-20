using System.Collections.Generic;
using ProductionPlanner.Enums;
using ProductionPlanner.Models;

namespace ProductionPlanner.Interfaces
{
    public interface IDataService
    {
        public void SaveWeeks(List<Week> weeks);
        public List<Week> GetWeeks();
        public bool SaveProjectTemplate(ProjectTemplate projectTemplate);
        public void EditProjectTemplate(ProjectTemplate projectTemplate);
        public void DeleteProjectTemplate(int id);
        public bool CreateProject(ProjectTemplate projectTemplate);
        public void UpdateProjectProgress(int id, ProjectProgress projectProgress);
        public void UpdateProjectPriority(int id, int projectPriority);
        public void UpdateProjectTaskProgress(int id, ProjectTaskProgress projectProgress);
    }
}