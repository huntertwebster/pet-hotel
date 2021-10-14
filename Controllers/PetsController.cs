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
        private readonly ILogger<PetsController> _logger;
        public PetsController(ApplicationContext context, ILogger<PetsController> logger)
        {
            _logger = logger;
            _context = context;
        }

        // GET pets by ID
        [HttpGet("{id}")]
        // GET /api/PetInventory/10 -- to get single bread entry with id of 10
        public PetInventory GetPetByID(int id)
        {
            return _context.PetInventory.Find(id);
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

        // POST - create a new pet
        [HttpPost]
        public IActionResult CreatePet([FromBody] PetInventory pet)
        {
            _logger.LogInformation($"Here is the owner id: {pet.petOwnerid}");
            var petOwner = _context.PetOwners.Find(pet.petOwnerid);
            if (petOwner == null)
            {
                ModelState.AddModelError("petOwnerid", "HEY YOU DONT HAVE A PET OWNER ID");
                return ValidationProblem(ModelState);
            }
            _context.PetInventory.Add(pet);
            _context.SaveChanges();

            // fancy way of returning true HTTP 201, including a URL of where to get the owner
            return CreatedAtAction(nameof(GetPetByID), new { id = pet.id }, pet);
            // return Ok(); // TODO: return HTTP 201 Created
        }

        // PUT - CHECK In 
        [HttpPut("{id}/checkin")]
        public object doCheckin(int id)
        {
            PetInventory pet = _context.PetInventory.SingleOrDefault(p => p.id == id);
            if (pet == null)
            {
                System.Console.WriteLine("Pet Not Found");
                return NotFound();
            }
            // how do we checkin the pet?
            pet.increase();
            _context.Update(pet); // mark as updated
            _context.SaveChanges(); // run SQL update command

            return pet;
        }


        // PUT - CHECK OUT
        [HttpPut("{id}/checkout")]
        public object doCheckOut(int id)
        {
            PetInventory pet = _context.PetInventory.SingleOrDefault(p => p.id == id);
            if (pet == null)
            {
                System.Console.WriteLine("Pet Not Found");
                return NotFound();
            }
            // how do we checkin the pet?
            pet.decrease();
            _context.Update(pet); // mark as updated
            _context.SaveChanges(); // run SQL update command

            return pet;
        }

        // PUT a petOwner
        [HttpPut("{id}")]
        public object editOwner(int id, [FromBody] PetInventory potentialPet)
        {
            PetInventory pet = _context.PetInventory.Find(id);
            if (pet == null)
            {
                System.Console.WriteLine("Pet Not Found");
                return NotFound();
            }
            // how do we checkin the pet?
            pet.name = potentialPet.name;
            _context.Update(pet); // mark as updated
            _context.SaveChanges(); // run SQL update command

            return pet;
        }



        // [HttpGet]
        // [Route("test")]
        // public IEnumerable<PetInventory> GetPets()
        // {
        //     PetOwner blaine = new PetOwner
        //     {
        //         name = "Blaine"
        //     };

        //     PetInventory newPet1 = new PetInventory
        //     {
        //         name = "Big Dog",
        //         PetOwner = blaine,
        //         color = PetColorType.Black,
        //         breed = PetBreedType.Poodle,
        //     };

        //     PetInventory newPet2 = new PetInventory
        //     {
        //         name = "Little Dog",
        //         PetOwner = blaine,
        //         color = PetColorType.Golden,
        //         breed = PetBreedType.Labrador,
        //     };

        //     return new List<PetInventory> { newPet1, newPet2 };
        // }
    }
}
