﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.Pkcs;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ZealEducationManager.Entities;
using ZealEducationManager.Models.CandidatesViewModels;

namespace ZealEducationManager.Controllers
{
    [Authorize]
    public class CandidatesController : Controller
    {
        private readonly ZealEducationManagerContext _context;

        public CandidatesController(ZealEducationManagerContext context)
        {
            _context = context;
        }

        // GET: Candidates
        public async Task<IActionResult> Index()
        {
            return View(await _context.Candidates.Include(c => c.Batch).ToListAsync());
        }

        // GET: Candidates/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            var candidate = await _context.Candidates.Include(c => c.Batch)
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidate == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            return View(candidate);
        }

        // GET: Candidates/Create
        public IActionResult Create()
        {
			var viewModel = new AddCandidateView
			{
				BatchCode = _context.Batches.Select(c => new SelectListItem
				{
					Value = c.BatchId.ToString(),
					Text = c.BatchCode
				})
			};
			return View(viewModel);
        }

        // POST: Candidates/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(AddCandidateView viewodel )
        {
            if (ModelState.IsValid)
            {
                var candidate = new Candidate
                {
                    FirstName = viewodel.FirstName,
                    LastName = viewodel.LastName,
                    DateOfBirth = viewodel.DateOfBirth,
                    DateOfJoining = viewodel.DateOfJoining,
                    ContactInfo = viewodel.ContactInfo,
                    OutstandingFee = viewodel.OutstandingFee,
                    Status = viewodel.Status,
                    BatchId = viewodel.BatchId,
                };
                _context.Add(candidate);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(viewodel);
        }

        // GET: Candidates/Edit/5
        [Route("candidates/edit")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }
            var candidate = await _context.Candidates.FindAsync(id);
            if (candidate == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            var viewModel = new EditCandidateView
            {
                CandidateId = candidate.CandidateId,
                FirstName = candidate.FirstName,
                LastName = candidate.LastName,
                DateOfBirth = candidate.DateOfBirth,
                DateOfJoining = candidate.DateOfJoining,
                ContactInfo = candidate.ContactInfo,
                OutstandingFee = candidate.OutstandingFee,
                Status = candidate.Status,
                BatchId = candidate.BatchId,
                BatchCode = _context.Batches.Select(c => new SelectListItem
                {
                    Value = c.BatchId.ToString(),
                    Text = c.BatchCode
                })
            };
            return View(viewModel);
        }

        // POST: Candidates/Edit/5
        [Route("candidates/edit")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("CandidateId,FirstName,LastName,DateOfBirth,DateOfJoining,ContactInfo,OutstandingFee,Status,BatchId")] EditCandidateView viewmodel)
        {
            if (id != viewmodel.CandidateId)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var candidate = await _context.Candidates.FindAsync(viewmodel.CandidateId);
                    candidate.FirstName = viewmodel.FirstName;
                    candidate.LastName = viewmodel.LastName;
                    candidate.DateOfBirth = viewmodel.DateOfBirth;
                    candidate.DateOfJoining = viewmodel.DateOfJoining;
                    candidate.ContactInfo = viewmodel.ContactInfo;
                    candidate.OutstandingFee = viewmodel.OutstandingFee;
                    candidate.Status = viewmodel.Status;
                    candidate.BatchId = viewmodel.BatchId;
                    _context.Update(candidate);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CandidateExists(viewmodel.CandidateId))
                    {
                        TempData["message"] = "Cannot find any data";
                        return RedirectToAction("Message", "Dashboard");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            } else
            {
                var viewModel = new EditCandidateView
                {
                    FirstName = viewmodel.FirstName,
                    LastName = viewmodel.LastName,
                    DateOfBirth = viewmodel.DateOfBirth,
                    DateOfJoining = viewmodel.DateOfJoining,
                    ContactInfo = viewmodel.ContactInfo,
                    OutstandingFee = viewmodel.OutstandingFee,
                    Status = viewmodel.Status,
                    BatchId = viewmodel.BatchId,
                    BatchCode = _context.Batches.Select(b => new SelectListItem
                    {
                        Value = b.BatchId.ToString(),
                        Text = b.BatchCode
                    })
                };
            }

            return View(viewmodel);
        }

        // GET: Candidates/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");
            }

            var candidate = await _context.Candidates
                .FirstOrDefaultAsync(m => m.CandidateId == id);
            if (candidate == null)
            {
                TempData["message"] = "Cannot find any data";
                return RedirectToAction("Message", "Dashboard");s
            }

            return View(candidate);
        }

        // POST: Candidates/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var candidate = await _context.Candidates.FindAsync(id);
                if (candidate != null)
                {
                    _context.Candidates.Remove(candidate);
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                TempData["message"] = "Cannot Delete this Record";
                return RedirectToAction("Message", "Dashboard");
            }
        }

        private bool CandidateExists(int id)
        {
            return _context.Candidates.Any(e => e.CandidateId == id);
        }
    }
}
