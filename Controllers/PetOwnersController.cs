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

        // This is just a stub for GET / to prevent any weird frontend errors that 
        // occur when the route is missing in this controller
        [HttpGet]
        public IEnumerable<PetOwner> GetPets()
        {
            return new List<PetOwner>();
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

    } // end of public class
} // end of namespace
