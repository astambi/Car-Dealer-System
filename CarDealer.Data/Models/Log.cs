namespace CarDealer.Data.Models
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Log
    {
        public int Id { get; set; }

        [Required]
        public string User { get; set; }

        [Required]
        public string Operation { get; set; }

        [Required]
        public string ModifiedTable { get; set; }

        public DateTime Time { get; set; } = DateTime.UtcNow;
    }
}
