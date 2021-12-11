using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AquaG.TasksMVC.ViewModels;
using AquaG.TasksDbModel;

namespace AquaG.TasksMVC.Controllers
{
    public class TaskController : BaseController
    {
        public TaskController() : base()
        {
        }


        public static TaskModel GetTaskModelFromDbModel(TaskInfo task)
        {
            TaskModel model = new();

            if (task != null)
            {
                model.Id = task.Id;
                model.ProjectID = task.ProjectID;
                model.Project = task.Project;

                model.Caption = task.Caption;
                model.Description = task.Description;

                model.CreationDate = task.CreationDate;
                model.LastModified = task.LastModified;

                model.IsOneAction = task.IsOneAction;
                model.IsCompleted = task.IsCompleted;
                model.IsDeleted = task.IsDeleted;
                model.IsNotifyNeeded = task.IsNotifyNeeded;
                model.OrderId = task.OrderId;

                model.StartDate = task.StartDate;
                model.EndDate = task.EndDate;
            }
            return model;
        }

        private void UpdateDbModelTask(TaskInfo task, TaskModel model)
        {
            if (task.Id != model.Id) throw new ArgumentException("обновление не того проекта");
            if (task.User != _authorizedUser) throw new ArgumentException("другой пользователь");
            task.Caption = model.Caption;
            task.Description = model.Description;
            task.LastModified = DateTime.Now;
            task.ProjectID = model.ProjectID;
            task.Project = model.Project;
            task.StartDate = model.StartDate;
            task.EndDate = model.EndDate;
            task.IsDeleted = model.IsDeleted;
            task.IsOneAction = model.IsOneAction;
            task.Priority = model.Priority;
            task.IsNotifyNeeded = model.IsNotifyNeeded;
            task.OrderId = model.OrderId;
        }

        [Route("Task/{id}")]
        [HttpGet]
        public async Task<IActionResult> Edit(int id, int? projectID)
        {
            if (id == 0)
                return View(new TaskModel() { ProjectID = projectID });

            return await GetOneRecordFromDbAsActionResult<TaskInfo, TaskModel>(
                    _db.TaskInfos,
                    (t => t.Id == id && t.User == _authorizedUser && (projectID == null || t.ProjectID == projectID)),
                    GetTaskModelFromDbModel);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Caption,Description,CreationDate,LastModified,IsDeleted,StartDate,EndDate,IsNotifyNeeded,Priority,IsCompleted,IsOneAction,OrderId")] TaskModel model)
        {
            if (id != model.Id) return BadRequest();
            if (_authorizedUser == null) return Unauthorized();

            if (ModelState.IsValid)
            {
                TaskInfo task;
                if (model.Id != 0)
                {
                    task = await _db.TaskInfos.FirstOrDefaultAsync(t => t.Id == model.Id && t.User == _authorizedUser);
                    if (task == null) return BadRequest();
                    UpdateDbModelTask(task, model);
                }
                else
                {
                    task = new() { User = _authorizedUser };
                    UpdateDbModelTask(task, model);
                    _db.TaskInfos.Add(task);
                }
                await _db.SaveChangesAsync();                 //catch (DbUpdateConcurrencyException)
                return RedirectToAction(nameof(Index));
            }

            return View(model);
        }




        // GET: Task/Delete/5
        public async Task<IActionResult> Delete(int id)
        {
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
            if (_authorizedUser == null) return Unauthorized();

            var task = await _db.TaskInfos.FirstOrDefaultAsync((t => t.Id == id && t.User == _authorizedUser));
            if (task == null) return BadRequest();

            int? projectID = task.ProjectID;

            task.IsDeleted = true;
            await _db.SaveChangesAsync();

            return RedirectToAction("/Project" + (projectID == null ? "" : $"/{projectID}"));
        }


    }
}
