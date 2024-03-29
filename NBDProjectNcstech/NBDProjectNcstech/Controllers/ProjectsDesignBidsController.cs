﻿using CateringManagement.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using NBDProjectNcstech.CustomControllers;
using NBDProjectNcstech.Data;
using NBDProjectNcstech.Models;
using NBDProjectNcstech.Utilities;

namespace NBDProjectNcstech.Controllers
{
	public class ProjectsDesignBidsController : ElephantController
    {
        private readonly NBDContext _context;

        public ProjectsDesignBidsController(NBDContext context)
        {
            _context = context;
        }

        // GET: ProjectsDesignBids
        public async Task<IActionResult> Index(int? ProjectID, int? page, int? pageSizeID)
        {
            var designBids = from d in _context.DesignBids
                .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                .Include(d => d.LabourRequirments)
                .Include(d => d.MaterialRequirments)
                .Include(d => d.Approval)
                .Include(d => d.Project)
                where d.ProjectID == ProjectID.GetValueOrDefault()
                select d;
            // Get the URL with the last filter, sort and page parameters from THE PATIENTS Index View
            ViewData["returnURL"] = MaintainURL.ReturnURL(HttpContext, "ProjectsDesignBids");

            if (!ProjectID.HasValue)
            {
                //Go back to the proper return URL for the Patients controller
                return Redirect(ViewData["returnURL"].ToString());
            }

            //Now get the MASTER record, the client, so it can be displayed at the top of the screen
            Project project = await _context.Projects
				.Include(p => p.DesignBids)
                .Include(p => p.Client)
				.Where(p => p.Id == ProjectID.GetValueOrDefault())
				.AsNoTracking()
				.FirstOrDefaultAsync();

            ViewBag.projects = project;

            //Handle Paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<DesignBid>.CreateAsync(designBids.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

		// GET: ProjectsDesignBids/Details/5
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
		public async Task<IActionResult> Details(int? ID)
        {
            if (ID == null || _context.DesignBids == null)
            {
                return NotFound();
            }

            var designBid = await _context.DesignBids
                .Include(d => d.Approval)
                .Include(d => d.Project)
                .Include(d => d.LabourRequirments).ThenInclude(d => d.Labour)
                .Include(d => d.MaterialRequirments).ThenInclude(d => d.Inventory)
                .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                .FirstOrDefaultAsync(m => m.ID == ID);
            if (designBid == null)
            {
                return NotFound();
            }

            return View(designBid);
        }

		// GET: ProjectsDesignBids/Create
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
		public IActionResult Create(int? projectID, int? PositionID)
        {
			ViewData["id"] = projectID;
			var designBid = new DesignBid();
            PopulateSortingList(PositionID);
            PopulateDropDownLists();
            PopulateAssignedDesignStaffLists(designBid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectSite");
            return View();
        }

        // POST: ProjectsDesignBids/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
		public async Task<IActionResult> Create([Bind("ID,ProjectID,ApprovalID")] DesignBid designBid, string[] selectedOptions)
        {
            try
            {
                designBid.Approval = new Approval();

                if (selectedOptions != null)
                {
                    foreach (var staff in selectedOptions)
                    {
                        var staffToAdd = new DesignBidStaff { DesignBidID = designBid.ID, StaffID = int.Parse(staff) };
                        designBid.DesignBidStaffs.Add(staffToAdd);

                    }
                }

                //Create new Approval with both its status as pending 
                designBid.Approval.AdminApprovalStatus = ApprovalStatus.Pending.ToString();
                designBid.Approval.ClientApprovalStatus = ApprovalStatus.Pending.ToString();

                if (ModelState.IsValid)
            {
                _context.Add(designBid);
                await _context.SaveChangesAsync();
                return RedirectToAction("AddLabourAndMaterialPage", "DesignBids", new { id = designBid.ID });
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
            PopulateAssignedDesignStaffLists(designBid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectSite", designBid.ProjectID);
            return View(designBid);
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> Approve(int? id)
        {
            if (id == null || _context.Clients == null)
            {
                return NotFound();
            }
            var designBid = await _context.DesignBids
                            .Include(d => d.Project)
                            .Include(d => d.Approval)
                            .Include(d => d.LabourRequirments)
                            .Include(d => d.MaterialRequirments)
                            .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(m => m.ID == id);
            if (designBid == null)
            {
                return Problem("There are no Design Bids to delete.");

            }

            if (ModelState.IsValid)
            {
                try
                {

                    designBid.Approval.AdminApprovalStatus = ApprovalStatus.Approved.ToString();
                    _context.Update(designBid);
                    _context.ApproveEntity();
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesignBidExists(designBid.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(designBid);

        }
        [HttpPost]
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> Reject(int id)
        {
            var designBid = await _context.DesignBids
                                        .Include(d => d.Project)
                                        .Include(d => d.Approval)
                                        .Include(d => d.LabourRequirments)
                                        .Include(d => d.MaterialRequirments)
                                        .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(m => m.ID == id);
            if (designBid == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    designBid.Approval.AdminApprovalStatus = ApprovalStatus.Denied.ToString();
                    _context.Update(designBid);
                    _context.RejectEntity();
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesignBidExists(designBid.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

            }
            return View(designBid);
        }

		// GET: ProjectsDesignBids/Edit/5
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
		public async Task<IActionResult> Edit(int? id, int? PositionID)
        {
            if (id == null || _context.DesignBids == null)
            {
                return NotFound();
            }

            var designBid = await _context.DesignBids
                .Include(d => d.Project)
                .Include(d => d.Approval)
                .Include(d => d.LabourRequirments).ThenInclude(d => d.Labour)
                .Include(d => d.MaterialRequirments).ThenInclude(d => d.Inventory)
                .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);
            if (designBid == null)
            {
                return NotFound();
            }
            PopulateDropDownLists();
            PopulateAssignedDesignStaffLists(designBid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectSite", designBid.ProjectID);
            return View(designBid);
        }

        // POST: ProjectsDesignBids/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
		[Authorize(Roles = "Admin,Management,Designer,Sales")]
		public async Task<IActionResult> Edit(int id, [Bind("ID,ProjectID,ApprovalID")] DesignBid designBid, int? PositionID)
        {
            if (id != designBid.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(designBid);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesignBidExists(designBid.ID))
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
            ViewData["ApprovalID"] = new SelectList(_context.Approvals, "ID", "ID", designBid.ApprovalID);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectSite", designBid.ProjectID);
            return View(designBid);
        }

        // GET: ProjectsDesignBids/Delete/5
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.DesignBids == null)
            {
                return NotFound();
            }

            var designBid = await _context.DesignBids
                .Include(d => d.Approval)
                .Include(d => d.Project)
                .FirstOrDefaultAsync(m => m.ID == id);
            if (designBid == null)
            {
                return NotFound();
            }

            return View(designBid);
        }

        // POST: ProjectsDesignBids/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.DesignBids == null)
            {
                return Problem("Entity set 'NBDContext.DesignBids'  is null.");
            }
            var designBid = await _context.DesignBids.FindAsync(id);
            if (designBid != null)
            {
                _context.DesignBids.Remove(designBid);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> ApprovalEdit(int? id)
        {
            if (id == null || _context.Approvals == null)
            {
                return NotFound();
            }

            var approvalToUpdate = await _context.Approvals
                                  .FirstOrDefaultAsync(d => d.ID == id);
            if (approvalToUpdate == null)
            {
                return NotFound();
            }
            return View(approvalToUpdate);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Management")]
        public async Task<IActionResult> AdminApproval(int id, string adminApprovalStatus, string adminApprovalNotes)
        {
            var approvalToUpdate = await _context.Approvals
                                .FirstOrDefaultAsync(d => d.ID == id);

            if (approvalToUpdate == null)
            {
                return NotFound();
            }

            if (await TryUpdateModelAsync<Approval>(approvalToUpdate, "",
                a => a.AdminApprovalStatus, a => a.AdminApprovalNotes, a => a.AdminApprovalDate))
            {
                try
                {
                    _context.Update(approvalToUpdate);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DesignBidExists(approvalToUpdate.ID))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return View(approvalToUpdate);
        }
        private SelectList PopulateSortingList(int? PositionID)
        {
            return new SelectList(_context.StaffPositions.Where(s => s.PositionName == "Designer" || s.PositionName == "Sales Associate")
                .OrderBy(m => m.PositionName), "ID", "PositionName", PositionID);

        }
        private SelectList PopulateApprovelList(string ApprovalStatus)
        {
            List<string> Options = new List<string>();
            Options.Add("Approved");
            Options.Add("Pending");
            Options.Add("Denied");
            return new SelectList(Options);
        }
        private void PopulateDropDownLists(DesignBidStaff sp = null, DesignBid db = null)
        {
            ViewData["PositionID"] = PopulateSortingList(sp?.Staff.StaffPositionID);
            ViewData["ApprovalStatus"] = PopulateApprovelList(db?.Approval.AdminApprovalStatus);
        }

        private void PopulateAssignedDesignStaffLists(DesignBid designbid)
        {

            // For this to work, you must have Included the child collection in the parent object
            var allOptions = _context.Staffs.
                                    Where(s => s.StaffPosition != null &&
                                                (s.StaffPosition.PositionName == "Designer" || s.StaffPosition.PositionName == "Sales Associate"))
                                    .Include(s => s.StaffPosition) // Ensure StaffPosition is loaded
                                    .OrderBy(s => s.StaffPosition.PositionName); // Order by position name


            //var allOptions = _context.Staffs.Include(s => s.StaffPosition);

            var currentOptionsHS = new HashSet<int>(designbid.DesignBidStaffs.Select(b => b.StaffID));

            // Instead of one list with a boolean, we will make two lists
            var selected = new List<ListOptionVM>();
            var available = new List<ListOptionVM>();

            foreach (var r in allOptions)
            {
                if (currentOptionsHS.Contains(r.ID))
                {
                    selected.Add(new ListOptionVM
                    {
                        ID = r.ID,
                        Staff = r.StaffPosition.PositionName,
                        DisplayText = $"{r.FullName} {r.StaffPosition.PositionName}"
                    });
                }
                else
                {
                    available.Add(new ListOptionVM
                    {
                        ID = r.ID,
                        Staff = r.StaffPosition.PositionName,
                        DisplayText = $"{r.FullName} {r.StaffPosition.PositionName}"
                    });
                }
            }

            ViewData["selOpts"] = new MultiSelectList(selected, "ID", "DisplayText", "", "Staff");
            ViewData["availOpts"] = new MultiSelectList(available, "ID", "DisplayText", "", "Staff");
        }
        private void UpdateStaffListboxes(string[] selectedOptions, DesignBid designBidToUpdate)
        {
            if (selectedOptions == null)
            {
                designBidToUpdate.DesignBidStaffs = new List<DesignBidStaff>();
                return;
            }

            var selectedOptionsHS = new HashSet<string>(selectedOptions);
            var currentOptionsHS = new HashSet<int>(designBidToUpdate.DesignBidStaffs.Select(b => b.StaffID));
            foreach (var r in _context.Staffs)
            {
                if (selectedOptionsHS.Contains(r.ID.ToString()))//it is selected
                {
                    if (!currentOptionsHS.Contains(r.ID))//but not currently in the Function's collection - Add it!
                    {
                        designBidToUpdate.DesignBidStaffs.Add(new DesignBidStaff
                        {
                            StaffID = r.ID,
                            DesignBidID = designBidToUpdate.ID
                        });
                    }
                }
                else //not selected
                {
                    if (currentOptionsHS.Contains(r.ID))//but is currently in the Function's collection - Remove it!
                    {
                        DesignBidStaff staffToRemove = designBidToUpdate.DesignBidStaffs.FirstOrDefault(d => d.StaffID == r.ID);
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
