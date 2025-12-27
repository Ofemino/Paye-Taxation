using System;

namespace Paye.Domain.Entities
{
    public class RentRelief
    {
        public decimal AnnualRent { get; private set; }
        public decimal ComputedAmount { get; private set; }
        public decimal Cap { get; } = 500000M;
        public string FormulaVersion { get; private set; }
        public DateTime ComputedAt { get; private set; }

        public RentRelief(decimal annualRent, string formulaVersion)
        {
            AnnualRent = annualRent;
            ComputedAmount = Math.Min(annualRent * 0.20M, Cap);
            FormulaVersion = formulaVersion;
            ComputedAt = DateTime.UtcNow;
        }
    }
}
