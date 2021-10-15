using System.Collections.Generic;
using System;
using System.Text.Json.Serialization;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace pet_hotel
{
    public class Transactions
    {
        // Primary Key: dotnet KNOWS this will be a primary key because it is called 'id'
        public int id { get; set; }

        // Message
        [Required] // each transation should have a message.
        public string message { get; set; }

        // timestamp
        [Required]
        public DateTime? timestamp { get; set; }

    }
}