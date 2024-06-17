using Domain.Entities;
using Tests.Overrides.Services;

namespace Tests
{
    public class RentServiceTest
    {
        [Theory]
        [InlineData(30, 20, 50, 210, 0, 210, 0, 0)] // plan 7
        [InlineData(30, 20, 50, 210, -1, 180, 6, 0)] // plan 7 earlier 1
        [InlineData(30, 20, 50, 210, -6, 30, 36, 0)] // plan 7 earlier 6
        [InlineData(30, 20, 50, 210, 1, 210, 0, 50)] // plan 7 later 1
        [InlineData(30, 20, 50, 210, 6, 210, 0, 300)] // plan 7 later 6
        public void TestRentService_UpdateRentValue(decimal planPrice, decimal planFee, decimal planAdd, decimal value, int dateDiffDays, decimal expectedValue, decimal expectedFee, decimal expectedAdd)
        {
            var rent = new Rent()
            {
                EndDate = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(dateDiffDays)),
                PreviewEndDate = DateOnly.FromDateTime(DateTime.UtcNow),
                Value = value,
                RentPlan = new RentPlan()
                {
                    Price = planPrice,
                    Fee = planFee,
                    AdditionalDailyPrice = planAdd
                }
            };

            RentServiceOverride.UpdateRentValuePublic(ref rent);

            Assert.Equal(expectedValue, rent.Value);
            Assert.Equal(expectedFee, rent.Fee);
            Assert.Equal(expectedAdd, rent.AdditionalValue);
        }
    }
}