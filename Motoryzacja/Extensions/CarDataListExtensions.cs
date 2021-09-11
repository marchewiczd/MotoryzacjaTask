namespace Motoryzacja.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    
    using Data;
    
    public static class CarDataListExtensions
    {
        public static double CalculateAvgDisplacement(this List<CarData> carDataList)
        {
            return Math.Round(carDataList.Sum(x => (double)x.Displacement / 1000) / carDataList.Count, 5);
        }
        
        public static double CalculateAvgMileage(this List<CarData> carDataList)
        {
            return Math.Round(carDataList.Sum(x => (double)x.Mileage / carDataList.Count), 1);
        }
        
        public static double CalculateAvgPrice(this List<CarData> carDataList)
        {
            return Math.Round(carDataList.Sum(x => (double)x.Price / carDataList.Count), 1);
        }
        
        public static double CalculateAvgYear(this List<CarData> carDataList)
        {
            return Math.Round(carDataList.Sum(x => (double)x.ProductionYear / carDataList.Count), 1);
        }
    }
}