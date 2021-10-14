using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace pet_hotel
{
    public class PetOwner
    {
        // Primary Key: dotnet KNOWS this will be a primary key because it is called 'id'
        public int id { get; set; }
        // Owner Name
        [Required] // each pet should have an owner name
        public string name { get; set; }
        // Email
        [Required]
        public string emailAddress { get; set; }
        // Pets ( joined like the bakedby )
        [JsonIgnore]
        public List<Pets> pets { get; set; }
        // Special Function: Grabs number? of all pets that this owner has
        [NotMapped]
        public int petCount
        {
            get
            {
                return (this.pets != null ? this.pets.Count : 0);
            }
        }
    }
}