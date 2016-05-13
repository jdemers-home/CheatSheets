using System;
using System.Text.RegularExpressions;
using Ploeh.AutoFixture;
using Moq;
using Xunit;

namespace TddTests
{
    public class MoqCheatSheetTests
    {
        [Trait("Method", "Stubbing")]
        [Fact]
        public void SimpleMethodStubbing()
        {
            // Arrange
            var sut = new Mock<IFoo>();
            sut.Setup(foo => foo.DoSomething("ping")).Returns(true);
            // Act
            bool result = sut.Object.DoSomething("ping");
            // Assert
            Assert.Equal(true, result);
        }

        [Trait("Method", "Argument")]
        [Theory]
        [InlineData("miSSile", "missile")]
        [InlineData("DESTroyer", "destroyer")]
        [InlineData("withIN", "within")]
        [InlineData("12", "12")]
        [InlineData("miles", "miles")]
        public void AccessInvocationArgument(string inStr, string outStr)
        {
            var sut = new Mock<IFoo>();
            sut.Setup(foo => foo.DoSomethingStringy(It.IsAny<string>())).Returns((string s) => s.ToLower());

            string result = sut.Object.DoSomethingStringy(inStr);

            Assert.Equal(result, outStr);
        }

        [Trait("Method", "Ref")]
        [Fact]
        public void RefMatchOnSameInstance()
        {
            Bar[] bar = {new Bar()};
            var sut = new Mock<IFoo>();
            sut.Setup(foo => foo.Submit(ref bar[0])).Returns(true);

            bool result = sut.Object.Submit(ref bar[0]);

            Assert.Equal(true, result);
        }

        [Trait("Method", "Ref")]
        [Fact]
        public void RefDoesntMatchOnDifferentInstance()
        {
            Bar[] bar = { new Bar(), new Bar() };
            var sut = new Mock<IFoo>();
            sut.Setup(foo => foo.Submit(ref bar[0])).Returns(true);

            bool result = sut.Object.Submit(ref bar[1]);

            Assert.Equal(false, result);
        }

        [Trait("Method", "Exception")]
        [Fact]
        public void ThrowException()
        {
            var sut = new Mock<IFoo>();
            sut.Setup(foo => foo.GetCount()).Throws(new InvalidCastException());

            Exception exception = Record.Exception(() => sut.Object.GetCount());
            
            Assert.IsType<InvalidCastException>(exception);
        }

        [Trait("Method", "Callback")]
        [Fact]
        public void Callback()
        {
            var sut = new Mock<IFoo>();
            var nbCalls = 0;
            sut.Setup(foo => foo.GetCount())
                .Returns(() => nbCalls)
                .Callback(() => nbCalls++);

            sut.Object.GetCount();
            sut.Object.GetCount();
            sut.Object.GetCount();

            Assert.Equal(3, nbCalls);
        }

        [Trait("Matching Argument", "IsAny")]
        [Theory]
        [InlineData("Pictures of the aftermath", true)]
        [InlineData("of", true)]
        [InlineData("the", true)]
        [InlineData("aftermath", true)]
        public void IsAny(string str, bool expected)
        {
            var sut = new Mock<IFoo>();
            sut.Setup(foo => foo.DoSomething(It.IsAny<string>())).Returns(true);

            bool result = sut.Object.DoSomething(str);

            Assert.Equal(expected, result);
        }

        [Trait("Matching Arguments", "Is")]
        [Theory]
        [InlineData(0, true)]
        [InlineData(1, false)]
        [InlineData(2, true)]
        [InlineData(3, false)]
        public void Is(int number, bool expected)
        {
            var sut = new Mock<IFoo>();
            sut.Setup(foo => foo.Add(It.Is<int>(i => i % 2 == 0))).Returns(true);

            bool result = sut.Object.Add(number);

            Assert.Equal(expected, result);
        }

        [Trait("Matching Arguments", "IsInRange")]
        [Theory]
        [InlineData(0, true)]
        [InlineData(10, true)]
        [InlineData(7, true)]
        [InlineData(-4, false)]
        [InlineData(11, false)]
        public void IsInRange(int number, bool expected)
        {
            var sut = new Mock<IFoo>();
            sut.Setup(foo => foo.Add(It.IsInRange(0, 10, Range.Inclusive))).Returns(true);

            bool result = sut.Object.Add(number);

            Assert.Equal(expected, result);
        }

        [Trait("Matching Arguments", "IsRegex")]
        [Theory]
        [InlineData("aaaaaa", true)]
        [InlineData("c", true)]
        [InlineData("A", true)]
        [InlineData("abcdbDA", true)]
        [InlineData("ff", false)]
        [InlineData("GH", false)]
        public void IsRegex(string str, bool expected)
        {
            var sut = new Mock<IFoo>();
            sut.Setup(foo => foo.DoSomething(It.IsRegex("[a-d]+", RegexOptions.IgnoreCase))).Returns(true);

            bool result = sut.Object.DoSomething(str);

            Assert.Equal(expected, result);
        }


        [Trait("Method", "StubbingLinq")]
        [Fact]
        public void SimpleMethodStubbingWithLinq()
        {
            var sut = Mock.Of<IFoo>(foo => foo.GetCount() == 50);

            int result = sut.GetCount();

            Assert.Equal(50, result);
        }

        [Trait("Method", "Verify")]
        [Fact]
        public void VerifyMethodInvocation()
        {
            var sut = new Mock<IFoo>();
            sut.Object.Add(1);
            
            Exception exception = Record.Exception(() => sut.Verify(foo => foo.Add(2)));

            Assert.IsType<MockException>(exception);
        }

        [Trait("Properties", "Stubbing")]
        [Fact]
        public void SimplePropertyStubbing()
        {
            var sut = new Mock<IFoo>();
            sut.Setup(foo => foo.Name).Returns("John");

            string result = sut.Object.Name;

            Assert.Equal("John", result);
        }

        [Trait("Properties", "SetupGet")]
        [Fact]
        public void SetupGet()
        {
            var sut = new Mock<IFoo>();
            sut.SetupGet(foo => foo.Name).Returns("John");

            string result = sut.Object.Name;

            Assert.Equal("John", result);
        }

        [Trait("Properties", "SetupSet")]
        [Fact]
        public void SetupSet()
        {
            var sut = new Mock<IFoo>();
            sut.SetupSet(foo => foo.Name = "John").Verifiable();
            sut.Object.Name = "Rick";

            Assert.ThrowsAny<MockException>(() => sut.Verify());
        }

        [Trait("Properties", "SetupProperty")]
        [Fact]
        public void SetupProperty()
        {
            var sut = new Mock<IFoo>();
            sut.SetupProperty(foo => foo.Name, "John");

            string result = sut.Object.Name;

            Assert.Equal("John", result);
        }
        
        [Trait("Properties", "VerifySet")]
        [Fact]
        public void VerifySet()
        {
            var sut = new Mock<IFoo>();
            sut.Object.Name = "Doe";
            sut.VerifySet(foo => foo.Name = "Doe");
        }
    }
}