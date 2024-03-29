using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Google.DataTable.Net.Wrapper;
using Google.DataTable.Net.Wrapper.Extension;
using ProductionPlanner.Data;
using ProductionPlanner.Interfaces;
using ProductionPlanner.Models;

namespace ProductionPlanner.Services
{
    public class PlannerService : IPlannerService
    {
        public List<Week> AssignProjects(List<Project> projects)
        {
            var sortedProjects = projects.OrderBy(t => t.Priority).ToList();
            var sortedTasks = GetSortedTasks(sortedProjects);

            List<Week> weeks = new();

            Week week = AddNewWeek();
            ProjectTask subtask = null;
            foreach (ProjectTask task in sortedTasks)
            {
                subtask = task;
                while (subtask != null)
                {
                    if (!subtask.Assigned)
                    {
                        // Check if all days are assigned, otherwise add new week
                        if (GetTotalHoursLeftToBook(week) == 0)
                        {
                            weeks.Add(week);
                            week = AddNewWeek();
                        }

                        foreach (Day day in GetSortedDays(week.Days))
                        {
                            if (day.HoursLeftToBook == 0)
                                continue;
                            
                            // Assign task to day
                            if (day.HoursLeftToBook >= subtask.Duration)
                            {
                                subtask.Assigned = true;
                                day.Tasks.Add(subtask);
                                day.HoursLeftToBook -= subtask.Duration;
                                
                                subtask = null;
                                
                                break;
                            }
                            else
                            {
                                var totalTaskHours = subtask.Duration;
                                var hoursBookedToday = day.HoursLeftToBook;
                                subtask.Subtask = true;
                                subtask.Duration = day.HoursLeftToBook;
                                day.HoursLeftToBook -= day.HoursLeftToBook;
                                subtask.Assigned = true;
                                
                                day.Tasks.Add(subtask);
                                
                                // Deep clone subtask
                                ProjectTask s = new ProjectTask
                                {
                                    Name = subtask.Name,
                                    ProjectName = subtask.ProjectName,
                                    Progress = subtask.Progress,
                                    Description = subtask.Description,
                                    Priority = subtask.Priority,
                                    Duration = subtask.Duration,
                                    Date = subtask.Date,
                                    Assigned = subtask.Assigned,
                                    PersonAssigned = subtask.PersonAssigned,
                                    Subtask = subtask.Subtask
                                };
                                subtask = s;
                                subtask.Assigned = false;
                                subtask.Duration = totalTaskHours - hoursBookedToday;

                                break;
                            }
                        }
                    }
                }
            }
            
            // If a week was partly filled, it wasn't added in the above foreach
            weeks.Add(week);
            
            return weeks;
        }

        private List<ProjectTask> GetSortedTasks(List<Project> projects)
        {
            List<ProjectTask> sortedTasks = new();

            // Long list of tasks sorted by priority
            foreach (Project project in projects)
            {
                foreach (ProjectTask projectTask in project.ProjectTasks)
                {
                    sortedTasks.Add(projectTask);
                }
            }

            return sortedTasks;
        }

        private List<Day> GetSortedDays(List<Day> days)
        {
            return days.OrderBy(d => d.Priority).ToList();
        }

        private int GetTotalHoursLeftToBook(Week week)
        {
            var totalHours = 0;
            foreach (Day day in week.Days)
            {
                totalHours += day.HoursLeftToBook;
            }

            return totalHours;
        }

        private Week AddNewWeek()
        {
            var days = new List<Day>
            {
                new Day { DayName = "Monday", Priority = 1, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 1), Tasks = new List<ProjectTask>()},
                new Day { DayName = "Tuesday", Priority = 2, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 2), Tasks = new List<ProjectTask>() },
                new Day { DayName = "Wednesday", Priority = 3, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 3), Tasks = new List<ProjectTask>() },
                new Day { DayName = "Thursday", Priority = 4, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 4), Tasks = new List<ProjectTask>() },
                new Day { DayName = "Friday", Priority = 5, AvailableHours = 8, HoursLeftToBook = 8, Date = new DateTime(2021, 12, 5), Tasks = new List<ProjectTask>() },
                /*new Day { DayName = "Saturday", Priority = 6, AvailableHours = 0, HoursLeftToBook = 0, Date = new DateTime(2021, 12, 6) },
                new Day { DayName = "Sunday", Priority = 7, AvailableHours = 0, HoursLeftToBook = 0, Date = new DateTime(2021, 12, 7) },*/
            };
            
            Week week = new Week
            {
                Days = days
            };

            return week;
        }

        public List<Project> MockData()
        {
            ProjectTask t = new ProjectTask
            {
                Priority = 1,
                Duration = 10,
                Name = "Opg 1",
                Description = "Opgave 1",
                ProjectName = "Projekt 1",
                PersonAssigned = "Donald",
            };
            ProjectTask t1 = new ProjectTask
            {
                Priority = 2,
                Duration = 4,
                Name = "Opg 2",
                Description = "Opgave 2",
                ProjectName = "Projekt 1",
                PersonAssigned = "Donald"
            };
            ProjectTask t2 = new ProjectTask
            {
                Priority = 3,
                Duration = 1,
                Name = "Opg 3",
                Description = "Opgave 3",
                ProjectName = "Projekt 1",
                PersonAssigned = "Donald"
            };
            ProjectTask t3 = new ProjectTask
            {
                Priority = 4,
                Duration = 1,
                Name = "Opg 4",
                Description = "Opgave 4",
                ProjectName = "Projekt 1",
                PersonAssigned = "Donald"
            };

            ProjectTask t4 = new ProjectTask
            {
                Priority = 1,
                Duration = 2,
                Name = "Opg 5",
                Description = "Opgave 5",
                ProjectName = "Projekt 2",
                PersonAssigned = "Donald"
            };
            ProjectTask t5 = new ProjectTask
            {
                Priority = 2,
                Duration = 3,
                Name = "Opg 6",
                Description = "Opgave 6",
                ProjectName = "Projekt 2",
                PersonAssigned = "Donald"
            };
            ProjectTask t6 = new ProjectTask
            {
                Priority = 3,
                Duration = 3,
                Name = "Opg 7",
                Description = "Opgave 7",
                ProjectName = "Projekt 2",
                PersonAssigned = "Donald"
            };
            ProjectTask t7 = new ProjectTask
            {
                Priority = 4,
                Duration = 10,
                Name = "Opg 8",
                Description = "Opgave 8",
                ProjectName = "Projekt 2",
                PersonAssigned = "Donald"
            };

            ProjectTask t8 = new ProjectTask
            {
                Priority = 1,
                Duration = 6,
                Name = "Opg 9",
                Description = "Opgave 9",
                ProjectName = "Projekt 3",
                PersonAssigned = "Donald"
            };
            ProjectTask t9 = new ProjectTask
            {
                Priority = 2,
                Duration = 2,
                Name = "Opg 10",
                Description = "Opgave 10",
                ProjectName = "Projekt 3",
                PersonAssigned = "Donald"
            };
            ProjectTask t10 = new ProjectTask
            {
                Priority = 3,
                Duration = 3,
                Name = "Opg 11",
                Description = "Opgave 11",
                ProjectName = "Projekt 3",
                PersonAssigned = "Donald"
            };
            ProjectTask t11 = new ProjectTask
            {
                Priority = 4,
                Duration = 5,
                Name = "Opg 12",
                Description = "Opgave 12",
                ProjectName = "Projekt 3",
                PersonAssigned = "Donald"
            };

            List<ProjectTask> pt1 = new List<ProjectTask>();
            pt1.Add(t);
            pt1.Add(t1);
            pt1.Add(t2);
            pt1.Add(t3);

            List<ProjectTask> pt2 = new List<ProjectTask>();
            pt2.Add(t4);
            pt2.Add(t5);
            pt2.Add(t6);
            pt2.Add(t7);

            List<ProjectTask> pt3 = new List<ProjectTask>();
            pt3.Add(t8);
            pt3.Add(t9);
            pt3.Add(t10);
            pt3.Add(t11);


            Project project1 = new Project
            {
                Name = "Mockproject 1",
                Priority = 1,
                ProjectTasks = pt1
            };

            Project project2 = new Project
            {
                Name = "Mockproject 2",
                Priority = 2,
                ProjectTasks = pt2
            };

            Project project3 = new Project
            {
                Name = "Mockproject 3",
                Priority = 3,
                ProjectTasks = pt3
            };

            List<Project> projectList = new List<Project>();
            projectList.Add(project1);
            projectList.Add(project2);
            projectList.Add(project3);

            return projectList;
        }
    }
}