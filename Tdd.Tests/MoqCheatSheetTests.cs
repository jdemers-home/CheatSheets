using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TddTests
{
    public class MoqCheatSheetTests
    {
        [Fact]
        public void Moq_CallToValueReturningMethodWithRightParameter_True()
        {
            var sut = new Mock<IFoo>();
            sut.Setup(x => x.DoSomething("ping")).Returns(true);
            Assert.Equal(true, sut.Object.DoSomething("ping"));
        }
        [Fact]
        public void Moq_CallToValueReturningMethodWithWrongParameter_False()
        {
            var sut = new Mock<IFoo>();
            sut.Setup(x => x.DoSomething("ping")).Returns(true);
            Assert.Equal(false, sut.Object.DoSomething("pong"));
        }
        [Fact]
        public void Moq_CallToValueReturningMethodWithoutSetup_Exception()
        {
            var sut = new Mock<IFoo>();
            bool test = sut.Object.DoSomething("ping");
            Assert.Equal(true, sut.Object.DoSomething("ping"));
        }
    }
}