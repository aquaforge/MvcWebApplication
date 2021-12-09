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

namespace AquaG.TasksMVC.Controllers
{

    [Authorize] //[Authorize(Roles = "admin")]
    public class ProjectController : BaseController
    {
        private User _authorizedUser;
        private User GetAuthorizedUser()
        {
            if (_authorizedUser == null) _authorizedUser = DI_UserManager.FindByEmailAsync(User.Identity.Name).GetAwaiter().GetResult();
            return _authorizedUser;
        }
        public ProjectController() : base() { }

        private ProjectModel GetProjectModelFromDbModel(Project p)
        {
            ProjectModel m = new ();

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

        private void UpdateDbModel(Project p, ProjectModel m)
        {
            if (p.Id != m.Id) throw new ArgumentException("обновление не того проекта");
            p.Caption = m.Caption;
            p.Description = m.Description;
            p.LastModified = DateTime.Now;
            p.IsActive = m.IsActive ?? true;
            p.IsDeleted = m.IsDeleted ?? false;
        }



        // GET: Project
        public async Task<IActionResult> Index()
        {
            User authUser = GetAuthorizedUser();

            var projectModels = new List<ProjectModel>();

            var projects = DI_Db.Projects.Where(p => p.User == authUser).OrderByDescending(p => p.LastModified).ToArray();
            foreach (var p in projects)
            {
                ProjectModel m = GetProjectModelFromDbModel(p);
                m.NumberOfTasks = DI_Db.TaskInfos.Where(t => t.ProjectID == p.Id && !t.IsDeleted && !t.IsCompleted).Count();
                projectModels.Add(m);
            }

            return View(projectModels.ToArray());
        }

        [Route("Project/{id}")]
        public async Task<IActionResult> Details(int id)
        {
            User authUser = GetAuthorizedUser();

            var project = await DI_Db.Projects
                .Include(p => p.Parent)
                .Include(p => p.TaskInfo)
                .FirstOrDefaultAsync(p => p.Id == id && p.User == authUser);

            if (project == null) return NotFound();

            return View(project);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            return await Edit(null);
        }

        // GET: Projects/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return View(new ProjectModel());
            }
            else
            {
                User authUser = GetAuthorizedUser();

                Project project = await DI_Db.Projects
                    .FirstOrDefaultAsync(p => p.Id == id && p.User == authUser);
                if (project == null) return NotFound();

                return View(GetProjectModelFromDbModel(project));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParentId,IsActive,OrderId,SubLevelNo,Id,Caption,Description,CreationDate,LastModified,IsDeleted")] ProjectModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                Project project;
                if (model.Id != 0)
                {
                    User authUser = GetAuthorizedUser();

                    project = await DI_Db.Projects.FirstOrDefaultAsync(p => p.Id == model.Id && p.User == authUser);
                    if (project == null) return NotFound();
                }
                else
                {
                    project = new();
                }
                UpdateDbModel(project, model);
                await DI_Db.SaveChangesAsync();                 //catch (DbUpdateConcurrencyException)
                return RedirectToAction(nameof(Index));
            }
            //ViewData["ParentId"] = new SelectList(DI_Db.Projects, "Id", "Caption", m.ParentId);
            return View(model);
        }

        //// GET: Projects/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var project = await DI_Db.Projects
        //        .Include(p => p.Parent)
        //        .FirstOrDefaultAsync(m => m.Id == id);
        //    if (project == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(project);
        //}

        //// POST: Projects/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public async Task<IActionResult> DeleteConfirmed(int id)
        //{
        //    var project = await DI_Db.Projects.FindAsync(id);
        //    DI_Db.Projects.Remove(project);
        //    await DI_Db.SaveChangesAsync();
        //    return RedirectToAction(nameof(Index));
        //}
    }
}
