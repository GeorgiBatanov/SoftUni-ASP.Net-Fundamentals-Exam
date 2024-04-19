﻿using SeminarHub.Data;
using System.ComponentModel.DataAnnotations;

namespace SeminarHub.Models
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;
    }
}
