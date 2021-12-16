using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AquaG.TasksDbModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using AquaG.TasksMVC.ViewModels;

namespace AquaG.TasksMVC.Controllers
{

    [Authorize] //[Authorize(Roles = "admin")]
    public class ProjectController : BaseController
    {

        public ProjectController() : base() { }


        public static ProjectModel GetProjectModelFromDbModel(Project p)
        {
            ProjectModel m = new();

            if (p != null)
            {
                m.Id = p.Id;
                m.Caption = p.Caption;
                m.Description = p.Description;

                if (p.TaskInfos != null)
                    m.NumberOfTasks = p.TaskInfos.Count;
                else
                    m.NumberOfTasks = 0;
            }
            return m;
        }

        private void UpdateDbModelProject(Project p, ProjectModel m)
        {
            if (p.Id != m.Id) throw new ArgumentException("обновление не того проекта");
            if (p.UserId != _authorizedUser.Id) throw new ArgumentException("другой пользователь");
            p.Caption = m.Caption;
            p.Description = m.Description;
            p.LastModidied = DateTime.Now;
        }



        [Route("Project")]
        [Route("Project/{id?}")]
        [HttpGet]
        public async Task<IActionResult> Index(int? id)
        {
            if (!await IsUserAuthorizedAsync()) return Unauthorized();

            var projects = await _db.Projects
            .Where(p => p.User == _authorizedUser && (p.Id == id || id == null))
            .Include(p => p.TaskInfos.Where(t => !t.IsCompleted))
            .AsNoTracking()
            .OrderByDescending(p => p.LastModidied)
            .ToListAsync();

            if (id != null && (projects == null || projects.Count == 0)) return BadRequest();

            var projectModels = new List<ProjectModel>();
            var taskModels = new List<TaskModel>();
            var taskModelsCompleted = new List<TaskModel>();

            foreach (var p in projects)
                projectModels.Add(GetProjectModelFromDbModel(p));


            var taskinfos = await _db.TaskInfos
                .Where(t => t.User == _authorizedUser && t.ProjectId == id)
                .AsNoTracking()
                .OrderByDescending(t => t.LastModidied)
                .ToListAsync();
            foreach (var t in taskinfos)
            {
                if (t.IsCompleted)
                    taskModelsCompleted.Add(TaskController.GetTaskModelFromDbModel(t));
                else
                    taskModels.Add(TaskController.GetTaskModelFromDbModel(t));
            }

            return View((id == null) ? "Index" : "Details", new UserAllViewModel()
            {
                IsOneProjectDetails = id != null,
                Projects = projectModels,
                Tasks = taskModels,
                TasksCompleted = taskModelsCompleted
            });
        }


        [HttpGet]
        [Route("Project/Edit/{id?}")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (!await IsUserAuthorizedAsync()) return Unauthorized();

            ProjectModel m = new();

            if (id.GetValueOrDefault() == 0)
                return View(new ProjectModel());

            return await GetOneRecordFromDbAsActionResult<Project, ProjectModel>(
                _db.Projects,
                (p => p.Id == id && p.User == _authorizedUser),
                GetProjectModelFromDbModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Caption,Description")] ProjectModel model)
        {
            if (!await IsUserAuthorizedAsync()) return Unauthorized();
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                Project project;
                if (model.Id != 0)
                {


                    project = await _db.Projects
                        .Include(p => p.User)
                        .FirstOrDefaultAsync(p => p.Id == model.Id && p.User == _authorizedUser);
                    if (project == null) return BadRequest();
                    UpdateDbModelProject(project, model);
                }
                else
                {
                    project = new() { UserId = _authorizedUser.Id };
                    UpdateDbModelProject(project, model);
                    _db.Projects.Add(project);
                }
                await _db.SaveChangesAsync();                 //catch (DbUpdateConcurrencyException)
                return RedirectToAction("Index", "Project", new { project.Id });
            }

            return View(model);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (!await IsUserAuthorizedAsync()) return Unauthorized();

            return await GetOneRecordFromDbAsActionResult<Project, ProjectModel>(
                _db.Projects,
                (p => p.Id == id && p.User == _authorizedUser),
                GetProjectModelFromDbModel);
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await IsUserAuthorizedAsync()) return Unauthorized();

            var project = await _db.Projects.FirstOrDefaultAsync((p => p.Id == id && p.User == _authorizedUser));
            if (project == null) return BadRequest();

            if (_db.TaskInfos.Any(t => t.ProjectId == id))
            {
                ModelState.AddModelError("id", "Нельзя удалить непустой проект.");
                return View("Delete", GetProjectModelFromDbModel(project));
            }

            _db.Projects.Remove(project);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Project");
        }
    }
}
