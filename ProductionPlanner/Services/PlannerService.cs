using System;
using System.Collections.Generic;
using System.Globalization;
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
        private readonly IDataService _dataService;
        public PlannerService(IDataService dataService)
        {
            _dataService = dataService;
        }
        public List<Week> AssignProjects(bool saveToDB)
        {
            DateTime dateTracker = DateTime.Today;

            var sortedProjects = _dataService.GetProjects();
            var sortedTasks = GetSortedTasks(sortedProjects);
            sortedTasks.ForEach(p =>
            {
                p.Assigned = false;
                
                // DurationWhenSorted is used instead of Duration
                // Prevents duration of a project task to be changed, when it's split into subtasks
                p.DurationWhenSorted = p.Duration;
            });
            
            List<Week> weeks = new();

            Week week = AddNewWeek(dateTracker);
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
                            
                            // Change the week 
                            dateTracker = dateTracker.AddDays(3);
                            week = AddNewWeek(dateTracker);
                        }

                        foreach (Day day in GetSortedDays(week.Days))
                        {
                            if (day.HoursLeftToBook == 0)
                                continue;
                            
                            // Assign task to day
                            if (day.HoursLeftToBook >= subtask.DurationWhenSorted)
                            {
                                subtask.Assigned = true;
                                day.Tasks.Add(subtask);
                                day.HoursLeftToBook -= subtask.DurationWhenSorted;
                                
                                subtask = null;
                                
                                break;
                            }
                            else
                            {
                                var totalTaskHours = subtask.DurationWhenSorted;
                                var hoursBookedToday = day.HoursLeftToBook;
                                subtask.Subtask = true;
                                subtask.DurationWhenSorted = day.HoursLeftToBook;
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
                                    Duration = 0,
                                    DurationWhenSorted = subtask.DurationWhenSorted,
                                    Date = subtask.Date,
                                    Assigned = subtask.Assigned,
                                    PersonAssigned = subtask.PersonAssigned,
                                    Subtask = subtask.Subtask
                                };
                                subtask = s;
                                subtask.Assigned = false;
                                subtask.DurationWhenSorted = totalTaskHours - hoursBookedToday;
                                break;
                            }
                        }
                    }
                }
            }
            
            // If a week was partly filled, it wasn't added in the above foreach
            weeks.Add(week);

            if (saveToDB)
                _dataService.SaveWeeks(weeks);
            
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

        private Week AddNewWeek(DateTime fromDate)
        {
            List<Day> days = new()
            {
                // Monday
                new Day { DayName = "Monday", Priority = 1, AvailableHours = 8, HoursLeftToBook = 8, Date = fromDate, Tasks = new List<ProjectTask>()},
                // Tuesday
                new Day { DayName = "Tuesday", Priority = 1, AvailableHours = 8, HoursLeftToBook = 8, Date = fromDate.AddDays(1), Tasks = new List<ProjectTask>()},
                // Wednesday
                new Day { DayName = "Wednesday", Priority = 1, AvailableHours = 8, HoursLeftToBook = 8, Date = fromDate.AddDays(2), Tasks = new List<ProjectTask>()},
                // Thursday
                new Day { DayName = "Thursday", Priority = 1, AvailableHours = 8, HoursLeftToBook = 8, Date = fromDate.AddDays(3), Tasks = new List<ProjectTask>()},
                // Friday
                new Day { DayName = "Friday", Priority = 1, AvailableHours = 8, HoursLeftToBook = 8, Date = fromDate.AddDays(4), Tasks = new List<ProjectTask>()}
            };
            
            // Get current week NO
            CultureInfo myCI = new CultureInfo("en-US");
            Calendar myCal = myCI.Calendar;
            CalendarWeekRule myCWR = myCI.DateTimeFormat.CalendarWeekRule;
            DayOfWeek myFirstDOW = myCI.DateTimeFormat.FirstDayOfWeek;
            
            Week week = new()
            {
                Days = days,
                WeekNo = myCal.GetWeekOfYear( fromDate, myCWR, myFirstDOW ) // TODO: fiks ugenummer
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