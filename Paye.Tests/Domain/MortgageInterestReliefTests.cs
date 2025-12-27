using System;
using Paye.Domain.Entities;
using Xunit;

namespace Paye.Tests.Domain
{
    public class MortgageInterestReliefTests
    {
        [Fact]
        public void Only_FCSP_Can_Set_MortgageInterest()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new MortgageInterestRelief(10000, "", new DateTime(2026, 12, 31), "amort.pdf", 2026));
        }

        [Fact]
        public void Interest_End_Date_Cannot_Exceed_Tax_Year()
        {
            Assert.Throws<InvalidOperationException>(() =>
                new MortgageInterestRelief(10000, "FCSP", new DateTime(2027, 1, 1), "amort.pdf", 2026));
        }
    }
}
