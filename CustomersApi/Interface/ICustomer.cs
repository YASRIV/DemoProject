using Car_Wash_Library.Models;

namespace CustomersAPI.Interface
{
    public interface ICustomer
    {
        #region CustomerOperation
        Task<bool> AddCustomer(Customer customer);
        Task<bool> UpdateCustomer(Customer customer);
        Task<Customer> GetCustomerById(int customerId);
        Task<IEnumerable<Customer>> GetCustomers();
        Task<bool> RemoveCustomer(int customerId);
        Task<IEnumerable<Car>> GetCarsByCustomerId(int customerId);

        #endregion
    }
    public interface ICar
    {
        #region CarOperation
        Task<bool> AddCar(Car car);
        Task<bool> UpdateCar(Car car);
        Task<bool> RemoveCar(int carId);
        Task<Car> GetCarById(int carId);
        Task<IEnumerable<Car>> GetCars();
        #endregion
    }
}
