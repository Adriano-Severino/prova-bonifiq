using AutoFixture;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ProvaPub.Models;
using ProvaPub.Repository;
using ProvaPub.Services;
using ProvaPub.Services.Interface;

namespace ProvaPub.Tests
{
    [TestClass]
    public class CustomerServiceTests
    {
        private TestDbContext _dbContext;
        private ICustomerService _customerService;
        private Fixture _fixture;

        [TestInitialize]
        public void Initialize()
        {
            _fixture = new Fixture();
            _dbContext = new TestDbContext(new DbContextOptionsBuilder<TestDbContext>().UseInMemoryDatabase(databaseName: "TestDatabase").Options);
            _customerService = new CustomerService(_dbContext);
        }

        [TestMethod]
        public void ListCustomers_ReturnsCorrectPage()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Arrange
            var expectedPage = 2;
            var expectedPageSize = 10;
            var customers = _fixture.CreateMany<Customer>(expectedPageSize * 3).ToList();
            _dbContext.Customers.AddRange(customers);
            _dbContext.SaveChanges();

            // Act
            var result = _customerService.ListCustomers(expectedPage);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(expectedPage, result.Pagination.Page);
            Assert.AreEqual(expectedPageSize, result.Customers.Count);
        }

        [TestMethod]
        public void CanPurchase_IfCustomerIdEqualZero()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Arrange
            var expectedPageSize = 10;
            var customerId = 0;
            var purchaseValue = 10.0M;
            var customers = _fixture.CreateMany<Customer>(expectedPageSize * 3).ToList();
            _dbContext.Customers.AddRange(customers);
            _dbContext.SaveChanges();

            // Act
            var result = _customerService.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.IsNotNull(result);
            Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => _customerService.CanPurchase(customerId, purchaseValue));

        }

        [TestMethod]
        public void CanPurchase_IfPurchaseValueEqualZero()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Arrange
            var expectedPageSize = 10;
            var customerId = 1;
            var purchaseValue = 0;
            var customers = _fixture.CreateMany<Customer>(expectedPageSize * 3).ToList();
            _dbContext.Customers.AddRange(customers);
            _dbContext.SaveChanges();

            // Act
            var result = _customerService.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.IsNotNull(result);
            Assert.ThrowsExceptionAsync<ArgumentOutOfRangeException>(() => _customerService.CanPurchase(customerId, purchaseValue));

        }

        [TestMethod]
        public void CanPurchase_NonRegisteredCustomersCannotPurchase()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Arrange
            var expectedPageSize = 10;
            var customerId = 1;
            var purchaseValue = 12;
            var customers = _fixture.CreateMany<Customer>(expectedPageSize * 3).ToList();
            customers.Clear();
            _dbContext.Customers.AddRange(customers);
            _dbContext.SaveChanges();

            var custumer = _dbContext.Customers.FirstOrDefault(x => x.Id == customerId);

            // Act
            var result = _customerService.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.IsNotNull(result);
            Assert.ThrowsExceptionAsync<InvalidOperationException>(() => _customerService.CanPurchase(customerId, purchaseValue));
        }

        [TestMethod]
        public void CanPurchase_CustomerCanPurchaseOnlySingleTimePerMonth()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Arrange
            var customerId = 1;
            var purchaseValue = 12;
            var customers = _fixture.Create<Customer>();
            customers.Id = 1;
            customers.Orders.Clear();
            customers.Orders.Add(new Order() { Id = 1, CustomerId = 1, OrderDate = DateTime.Now, Value = 10 });

            _dbContext.Customers.AddRange(customers);
            _dbContext.SaveChanges();

            // Act
            var result = _customerService.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);
        }

        [TestMethod]
        public void CanPurchase_CustomerThatNeverBoughtBeforeCanMakeFirstPurchaseMaximum100()
        {
            _fixture.Behaviors.OfType<ThrowingRecursionBehavior>().ToList().ForEach(b => _fixture.Behaviors.Remove(b));
            _fixture.Behaviors.Add(new OmitOnRecursionBehavior());

            // Arrange
            var customerId = 1;
            var purchaseValue = 200;
            var customers = _fixture.Create<Customer>();
            customers.Id = 1;
            customers.Orders.Clear();

            _dbContext.Customers.AddRange(customers);
            _dbContext.SaveChanges();

            // Act
            var result = _customerService.CanPurchase(customerId, purchaseValue);

            // Assert
            Assert.IsNotNull(result);
            Assert.IsFalse(result.Result);

        }
    }
}
