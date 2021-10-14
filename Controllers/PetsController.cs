using System.Net.NetworkInformation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using pet_hotel.Models;
using Microsoft.EntityFrameworkCore;

namespace pet_hotel.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PetsController : ControllerBase
    {
        private readonly ApplicationContext _context;
        public PetsController(ApplicationContext context)
        {
            _context = context;
        }

        // GET pets by ID
        [HttpGet("{id")]
        // GET /api/PetInventory/10 -- to get single bread entry with id of 10
        public PetInventory GetPetByID(int id)
        {
            return _context.PetInventory.Find(id);
        }

        // GET all pets - safety net 
        [HttpGet]
        public IEnumerable<PetInventory> GetPets()
        {
            return new List<PetInventory>();
        }

        // GET all pets
        [HttpGet] // respond to the GET at this route --> /
        public List<PetInventory> GetPet()
        {
            // this includes asks donet to run the appropraite JOIN requests to join the pets table 
            return _context.PetInventory.Include(p => p.ownedBy).OrderBy(pet => pet.ownedBy).ToList();
        }

        // DELETE
        [HttpDelete("{id}")] // route --> /api/PetInventory/:id

        public IActionResult deletePet(int id)
        {
            PetInventory p = _context.PetInventory.Find(id);
            if (p == null)
            {
                // if the id is invalid return a 404 not found.
                return NotFound();
            }
            // Actually delete the pet
            _context.PetInventory.Remove(p); // marks it for deletion
            _context.SaveChanges();

            return NoContent();
        }

        // POST
        [HttpPost]
        public IActionResult createPet([FromBody] PetInventory pet)
        {
            _context.PetInventory.Add(pet);
            _context.SaveChanges();

            // fancy way of returning true HTTP 201, including a URL of where to get the owner
            return CreatedAtAction(nameof(GetPetByID), new { id = pet.id }, pet);
            // return Ok(); // TODO: return HTTP 201 Created
        }



        // [HttpGet]
        // [Route("test")]
        // public IEnumerable<Pet> GetPets() {
        //     PetOwner blaine = new PetOwner{
        //         name = "Blaine"
        //     };

        //     Pet newPet1 = new Pet {
        //         name = "Big Dog",
        //         petOwner = blaine,
        //         color = PetColorType.Black,
        //         breed = PetBreedType.Poodle,
        //     };

        //     Pet newPet2 = new Pet {
        //         name = "Little Dog",
        //         petOwner = blaine,
        //         color = PetColorType.Golden,
        //         breed = PetBreedType.Labrador,
        //     };

        //     return new List<Pet>{ newPet1, newPet2};
        // }
    }
}
