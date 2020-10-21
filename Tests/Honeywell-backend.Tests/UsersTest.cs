using Honeywell_backend.Controllers;
using Honeywell_backend.ViewModel;
using System;
using System.Xml;
using Xunit;

namespace Honeywell_backend.Tests
{
    public class UsersTest
    {
        [Fact]
        public void UsersTest_UserLogin_ShouldLoginUser()
        {
            var register = new RegisterViewModel()
            {
                Address = "Test address",
                Email = "adaojosen@hotmail.com",
                Name = "Adao",
                Password = "Teste123@1234",
                Phone = "0738815 4473"
            };

            var validator = new RegisterViewModelValidation().Validate(register);

            Assert.True(validator.IsValid);
        }

        [Fact]
        public void UsersTest_UserLogin_ShouldNotLoginUser()
        {
            var register = new RegisterViewModel()
            {
                Address = "",
                Email = "",
                Name = "",
                Password = "teste123@1234",
                Phone = ""
            };

            var validator = new RegisterViewModelValidation().Validate(register);

            Assert.Equal(10, validator.Errors.Count);
        }


        /*[Theory]
        [InlineData(1, 3, 4)]
        [InlineData(2, 2, 4)]
        [InlineData(5, 3, 8)]        
        public void Calculator(int x, int y, int z)
        {
            var result = Soma(x, y);
            Assert.Equal(z, result);
        }

        public int Soma(int x, int y)
        {
            return x + y;
        }*/
    }
}
