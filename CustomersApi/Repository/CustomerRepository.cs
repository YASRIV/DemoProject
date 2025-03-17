using System.Threading.Tasks;
using Car_Wash_Library.CustomException;
using Car_Wash_Library.Models;
using CustomersAPI.Interface;
using Microsoft.EntityFrameworkCore;

namespace CustomersAPI.Repository
{
    public class CustomerRepository : ICustomer
    {
        private readonly CustomerDbContext _context;
        public CustomerRepository(CustomerDbContext context)
        {
            _context = context;
        }
        //Method Add Customer in CuatomerDB DataBase
        public async Task<bool> AddCustomer(Customer customer)
        {
            var status = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);
            try
            {
                if (status == null)
                {
                    _context.Customers.Add(customer);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception ex)
            {
                throw new InvalidDataInsertException("Please Insert a valid data and try again");
            }
        }

        //Method used to get list of all Customer Available in DataBase
        public async Task<IEnumerable<Customer>> GetCustomers()
        {
            try
            {
                var item = await _context.Customers.Where(c => c.IsActive == true).Select(c => c).ToListAsync();
                if (item == null)
                {
                    throw new NoDataAvailableException("No Customers Data Available");
                }
                return item;
            }
            catch (NoDataAvailableException ex)
            {
                throw;
            }
        }

        //method is used to get perticular customer object by customerId
        public async Task<Customer> GetCustomerById(int customerId)
        {

            var item = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            try
            {
                if (item == null)
                {
                    throw new NoDataAvailableException("No Customers Data Available");
                }
                return item;
            }
            catch (NoDataAvailableException ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
                throw;
            }

        }

        //Get list of all cars for a perticular customer in database using customerId
        public async Task<IEnumerable<Car>> GetCarsByCustomerId(int customerId)
        {
            var item = await _context.Cars.Where(c => c.CustomerId == customerId).ToListAsync();
            if (item.Count() == 0)
            {
                // throw NoDataAvailableException();

            }
            return item;
        }

        //public IEnumerable<Order> MyOrder(int custmerId)
        //{
        //    throw new NotImplementedException();
        //}

        //Remove Customer or Detactivate Customer from database based on customerId
        public async Task<bool> RemoveCustomer(int customerId)
        {
            var item = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customerId);
            if (item != null)
            {
                item.IsActive = false;
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        //modifying already stored customer data in database
        public async Task<bool> UpdateCustomer(Customer customer)
        {
            var status = await _context.Customers.FirstOrDefaultAsync(c => c.CustomerId == customer.CustomerId);
                if (status != null)
                {
                    _context.Customers.Update(customer);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
        }
    }
}
