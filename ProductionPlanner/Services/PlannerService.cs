using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Google.DataTable.Net.Wrapper;
using Google.DataTable.Net.Wrapper.Extension;
using ProductionPlanner.Interfaces;
using ProductionPlanner.Models;

namespace ProductionPlanner.Services
{
    public class PlannerService : IPlannerService
    {
        Week week = new();

        public List<Project> AssignProjects(List<Project> projects)
        {
            var sortedProjects = projects.OrderBy(t => t.Priority).ToList();
            Dictionary<ProjectTask, Guid> tasksToAdd = new();

            // Get highest priority project that isn't fully assigned
            foreach (Project project in sortedProjects)
            {
                if (project.AllTasksAssigned)
                    continue;

                // Get highest priority task that isn't assigned
                foreach (ProjectTask task in project.Tasks)
                {
                    // If task assigned, skip to next task
                    if (task.Assigned)
                        continue;

                    // Tasks extending over multiple days
                    bool multipleDayTask = false;
                    int multipleDayTaskDuration = task.Duration;

                    // Find the first available day, in which the task fits
                    foreach (Day day in week.Days)
                    {
                        if (day.HoursLeftToBook == 0)
                            continue;

                        // Task has previously been flagged as multiple day task
                        if (multipleDayTask)
                        {
                            // Task was flagged as multiple day, but has already been delegated out fully
                            if (multipleDayTaskDuration == 0)
                                break;

                            // Deep copy list, so task can be split into multiple tasks, over multiple days
                            ProjectTask projectTask = new ProjectTask
                            {
                                Priority = task.Priority,
                                Duration = multipleDayTaskDuration,
                                Date = task.Date,
                                BufferTimer = task.BufferTimer,
                                Name = task.Name,
                                Description = task.Description,
                            };

                            // If project can finally be completed, move on
                            if (day.HoursLeftToBook > projectTask.Duration)
                            {
                                projectTask.StartHour = 8 + day.AvailableHours - day.HoursLeftToBook;
                                projectTask.EndHour = projectTask.StartHour + projectTask.Duration;

                                day.HoursLeftToBook -= projectTask.Duration;
                                projectTask.Date = day.Date;
                                projectTask.Assigned = true;
                                day.Tasks.Add(projectTask);
                                tasksToAdd.Add(projectTask, project.Guid);
                                //project.Tasks.Add(projectTask);
                                break;
                            }

                            // If still too big, run the loop again until task is broken down to tasks that fit
                            if (day.HoursLeftToBook < projectTask.Duration)
                            {
                                projectTask.StartHour = 8 + day.AvailableHours - day.HoursLeftToBook;
                                projectTask.EndHour = projectTask.StartHour + projectTask.Duration;

                                day.HoursLeftToBook -= day.HoursLeftToBook;
                                projectTask.Duration = day.HoursLeftToBook;
                                multipleDayTaskDuration -= task.Duration - day.HoursLeftToBook;

                                projectTask.Date = day.Date;
                                projectTask.Assigned = true;
                                day.Tasks.Add(projectTask);
                                tasksToAdd.Add(projectTask, project.Guid);
                                //project.Tasks.Add(projectTask);
                            }
                        }
                        
                        // Task can be inserted directly into a plan timeslot
                        else if (day.HoursLeftToBook > task.Duration)
                        {
                            task.StartHour = 8 + day.AvailableHours - day.HoursLeftToBook;
                            task.EndHour = task.StartHour + task.Duration;
                            day.HoursLeftToBook -= task.Duration;
                            task.Date = day.Date;
                            task.Assigned = true;
                            day.Tasks.Add(task);

                            // Prevent task from being assigned to multiple days
                            break;
                        }
                        
                        // Task doesn't fit into timeslot, but must be planned over multiple days
                        else if (day.HoursLeftToBook < task.Duration)
                        {
                            multipleDayTask = true;
                            multipleDayTaskDuration = task.Duration - day.HoursLeftToBook;

                            task.StartHour = 8 + day.AvailableHours - day.HoursLeftToBook;
                            task.EndHour = task.StartHour + day.HoursLeftToBook;

                            task.Duration = day.HoursLeftToBook;
                            day.HoursLeftToBook -= day.HoursLeftToBook;
                            task.Date = day.Date;
                            task.Assigned = true;
                            day.Tasks.Add(task);
                        }
                    }
                }
            }

            // Add all the tasks that wasn't added during the foreach loop
            foreach (var key in tasksToAdd.Keys)
            {
                foreach (Project project in sortedProjects)
                {
                    if (tasksToAdd[key] == project.Guid)
                    {
                        project.Tasks.Add(key);
                    }
                }
            }

            return sortedProjects;
        }

        public string ChartJsonGenerator(List<Project> projects)
        {
            List<TimelineRow> timelineFormat = new List<TimelineRow>();

            foreach (Project project in projects)
            {
                foreach (ProjectTask projectTask in project.Tasks)
                {
                    TimelineRow t = new()
                    {
                        BarLabel = project.Name,
                        RowLabel = projectTask.Name,
                        Start = projectTask.Date.AddHours(8 + projectTask.StartHour),
                        End = projectTask.Date.AddHours(8 + projectTask.EndHour),
                    };

                    timelineFormat.Add(t);
                }
            }

            var json = timelineFormat.ToGoogleDataTable()
                .NewColumn(new Column(ColumnType.String, "Row label"), x => x.RowLabel)
                .NewColumn(new Column(ColumnType.String, "Bar label"), x => x.BarLabel)
                .NewColumn(new Column(ColumnType.Datetime, "Start"), x => x.Start)
                .NewColumn(new Column(ColumnType.Datetime, "End"), x => x.End)
                .Build()
                .GetJson();

            return json;
        }

        public List<Project> MockData()
        {
            ProjectTask t = new ProjectTask
            {
                Priority = 1,
                Duration = 5,
                Name = "Opg 1",
                Description = "Opgave 1",
            };
            ProjectTask t1 = new ProjectTask
            {
                Priority = 2,
                Duration = 2,
                Name = "Opg 2",
                Description = "Opgave 2",
            };
            ProjectTask t2 = new ProjectTask
            {
                Priority = 3,
                Duration = 3,
                Name = "Opg 3",
                Description = "Opgave 3",
            };
            ProjectTask t3 = new ProjectTask
            {
                Priority = 4,
                Duration = 10,
                Name = "Opg 4",
                Description = "Opgave 4",
            };
            ProjectTask t4 = new ProjectTask
            {
                Priority = 5,
                Duration = 8,
                Name = "Opg 5",
                Description = "Opgave 5",
            };
            ProjectTask t5 = new ProjectTask
            {
                Priority = 6,
                Duration = 3,
                Name = "Opg 6",
                Description = "Opgave 6",
            };
            ProjectTask t6 = new ProjectTask
            {
                Priority = 7,
                Duration = 15,
                Name = "Opg 7",
                Description = "Opgave 7",
            };
            ProjectTask t7 = new ProjectTask
            {
                Priority = 8,
                Duration = 6,
                Name = "Opg 8",
                Description = "Opgave 8",
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

            Project project1 = new Project
            {
                Priority = 1,
                Tasks = pt1
            };

            Project project2 = new Project
            {
                Priority = 2,
                Tasks = pt2
            };

            List<Project> projectList = new List<Project>();
            projectList.Add(project1);
            projectList.Add(project2);

            return projectList;
        }
    }
}