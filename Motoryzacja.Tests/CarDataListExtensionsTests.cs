using System;
using System.Collections.Generic;
using FluentAssertions;
using Motoryzacja.Data;
using Motoryzacja.Extensions;
using Xunit;

namespace Motoryzacja.Tests
{
    public class CarDataListExtensionsTests
    {
        private List<CarData> carDataList = new List<CarData>()
        {
            new CarData()
            {
                Displacement = 2000,
                Mileage = 230_000,
                Price = 25_000,
                ProductionYear = 2012
            },
            new CarData()
            {
                Displacement = 1000,
                Mileage = 1000,
                Price = 75_000,
                ProductionYear = 2021
            },
        };
        
        [Fact]
        public void CalculateAvgDisplacement_ShouldCalculateAverageDisplacement_WhenGivenCorrectInput()
        {
            int displacementDigits = 5;
            double expectedDisplacement = 
                Math.Round((double)(carDataList[0].Displacement + carDataList[1].Displacement) / 1000 / 2, displacementDigits);

            carDataList.CalculateAvgDisplacement().Should().Be(expectedDisplacement);
        }
        
        [Fact]
        public void CalculateAvgMileage_ShouldCalculateAverageMileage_WhenInputIsValid()
        {
            int displacementDigits = 1;
            double expectedMileage = 
                Math.Round((double)(carDataList[0].Mileage + carDataList[1].Mileage) / 2, displacementDigits);

            carDataList.CalculateAvgMileage().Should().Be(expectedMileage);
        }
        
        [Fact]
        public void CalculateAvgPrice_ShouldCalculateAveragePrice_WhenInputIsValid()
        {
            int displacementDigits = 1;
            double expectedPrice = 
                Math.Round((double)(carDataList[0].Price + carDataList[1].Price) / 2, displacementDigits);

            carDataList.CalculateAvgPrice().Should().Be(expectedPrice);
        }
        
        [Fact]
        public void CalculateAvgPrice_ShouldCalculateAverageYear_WhenInputIsValid()
        {
            int displacementDigits = 1;
            double expectedYear = 
                Math.Round((double)(carDataList[0].ProductionYear + carDataList[1].ProductionYear) / 2, displacementDigits);

            carDataList.CalculateAvgYear().Should().Be(expectedYear);
        }
    }
}