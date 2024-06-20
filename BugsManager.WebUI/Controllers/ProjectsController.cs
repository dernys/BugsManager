using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugsManager.Domain.Entities;
using BugsManager.Persistence.Contexts;
using BugsManager.Application.Interfaces.Repositories;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using BugsManager.Application.Features.Bugs.Queries.GetAllBugs;
using System.Threading;
using BugsManager.Application.Features.Projects.Queries.GetAllProjects;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using BugsManager.Application.Features.Projects.Queries.GetProjectById;
using BugsManager.Domain.Common.Interfaces;
using MediatR;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using BugsManager.Application.Features.Projects.Commands.CreateProject;
using BugsManager.Application.DTOs;
using BugsManager.Application.Features.Projects.Commands.UpdateProject;
using BugsManager.Application.Features.Projects.Commands.DeleteProject;

namespace BugsManager.WebUI.Controllers
{
    public class ProjectsController : Controller
    {
        private readonly IMediator _mediator;

        public ProjectsController(IMediator mediator)
        {
           _mediator = mediator;
        }

        // GET: Projects
        public async Task<IActionResult> ProjectIndex()
        {
            var Bugs = await _mediator.Send(new GetAllProjectsQuery());
            return  View("Index",Bugs.Data);
        }
        
        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _mediator.Send(new GetProjectByIdQuery(id));

            return View(project.Data);
        }

        // GET: Projects/Create
        public IActionResult CreateProject()
        {
            return View("Create");
        }
 
        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Description")] ProjectDTO project)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new CreateProjectCommand { Name= project.Name, Description = project.Description});
                return RedirectToAction(nameof(Index));
            }
            return View(project);
        }
       
        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var project = await _mediator.Send(new GetProjectByIdQuery(id));
            if (project == null)
            {
                return NotFound();
            }
            return View(project.Data);
        }
        
        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description")] ProjectDTO project)
        {
            if (id != project.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(new UpdateProjectCommand { Id = id, projectDTO = project });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await ProjectExists(project.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(ProjectIndex));
            }
            return View(project);
        }
       
        // GET: Projects/Delete/5
        public async Task<IActionResult> DeleteById(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _mediator.Send(new GetProjectByIdQuery(id));
            if (project.Data == null)
            {
                return NotFound();
            }

            return View("Delete",project.Data);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {

            var project = await _mediator.Send(new GetProjectByIdQuery(id));
            if (project.Data != null)
            {
                await _mediator.Send(new DeleteProjectCommand(id));
            }

           
            return RedirectToAction(nameof(Index));
        }

        private async Task<bool> ProjectExists(int id)
        {
            var project = await _mediator.Send(new GetProjectByIdQuery(id));
            return project.Data != null;
           
        }
    }
}
