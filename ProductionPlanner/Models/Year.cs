using System.Collections.Generic;

namespace ProductionPlanner.Models
{
    public class Year
    {
        public List<Week> Weeks { get; set; }

        public int YearNo { get; set; }

        public List<Project> AssignProjects()
        {
            List<Project> projectsRemaining = new List<Project>();
            return projectsRemaining;
        }

        public List<Project> GetAllProjectsThisYear()
        {
            List<Project> projects = new List<Project>();

            foreach (Week week in Weeks)
            {
                foreach (Project project in week.Projects)
                {
                    projects.Add(project);
                }
            }
            return projects;
        }
    }
}