﻿using System.ComponentModel.DataAnnotations;
using ZealEducationManager.Entities;

namespace ZealEducationManager.Models.BatchViewModels
{
    public class AddBatchViewModel
    {
        public int BatchId { get; set; }
        [Required(ErrorMessage = "Batch code is required")] 
        public string BatchCode { get; set; } = null!;

        [Required(ErrorMessage = "Start Date code is required")]
        public DateOnly StartDate { get; set; }

        [Required(ErrorMessage = "End Date code is required")]
        public DateOnly EndDate { get; set; }

    }
}