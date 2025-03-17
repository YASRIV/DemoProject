using System.Threading.Tasks;
using Car_Wash_Library.Models;
using CustomersAPI.Interface;
using Microsoft.EntityFrameworkCore;

namespace CustomersAPI.Repository
{
    public class CarRepository : ICar
    {
        private readonly CustomerDbContext _context;
        public CarRepository(CustomerDbContext context)
        {
            _context = context;
        }
        public async Task<bool> AddCar(Car car)
        {  
            //var cust =await _repo.GetCustomerById(car.CustomerId);
            var status = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == car.CarId);
            if (status == null)
            {
                _context.Cars.Add(car);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<Car> GetCarById(int carId)
        {
            var item = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == carId);
            if (item != null)
            {
                if (item.IsActive)
                {
                    return item;
                }
                return item;
            }
            return null;
        }
        public async Task<IEnumerable<Car>> GetCars()
        {
            var item = await _context.Cars.Where(c => c.IsActive == true).ToListAsync();
            //if(item==null){
            //    // throw NoDataAvailableException();
            // }
            return item;
        }

        public async Task<bool> RemoveCar(int carId)
        {
            var item = await _context.Cars.FirstOrDefaultAsync(c => c.CarId == carId);
            if (item != null)
            {
                item.IsActive = false;
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateCar(Car car)
        {
            var status = await _context.Cars
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.CarId == car.CarId);
            if (status != null)
            {
                _context.Cars.Update(car);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

    }
}
