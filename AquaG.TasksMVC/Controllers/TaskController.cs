using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using AquaG.TasksMVC.ViewModels;

namespace AquaG.TasksMVC.Controllers
{
    public class TaskController : BaseController
    {
        public TaskController() : base()
        {
        
        }

        // GET: Task
        public async Task<IActionResult> Index()
        {
            return View(await DI_Db.TaskModel.ToListAsync());
        }

        // GET: Task/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await DI_Db.TaskModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskModel == null)
            {
                return NotFound();
            }

            return View(taskModel);
        }

        // GET: Task/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Task/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Caption,Description,CreationDate,LastModified,IsDeleted,StartDate,EndDate,IsNotifyNeeded,Priority,IsCompleted,IsOneAction,OrderId")] TaskModel taskModel)
        {
            if (ModelState.IsValid)
            {
                DI_Db.Add(taskModel);
                await DI_Db.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(taskModel);
        }

        // GET: Task/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await DI_Db.TaskModel.FindAsync(id);
            if (taskModel == null)
            {
                return NotFound();
            }
            return View(taskModel);
        }

        // POST: Task/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Caption,Description,CreationDate,LastModified,IsDeleted,StartDate,EndDate,IsNotifyNeeded,Priority,IsCompleted,IsOneAction,OrderId")] TaskModel taskModel)
        {
            if (id != taskModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    DI_Db.Update(taskModel);
                    await DI_Db.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TaskModelExists(taskModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(taskModel);
        }

        // GET: Task/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskModel = await DI_Db.TaskModel
                .FirstOrDefaultAsync(m => m.Id == id);
            if (taskModel == null)
            {
                return NotFound();
            }

            return View(taskModel);
        }

        // POST: Task/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var taskModel = await DI_Db.TaskModel.FindAsync(id);
            DI_Db.TaskModel.Remove(taskModel);
            await DI_Db.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TaskModelExists(int id)
        {
            return DI_Db.TaskModel.Any(e => e.Id == id);
        }
    }
}
