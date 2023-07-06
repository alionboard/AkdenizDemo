using Microsoft.Extensions.Localization;
using Moq;
using MRCase.API.Controllers;
using MRCase.API.Extensions;
using MRCase.API.Utilities;
using MRCase.Application.Localization;
using MRCase.Core.Localization;
using System;
using System.Collections.Generic;
using Xunit;

namespace MRCase.Tests
{
    public class UnitTest1
    {
        [Fact]
        public void Calculate_Sum_Test()
        {
            //Arrange

            //Act
            var result = Calculate.Sum(1, 2);

            //Assert
            Assert.Equal(3, result);
        }

        [Fact]
        public void Calculate_Sum_Wrong_Result_Test()
        {
            //Arrange


            //Act
            var result = Calculate.Sum(1, 2);

            //Assert
            Assert.NotEqual(4, result);
        }
    }
}
