using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Xunit.Abstractions;

namespace Tdd.Tests
{
    public class XUnitCheatSheetTests :
        IDisposable // Allows TearDown via Dispose()
    {
        private readonly ITestOutputHelper output; // Capturing Output

        public XUnitCheatSheetTests(ITestOutputHelper output)
        {
            // Setup
            // Contructor is called for each test.
            this.output = output;
            output.WriteLine("XUnitCheatSheetTests Constructor");
        }

        public void Dispose()
        {
            // Teardown
            // Dispose is called for each test.
            output.WriteLine("XUnitCheatSheetTests.Dispose()");
        }

        [Fact] // Tests which are always true. They test invariant conditions.
        public void SimpleFact()
        {
            Assert.True(true);
        }

        [Theory, // Theories are tests which are only true for a particular set of data.
        InlineData(1, true), // InLineData allows multiple traversals of the same test with different parameters
        InlineData(2, true),
        InlineData(3, false)]
        public void SimpleTheory(int number, bool expectedResult)
        {
            output.WriteLine("XUnitCheatSheetTests.SimpleTheory(int {0}, bool {1})", number, expectedResult);
            Assert.Equal(number < 3, expectedResult);
        }

        public static IEnumerable<object[]> DataByProperty
        {
            get
            {
                yield return new object[] { 1, true };
                yield return new object[] { 2, true };
                yield return new object[] { 3, false };
            }
        }

        [Theory, 
        MemberData("DataByProperty")] // Data source for theory, must be something static returning IEnumerable<object[]>
        public void SimpleTheoryWithDataProperty(int number, bool expectedResult)
        {
            output.WriteLine("XUnitCheatSheetTests.SimpleTheory(int {0}, bool {1})", number, expectedResult);
            Assert.Equal(number < 3, expectedResult);
        }

        [Fact]
        public void AssertionExamples()
        {
            output.WriteLine("XUnitCheatSheetTests.AssertionExamples()");

            Assert.Equal(1, 1);
            Assert.Equal(3.14159, 3.14, 2); // 3rd parameter is precision.
            Assert.Equal(3.14159, 3.142, 3);
            Assert.Equal(new List<string> { "A", "B", "C" }, new List<string> { "a", "b", "c" }, StringComparer.CurrentCultureIgnoreCase);
            Assert.NotEqual(3.14159, 3.141, 3);
            
            Assert.Throws<FileNotFoundException>( (Action) (() => { throw new FileNotFoundException(); } ) );
            Assert.ThrowsAny<Exception>((Action)(() => { throw new FileNotFoundException(); }));

            var items = new[] { 1, 1, 1 };
            
            Assert.All(items, x => Assert.Equal(1, x));
            Assert.Collection(items, 
                item => Assert.Equal(1, item),
                item => Assert.Equal(1, item),
                item => Assert.Equal(1, item));

            Assert.Contains(1, items);
            Assert.Contains("a", "ABC", StringComparison.CurrentCultureIgnoreCase);
            Assert.DoesNotContain(2, items);
            Assert.DoesNotContain("d", "ABC", StringComparison.CurrentCultureIgnoreCase);

            Assert.Empty(new List<object>());
            Assert.NotEmpty(items);

            Assert.InRange(50, 0, 100);
            Assert.NotInRange(50, 0, 30);

            Assert.IsNotType(typeof(Exception), new FileLoadException());
            Assert.IsType(typeof(Exception), new Exception());

            var foo = new object();
            var bar = new object();

            Assert.Same(foo, foo);
            Assert.NotSame(foo, bar);

            Assert.NotNull(false);
            Assert.Null(null);
            Assert.False(false);
            Assert.True(true);
        }
    }
}