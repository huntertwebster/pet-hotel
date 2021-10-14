using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetOwnersController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetOwnersController(ApplicationContext context)
        {
            _context = context;
        }

        // GET all owners
        [HttpGet]
        public List<PetOwner> GetOwners()
        {
            return _context.PetOwners.Include(p => p.pets).ToList();
        }

        //GET owner by ID
        [HttpGet("{id}")]
        public PetOwner GetOwners(int id)
        {
            return _context.PetOwners.Find(id);
        }

        // POST an owner
        [HttpPost]
        public IActionResult createPetOwner([FromBody] PetOwner owner)
        {
            _context.PetOwners.Add(owner);
            _context.SaveChanges();

            // fancy way of returning true HTTP 201, including a URL of where to get the baker
            return CreatedAtAction(nameof(GetOwners), new { id = owner.id }, owner);
            // return Ok(); // TODO: return HTTP 201 Created
        }

        // DELETE a petOwner
        [HttpDelete("{id}")] // DELETE /api/PetOwner/:id
        public IActionResult deletePetOwner(int id)
        {
            // Delete a petOwner by the ID
            PetOwner p = _context.PetOwners.Find(id);
            if (p == null)
            {
                // if the id is invalid, return a 404 not found
                return NotFound();
            }
            // Actually delete the baker
            _context.PetOwners.Remove(p); // marks it for deletion
            _context.SaveChanges(); // runs the SQL delete command

            // return Ok(); // Returns HTTP 200 OK
            return NoContent(); // Returns HTTP 204
        }




        // PUT a petOwner
        [HttpPut("{id}")]
        public object editOwner(int id, [FromBody] PetOwner potentialPetOwner)
        {
            PetOwner owner = _context.PetOwners.Find(id);
            if (owner == null)
            {
                System.Console.WriteLine("PetOwner Not Found");
                return NotFound();
            }
            // how do we checkin the pet?
            owner.name = potentialPetOwner.name;
            _context.Update(owner); // mark as updated
            _context.SaveChanges(); // run SQL update command

            return owner;
        }

    } // end of public class
} // end of namespace
