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
                m.CreationDate = p.CreationDate;
                m.LastModified = p.LastModified;
                m.IsActive = p.IsActive;
                m.IsDeleted = p.IsDeleted;
            }
            return m;
        }

        private void UpdateDbModelProject(Project p, ProjectModel m)
        {
            if (p.Id != m.Id) throw new ArgumentException("обновление не того проекта");
            if (p.User != _authorizedUser) throw new ArgumentException("другой пользователь");
            p.Caption = m.Caption;
            p.Description = m.Description;
            p.LastModified = DateTime.Now;
        }



        [Route("Project")]
        [Route("Project/{id?}")]
        [HttpGet]
        public IActionResult Index(int? id)
        {
            //ViewData["ProjectId"] = id;

            if (_authorizedUser == null) return Unauthorized();

            var projectModels = new List<ProjectModel>();
            var projects = _db.Projects
                .Where(p => p.User == _authorizedUser && !p.IsDeleted && p.IsActive && (id == null || p.Id == id))
                .OrderByDescending(p => p.LastModified)
                .ToArray();
            foreach (var p in projects)
            {
                ProjectModel m = GetProjectModelFromDbModel(p);
                m.NumberOfTasks = _db.TaskInfos
                    .Where(t => t.ProjectID == p.Id && !t.IsDeleted && !t.IsCompleted)
                    .Count();
                projectModels.Add(m);
            }

            var taskModels = new List<TaskModel>();
            var taskinfos = _db.TaskInfos
                .Where(t => t.User == _authorizedUser && t.ProjectID == null && !t.IsDeleted && !t.IsCompleted && (id == null || t.ProjectID == id))
                .OrderByDescending(t => t.LastModified).ToArray();
            foreach (var ti in taskinfos)
            {
                taskModels.Add(TaskController.GetTaskModelFromDbModel(ti));
            }
            return View((id == null) ? "Index" : "Details", new UserAllViewModel() { Projects = projectModels, Tasks = taskModels });
        }


        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
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
        public async Task<IActionResult> Edit(int id, [Bind("ParentId,IsActive,OrderId,SubLevelNo,Id,Caption,Description,CreationDate,LastModified,IsDeleted")] ProjectModel model)
        {
            if (id != model.Id) return BadRequest();
            if (_authorizedUser == null) return Unauthorized();

            if (ModelState.IsValid)
            {
                Project project;
                if (model.Id != 0)
                {


                    project = await _db.Projects.FirstOrDefaultAsync(p => p.Id == model.Id && p.User == _authorizedUser);
                    if (project == null) return BadRequest();
                    UpdateDbModelProject(project, model);
                }
                else
                {
                    project = new() { User = _authorizedUser };
                    UpdateDbModelProject(project, model);
                    _db.Projects.Add(project);
                }
                await _db.SaveChangesAsync();                 //catch (DbUpdateConcurrencyException)
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
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
            if (_authorizedUser == null) return Unauthorized();

            var project = await _db.Projects.FirstOrDefaultAsync((p => p.Id == id && p.User == _authorizedUser));
            if (project == null) return BadRequest();

            project.IsDeleted = true;
            await _db.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}
