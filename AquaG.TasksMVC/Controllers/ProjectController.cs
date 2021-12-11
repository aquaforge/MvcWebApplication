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

        public void UpdateDbModel(Project p, ProjectModel m)
        {
            if (p.Id != m.Id) throw new ArgumentException("обновление не того проекта");
            if (p.User != GetAuthorizedUser()) throw new ArgumentException("другой пользователь");
            p.Caption = m.Caption;
            p.Description = m.Description;
            p.LastModified = DateTime.Now;
        }



        // GET: Project
        public IActionResult Index(int? id)
        {
            User authUser = GetAuthorizedUser();
            if (authUser == null) return Unauthorized();



            var projectModels = new List<ProjectModel>();
            var projects = DI_Db.Projects
                .Where(p => p.User == authUser && !p.IsDeleted && p.IsActive)
                .OrderByDescending(p => p.LastModified)
                .ToArray();
            foreach (var p in projects)
            {
                ProjectModel m = GetProjectModelFromDbModel(p);
                m.NumberOfTasks = DI_Db.TaskInfos
                    .Where(t => t.ProjectID == p.Id && !t.IsDeleted && !t.IsCompleted)
                    .Count();
                projectModels.Add(m);
            }

            var taskModels = new List<TaskModel>();
            var taskinfos = DI_Db.TaskInfos
                .Where(t => t.User == authUser && t.ProjectID == null && !t.IsDeleted && !t.IsCompleted)
                .OrderByDescending(t => t.LastModified).ToArray();
            foreach (var ti in taskinfos)
            {
                taskModels.Add(TaskController.GetTaskModelFromDbModel(ti));
            }
            return View(new UserAllViewModel() { Projects = projectModels, Tasks = taskModels });
        }




        //public async Task<IActionResult> Details(int id)
        //{
        //    User authUser = GetAuthorizedUser();
        //    if (authUser == null) return Unauthorized();

        //    var project = await DI_Db.Projects
        //        .Include(p => p.Parent)
        //        .Include(p => p.TaskInfo)
        //        .FirstOrDefaultAsync(p => p.Id == id && p.User == authUser);

        //    if (project == null) return BadRequest();

        //    return View(GetProjectModelFromDbModel(project));
        //}

        // GET: Projects/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            ProjectModel m = new();

            if (id != null & id != 0)
            {
                User authUser = GetAuthorizedUser();
                if (authUser == null) return Unauthorized();

                Project project = await DI_Db.Projects
                    .FirstOrDefaultAsync(p => p.Id == id && p.User == authUser);
                if (project == null) return BadRequest();

                m = GetProjectModelFromDbModel(project);
            }
            return View(m);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParentId,IsActive,OrderId,SubLevelNo,Id,Caption,Description,CreationDate,LastModified,IsDeleted")] ProjectModel model)
        {
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                Project project;
                if (model.Id != 0)
                {
                    User authUser = GetAuthorizedUser();
                    if (authUser == null) return Unauthorized();

                    project = await DI_Db.Projects.FirstOrDefaultAsync(p => p.Id == model.Id && p.User == authUser);
                    if (project == null) return BadRequest();
                    UpdateDbModel(project, model);
                }
                else
                {
                    project = new() { User = GetAuthorizedUser() };
                    UpdateDbModel(project, model);
                    DI_Db.Projects.Add(project);
                }
                await DI_Db.SaveChangesAsync();                 //catch (DbUpdateConcurrencyException)
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ParentId"] = new SelectList(DI_Db.Projects, "Id", "Caption", m.ParentId);
            return View(model);
        }

        // GET: Project/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return BadRequest();

            User authUser = GetAuthorizedUser();
            if (authUser == null) return Unauthorized();

            var project = await DI_Db.Projects.FirstOrDefaultAsync((p => p.Id == id && p.User == authUser));
            if (project == null) return BadRequest();

            return View(GetProjectModelFromDbModel(project));
        }

        // POST: Project/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            User authUser = GetAuthorizedUser();
            if (authUser == null) return Unauthorized();

            var project = await DI_Db.Projects.FirstOrDefaultAsync((p => p.Id == id && p.User == authUser));
            if (project == null) return BadRequest();

            DI_Db.Projects.Remove(project);
            await DI_Db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
