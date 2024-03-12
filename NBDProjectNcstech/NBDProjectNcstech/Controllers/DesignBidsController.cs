using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CateringManagement.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Elfie.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Options;
using NBDProjectNcstech.CustomControllers;
using NBDProjectNcstech.Data;
using NBDProjectNcstech.Models;
using NBDProjectNcstech.Utilities;
using NBDProjectNcstech.ViewModels;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public async Task<IActionResult> Index(string SearchString, int? page, int? pageSizeID)
        {
            var designBids = _context.DesignBids
                .Include(d => d.Project)
                .Include(d=>d.Approval)
                .Include(d => d.LabourRequirments)
                .Include(d => d.MaterialRequirments)
                .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                .AsNoTracking();


            //search and filter
            if (!System.String.IsNullOrEmpty(SearchString))
            {
                designBids = designBids.Where(c => c.Project.ProjectSite.ToUpper().Contains(SearchString.ToUpper()));
            }

            //Handle Paging
            int pageSize = PageSizeHelper.SetPageSize(HttpContext, pageSizeID, ControllerName());
            ViewData["pageSizeID"] = PageSizeHelper.PageSizeList(pageSize);
            var pagedData = await PaginatedList<DesignBid>.CreateAsync(designBids.AsNoTracking(), page ?? 1, pageSize);
            return View(pagedData);
        }

        // GET: DesignBids/Details/5
        public async Task<IActionResult> Details(int? ID)
        {
            if (ID == null || _context.DesignBids == null)
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
                .FirstOrDefaultAsync(m => m.ID == ID);
            if (designBid == null)
            {
                return NotFound();
            }

            return View(designBid);
        }

        // GET: DesignBids/Create
        public IActionResult Create(int? PositionID)
        {
            
            var designBid = new DesignBid();
            PopulateSortingList(PositionID);
            PopulateDropDownLists();
            PopulateAssignedDesignStaffLists(designBid);
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
            PopulateDropDownLists();
            PopulateAssignedDesignStaffLists(designBid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectSite", designBid.ProjectID);
            return View(designBid);
        }
       


        public async Task<IActionResult> AddLabourAndMaterialPage(int id)
        {
            var designBid = await _context.DesignBids
                            .Include(d => d.Project)
                            .Include(d => d.Approval)
                            .Include(d => d.LabourRequirments)
                            .Include(d => d.MaterialRequirments).ThenInclude(d => d.Inventory)
                            .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                            .AsNoTracking()
                            .FirstOrDefaultAsync(m => m.ID == id);

            return View(designBid);
        }

        [HttpPost]
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

        // GET: DesignBids/Edit/5
        public async Task<IActionResult> Edit(int? ID, int? PositionID)
        {
            if (ID == null || _context.DesignBids == null)
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
                .FirstOrDefaultAsync(m => m.ID == ID);
            if (designBid == null)
            {
                return NotFound();
            }
            PopulateDropDownLists();
            PopulateAssignedDesignStaffLists(designBid);
            ViewData["ProjectID"] = new SelectList(_context.Projects, "Id", "ProjectSite", designBid.ProjectID);
            return View(designBid);
        }

        // POST: DesignBids/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, string[] selectedOptions,int? PositionID)
        {
            var designBidToUpdate = await _context.DesignBids
                                    .Include(d => d.DesignBidStaffs).ThenInclude(d => d.Staff)
                                    .FirstOrDefaultAsync(d => d.ID == id);
            if (designBidToUpdate == null)
            {
                return NotFound();
            }
            

            UpdateStaffListboxes(selectedOptions, designBidToUpdate);
            PopulateAssignedDesignStaffLists(designBidToUpdate);

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
            PopulateDropDownLists();
            PopulateAssignedDesignStaffLists(designBidToUpdate);
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
				.Include(d => d.Approval)
				.Include(d => d.DesignBidStaffs)
                .ThenInclude(d => d.Staff)
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
                return Problem("Entity set 'NBDContext.DesignBids' is null.");
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

        // GET: DesignBids/Edit/5
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



        //private void PopulateAssignedStaffData(DesignBid designBid)
        //      {
        //          var allOptions = _context.Staffs;
        //          var currentOptionIDs = new HashSet<int>(designBid.DesignBidStaffs.Select(d => d.StaffID));
        //          var checkBoxes = new List<CheckOptionVM>();
        //          foreach (var option in allOptions)
        //          {
        //              checkBoxes.Add(new CheckOptionVM
        //              {
        //                  ID = option.ID,
        //                  DisplayText = option.FullName,
        //                  Assigned = currentOptionIDs.Contains(option.ID)
        //              });
        //          }
        //          ViewData["StaffOptions"] = checkBoxes;
        //      }

        //      private void UpdateDesignBidStaffs(string[] selectedOptions, DesignBid designBidToUpdate)
        //      {
        //          if (selectedOptions == null)
        //          {
        //              designBidToUpdate.DesignBidStaffs = new List<DesignBidStaff>();
        //              return;
        //          }

        //          var selectedOptionsHS = new HashSet<string>(selectedOptions);
        //          var designOptionsHS = new HashSet<int>
        //              (designBidToUpdate.DesignBidStaffs.Select(d => d.StaffID));
        //          foreach (var option in _context.Staffs)
        //          {
        //              if (selectedOptionsHS.Contains(option.ID.ToString())) //It is checked
        //              {
        //                  if (!designOptionsHS.Contains(option.ID))
        //                  {
        //                      designBidToUpdate.DesignBidStaffs.Add(new DesignBidStaff { DesignBidID = designBidToUpdate.ID, StaffID = option.ID });
        //                  }
        //              }
        //              else
        //              {
        //                  //Checkbox not Checked
        //                  if (designOptionsHS.Contains(option.ID))
        //                  {
        //                      DesignBidStaff staffToRemove = designBidToUpdate.DesignBidStaffs.SingleOrDefault(d => d.StaffID == option.ID);
        //                      _context.Remove(staffToRemove);
        //                  }
        //              }
        //          }
        //      }

       // 
       //
        private SelectList PopulateSortingList(int? PositionID)
        {
            return new SelectList(_context.StaffPositions.Where(s => s.PositionName == "Designer" || s.PositionName == "Laborer" ||
            s.PositionName == "Driver" || s.PositionName == "Sales Associate")
                .OrderBy(m => m.PositionName), "ID", "PositionName", PositionID);
            
        }
        private void PopulateDropDownLists(DesignBidStaff sp = null)
        {
            ViewData["PositionID"] = PopulateSortingList(sp?.Staff.StaffPositionID);
            
        }
      
        private void PopulateAssignedDesignStaffLists(DesignBid designbid)
        {
            
            // For this to work, you must have Included the child collection in the parent object
            var allOptions = _context.Staffs.
                                    Where(s => s.StaffPosition != null &&
                                                (s.StaffPosition.PositionName == "Designer" || s.StaffPosition.PositionName == "Laborer"
                                               || s.StaffPosition.PositionName =="Driver" || s.StaffPosition.PositionName == "Sales Associate"))
                                    .Include(s => s.StaffPosition) // Ensure StaffPosition is loaded
                                    .OrderBy(s => s.StaffPosition.PositionName); // Order by position name

       
            

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
                        STAFF = r.StaffPositionID,
                        DisplayText = $"{r.FullName} {r.StaffPosition.PositionName}"
                    }) ;
                }
                else
                {
                    available.Add(new ListOptionVM
                    {
                        ID = r.ID,
                        STAFF = r.StaffPositionID,
                        DisplayText = $"{r.FullName} {r.StaffPosition.PositionName}"
                    });
                }
            }

            ViewData["selOpts"] = new MultiSelectList(selected, "STAFF" , "DisplayText");
            ViewData["availOpts"] = new MultiSelectList(available, "STAFF", "DisplayText");
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
