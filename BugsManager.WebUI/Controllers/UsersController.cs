using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BugsManager.Domain.Entities;
using BugsManager.Persistence.Contexts;
using MediatR;
using BugsManager.Application.Features.Users.Queries.GetAllUsers;
using BugsManager.Application.Features.Users.Queries.GetUserById;
using BugsManager.Application.DTOs;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using BugsManager.Application.Features.Users.Commands.CreateUser;
using BugsManager.Application.Features.Users.Commands.UpdateUser;
using BugsManager.Application.Features.Users.Commands.DeleteUser;

namespace BugsManager.WebUI.Controllers
{
    public class UsersController : Controller
    {
        private readonly IMediator _mediator;

        public UsersController(IMediator mediator)
        {
            _mediator = mediator;
        }

        // GET: Users
        public async Task<IActionResult> UserIndex()
        {
            var users =  await _mediator.Send(new GetAllUserQuery());
            return View("Index", users.Data);
        }

        // GET: Users/Details/5
        public async Task<IActionResult> Details(int id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var user = await _mediator.Send(new GetUserByIdQuery(id));
            if (user.Data == null)
            {
                return NotFound();
            }

            return View("Details",user.Data);
        }

        // GET: Users/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Users/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name,Surname")] UserDTO user)
        {
            if (ModelState.IsValid)
            {
                await _mediator.Send(new CreateUserCommand { Name=user.Name, Surname=user.Surname});
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }

        // GET: Users/Edit/5
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var user = await _mediator.Send(new GetUserByIdQuery(id));
            if (user.Data == null)
            {
                return NotFound();
            }
            return View("Edit", user.Data);
        }

        // POST: Users/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Surname,Id")] UserDTO user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _mediator.Send(new UpdateUserCommand { Id = id, user = user });
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (! await UserExists(user.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(UserIndex));
            }
            return View("ÜserIndex",user);
        }

        // GET: Users/Delete/5
        public async Task<IActionResult> DeleteById(int id)
        {
            if (id == null )
            {
                return NotFound();
            }

            var user = await _mediator.Send(new GetUserByIdQuery(id));

            if (user == null)
            {
                return NotFound();
            }

            return View("Delete",user.Data);
        }

        // POST: Users/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));

            if (user.Data != null)
            {
                await _mediator.Send(new DeleteUserCommand(id));
            }

           
            return RedirectToAction(nameof(UserIndex));
        }

        private async Task<bool> UserExists(int id)
        {
            var user = await _mediator.Send(new GetUserByIdQuery(id));
            return user.Data!= null;
        }
    }
}
