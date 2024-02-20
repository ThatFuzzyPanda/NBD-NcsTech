using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NBDProjectNcstech.CustomControllers;
using NBDProjectNcstech.Data;
using NBDProjectNcstech.Models;
using NBDProjectNcstech.Utilities;
using NBDProjectNcstech.ViewModels;

namespace NBDProjectNcstech.Controllers
{
    public class DesignBidsController : CognizantController
    {
        private readonly NBDContext _context;

        public DesignBidsController(NBDContext context)
        {
            _context = context;
        }

        // GET: DesignBids
        public async Task<IActionResult> Index(int? page, int? pageSizeID)
        {
            var designBids = _context.DesignBids
                .Include(d => d.Project)
                .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                .AsNoTracking();

            //Handle Paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<DesignBid>.CreateAsync(designBids.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        // GET: DesignBids/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.DesignBids == null)
            {
                return NotFound();
            }

            var designBid = await _context.DesignBids
                .Include(d => d.Project)
                .Include(d => d.LabourRequirments)
                .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (designBid == null)
            {
                return NotFound();
            }

            return View(designBid);
        }

        // GET: DesignBids/Create
        public IActionResult Create()
        {
            var designBid = new DesignBid();
            PopulateAssignedStaffData(designBid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectSite");
            return View();
        }

        // POST: DesignBids/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ID,ProjectID")] DesignBid designBid, string[] selectedOptions)
        {
            try
            {
                if (selectedOptions != null)
                {
                    foreach (var staff in selectedOptions)
                    {
                        var staffToAdd = new DesignBidStaff { DesignBidID = designBid.ID, StaffID = int.Parse(staff) };
                        designBid.DesignBidStaffs.Add(staffToAdd);

                    }
                }
                if (ModelState.IsValid)
                {
                    _context.Add(designBid);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (RetryLimitExceededException)
            {
                ModelState.AddModelError("", "Unable to save changes after multiple attempts.");
            }
            catch (DbUpdateException dex)
            {
                ModelState.AddModelError("", "Unable to save changes.");
            }
            PopulateAssignedStaffData(designBid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectSite", designBid.ProjectID);
            return View(designBid);
        }

        // GET: DesignBids/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.DesignBids == null)
            {
                return NotFound();
            }

            var designBid = await _context.DesignBids
                                  .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                                  .FirstOrDefaultAsync(d => d.ID == id);
            if (designBid == null)
            {
                return NotFound();
            }
            PopulateAssignedStaffData(designBid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectSite", designBid.ProjectID);
            return View(designBid);
        }

        // POST: DesignBids/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedOptions)
        {
            var designBidToUpdate = await _context.DesignBids
                                    .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                                    .FirstOrDefaultAsync(d => d.ID == id);
            if (designBidToUpdate == null)
            {
                return NotFound();
            }

            UpdateDesignBidStaffs(selectedOptions,designBidToUpdate);

            if (await TryUpdateModelAsync<DesignBid>(designBidToUpdate, "",
                d => d.ProjectID))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (RetryLimitExceededException)
                {
                    ModelState.AddModelError("", "Unable to save changes");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesignBidExists(designBidToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (DbUpdateException dex)
                {
                    ModelState.AddModelError("", "Unable to save changes");
                }
            }

            PopulateAssignedStaffData(designBidToUpdate);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectSite", designBidToUpdate.ProjectID);
            return View(designBidToUpdate);
        }

        // GET: DesignBids/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DesignBids == null)
            {
                return NotFound();
            }

            var designBid = await _context.DesignBids
                .Include(d => d.Project)
                .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (designBid == null)
            {
                return NotFound();
            }

            return View(designBid);
        }

        // POST: DesignBids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DesignBids == null)
            {
                return Problem("Entity set 'NBDContext.DesignBids'  is null.");
            }
            var designBid = await _context.DesignBids
                                  .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                                  .FirstOrDefaultAsync(d => d.ID == id);
            if (designBid != null)
            {
                _context.DesignBids.Remove(designBid);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private void PopulateAssignedStaffData(DesignBid designBid)
        {
            var allOptions = _context.Staffs;
            var currentOptionIDs = new HashSet<int>(designBid.DesignBidStaffs.Select(d => d.StaffID));
            var checkBoxes = new List<CheckOptionVM>();
            foreach (var option in allOptions)
            {
                checkBoxes.Add(new CheckOptionVM
                {
                    ID = option.ID,
                    DisplayText = option.FullName,
                    Assigned = currentOptionIDs.Contains(option.ID)
                });
            }
            ViewData["StaffOptions"] = checkBoxes;
        }

        private void UpdateDesignBidStaffs(string[] selectedOptions, DesignBid designBidToUpdate)
        {
            if (selectedOptions == null)
            {
                designBidToUpdate.DesignBidStaffs = new List<DesignBidStaff>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var designOptionsHS = new HashSet<int>
                (designBidToUpdate.DesignBidStaffs.Select(d => d.StaffID));
            foreach (var option in _context.Staffs)
            {
                if (selectedOptionsHS.Contains(option.ID.ToString())) //It is checked
                {
                    if (!designOptionsHS.Contains(option.ID))
                    {
                        designBidToUpdate.DesignBidStaffs.Add(new DesignBidStaff { DesignBidID = designBidToUpdate.ID, StaffID = option.ID });
                    }
                }
                else
                {
                    //Checkbox not Checked
                    if (designOptionsHS.Contains(option.ID))
                    {
                        DesignBidStaff staffToRemove = designBidToUpdate.DesignBidStaffs.SingleOrDefault(d => d.StaffID == option.ID);
                        _context.Remove(staffToRemove);
                    }
                }
            }
        }

        private bool DesignBidExists(int id)
        {
            return _context.DesignBids.Any(e => e.ID == id);
        }
    }
}
