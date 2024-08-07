﻿using ZealEducationManager.Entities;

namespace ZealEducationManager.Models.PaymentViewModels
{
    public class AddPaymentModel
    {
        public int PaymentId { get; set; }

        public int CandidateId { get; set; }

        public decimal Amount { get; set; }

        public DateOnly PaymentDate { get; set; }

        public virtual Candidate? Candidate { get; set; } = null!;
    }
}