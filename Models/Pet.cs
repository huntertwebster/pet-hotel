using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;
namespace pet_hotel
{
    public enum PetBreedType
    {
        Shepherd, // 0
        Poodle,   // 1
        Beagle,   // 2
        Bulldog,  // 3
        Terrier,  // 4
        Boxer,    // 5
        Labrador, // 6
        Retriever // 7
    }
    public enum PetColorType
    {
        Black,    // 0
        White,    // 1
        Golden,   // 2
        Tricolor, // 3
        Spotted   // 4
    }
    public class PetInventory
    {
        // (id), name, breed, color, checked In, Pet Owner
        // id
        public int id { get; set; }
        // name of pet
        [Required]
        public string name { get; set; }
        // breed of pet
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PetBreedType breed { get; set; }
        // color of pet
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public PetColorType color { get; set; }
        // date/time checked in at 
        public DateTime? checkedInAt { get; set; } // ask this question!
        // Who owns the pet?
        public PetOwner ownedBy { get; set; }
        // lets us see the pet owner id when we POST a new pet
        [Required]
        [ForeignKey("PetOwners")]
        public int petOwnerid { get; set; }

        public int petCount { get; set; }

        public void increase()
        {
            this.checkedInAt = DateTime.Now;
        }

        public void decrease()
        {
            this.checkedInAt = null;
        }
    }
}