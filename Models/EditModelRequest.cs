﻿using System.ComponentModel.DataAnnotations;

namespace Mono.Models
{
    public class EditModelRequest
    {
        [Required]
        public int Id { get; set; }
        [Required]
        public int MakeId { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public string Abrv { get; set; }
    }
}
