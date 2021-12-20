using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AquaG.TasksMVC.ViewModels;
using AquaG.TasksDbModel;


//<select asp-for="Country" asp-items="Model.Countries"></select> 
//https://docs.microsoft.com/en-us/aspnet/core/mvc/views/working-with-forms?view=aspnetcore-5.0

/*
     <select asp-for="EnumCountry" 
            asp-items="Html.GetEnumSelectList<CountryEnum>()">
    </select>  

     public enum CountryEnum
    {
        [Display(Name = "United Mexican States")]
        Mexico,
        [Display(Name = "United States of America")]
        USA,
        Canada,
        France,
        Germany,
        Spain
    }
 * 
 * */

namespace AquaG.TasksMVC.Controllers
{
    public class TaskController : BaseController
    {
        public TaskController() : base() { }

        public static TaskModel GetTaskModelFromDbModel(TaskInfo task)
        {
            TaskModel model = new();

            if (task != null)
            {
                model.Id = task.Id;
                model.ProjectId = task.ProjectId;

                model.Caption = task.Caption;
                model.Description = task.Description;

                model.StartDate = task.StartDate;
                model.EndDate = task.EndDate;

                model.IsCompleted = task.IsCompleted;
            }
            return model;
        }

        private void UpdateDbModelTask(TaskInfo task, TaskModel model)
        {
            if (task.Id != model.Id) throw new ArgumentException("обновление не той задачи");
            if (task.UserId != _authorizedUser.Id) throw new ArgumentException("другой пользователь");

            task.ProjectId = model.ProjectId;
            task.Caption = model.Caption;
            task.Description = model.Description;
            task.StartDate = model.StartDate;
            task.EndDate = model.EndDate;
            task.IsCompleted = model.IsCompleted;

            task.LastModidied = DateTime.Now;
        }

        //[Route("Task/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int? projectId)
        {
            if (!await IsUserAuthorizedAsync()) return Unauthorized();

            List<ProjectDropListModel> projectsDropList = await _db.Projects
                .Where(p => p.User == _authorizedUser)
                .AsNoTracking()
                .OrderByDescending(p => p.LastModidied)
                .Select(p=> new ProjectDropListModel(p.Id,p.Caption))
                .ToListAsync();
            projectsDropList.Insert(0, new ProjectDropListModel(null, "--Нет проекта--"));

            ViewBag.ProjectsDropList = new SelectList(projectsDropList, "Id", "Caption");

            if (id == 0)
                return View(new TaskModel() { ProjectId = projectId });

            return await GetOneRecordFromDbAsActionResult<TaskInfo, TaskModel>(
                    _db.TaskInfos,
                    (t => t.Id == id && t.User == _authorizedUser && (projectId == null || t.ProjectId == projectId)),
                    GetTaskModelFromDbModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Caption,Description,ProjectId,StartDate,EndDate,IsCompleted")] TaskModel model)
        {
            if (!await IsUserAuthorizedAsync()) return Unauthorized();
            if (id != model.Id) return BadRequest();

            if (ModelState.IsValid)
            {
                TaskInfo task;
                if (model.Id != 0)
                {
                    task = await _db
                        .TaskInfos
                        .Include(t => t.User)
                        .FirstOrDefaultAsync(t => t.Id == model.Id && t.User == _authorizedUser);
                    if (task == null) return BadRequest();
                    UpdateDbModelTask(task, model);
                }
                else
                {
                    task = new() { UserId = _authorizedUser.Id };
                    UpdateDbModelTask(task, model);
                    _db.TaskInfos.Add(task);
                }
                await _db.SaveChangesAsync();                 //catch (DbUpdateConcurrencyException)
                return RedirectToAction("Index", "Project", new { id = model.ProjectId });
            }

            return View(model);
        }




        // GET: Task/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
            if (!await IsUserAuthorizedAsync()) return Unauthorized();

            return await GetOneRecordFromDbAsActionResult<TaskInfo, TaskModel>(
                _db.TaskInfos,
                (t => t.Id == id && t.User == _authorizedUser),
                GetTaskModelFromDbModel);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (!await IsUserAuthorizedAsync()) return Unauthorized();

            var task = await _db.TaskInfos
                .Include(t => t.User)
                .FirstOrDefaultAsync(t => t.Id == id && t.User == _authorizedUser);
            if (task == null) return BadRequest();

            int? projectId = task.ProjectId;

            _db.TaskInfos.Remove(task);
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Project", new { id = projectId });
        }



        [HttpGet]
        public async Task<IActionResult> Complete(int id, int? projectId)
        {
            if (!await IsUserAuthorizedAsync()) return Unauthorized();

            var task = await _db.TaskInfos.FirstOrDefaultAsync((t => t.Id == id && t.User == _authorizedUser));
            if (task == null) return BadRequest();

            if (task.ProjectId != projectId) return BadRequest();
            task.IsCompleted = true;
            task.EndDate = DateTime.Now;
            task.LastModidied = DateTime.Now;
            await _db.SaveChangesAsync();

            return RedirectToAction("Index", "Project", new { id = projectId });
        }




    }
}
