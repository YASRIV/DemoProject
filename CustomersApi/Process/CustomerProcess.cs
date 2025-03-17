using Car_Wash_Library.Models;
using CustomersAPI.Interface;

namespace CustomersAPI.Process
{
    public class CustomerProcess
    {
        private readonly ICar carRepo;
        private readonly ICustomer repo;

        public CustomerProcess(ICar carRepo, ICustomer repo)
        {
            this.carRepo = carRepo;
            this.repo = repo;
        }
        #region Car
        public async Task<bool> AddCar(Car car)
        {
            return await carRepo.AddCar(car);
        }
        public Task<bool> UpdateCar(Car car)
        {
            return carRepo.UpdateCar(car);
        }
        public async Task<bool> RemoveCar(int carId)
        {
            return await carRepo.RemoveCar(carId);
        }
        public async Task<Car> GetCarById(int carId)
        {
            return await carRepo.GetCarById(carId);
        }
        public async Task<IEnumerable<Car>> GetCars()
        {
            return await carRepo.GetCars();
        }
        #endregion

        #region Customers
        public async virtual Task<bool> AddCustomer(Customer customer)
        {
            return await repo.AddCustomer(customer);
        }
        public async Task<bool> ActivateCustomer(int customerId)
        {
            var cust = await repo.GetCustomerById(customerId);
            if (!cust.IsActive)
            {
                cust.IsActive = true;
                return await repo.UpdateCustomer(cust);
            }
            return false;
        }
        public async Task<bool> UpdateCustomer(Customer customer)
        {
            return await repo.UpdateCustomer(customer);
        }
        public async Task<Customer> GetCustomerById(int customerId)
        {
            return await repo.GetCustomerById(customerId);
        }
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            return await repo.GetCustomers();
        }
        public async Task<bool> RemoveCustomer(int customerId)
        {
            return await repo.RemoveCustomer(customerId);
        }
        public async Task<IEnumerable<Car>> GetCarsByCustomerId(int customerId)
        {
            return await repo.GetCarsByCustomerId(customerId);
        }
        #endregion

    }
}
