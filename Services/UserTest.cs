//using NUnit.Framework;

namespace Demo.Tests
{
    public class UserTest
    {
        //[Test]
        public void Test1()   // ❌ Violates: unclear name, does not follow method_state_expectedresult
        {
            // Arrange
            var service = new UserService();

            // Act
            var result = service.IsValidEmail("test@example.com");

            // Assert
          //  Assert.IsTrue(result);
        }
    }

    public class UserService
    {
        public bool IsValidEmail(string email) => email.Contains("@");
    }
}
