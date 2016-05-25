using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Xunit;

namespace CheatSheet
{
    public class RegexCheatSheetTests
    {
        [Fact]
        public void MatchingString()
        {
            // 1st Capturing group ([a-zA-Z]+)
            //  [a-zA-Z]+ match a single character present in the list below
            //   Quantifier: + Between one and unlimited times, as many times as possible, giving back as needed [greedy]
            //   a-z a single character in the range between a and z (case sensitive)
            //   A-Z a single character in the range between A and Z (case sensitive)
            // 2nd Capturing group (\d+)
            //  \d+ match a digit [0-9]
            //  Quantifier: + Between one and unlimited times, as many times as possible, giving back as needed [greedy]            
            const string pattern = @"([a-zA-Z]+) (\d+)";
            
            Match sut = Regex.Match("June 24", pattern);
            
            Assert.Equal(true, sut.Success);
            Assert.Equal(0, sut.Index);
            Assert.Equal(7, sut.Length);
            // If the regular expression engine can find a match, the first element of the GroupCollection 
            // object (the element at index 0) returned by the Groups property contains a string that 
            // matches the entire regular expression pattern. Each subsequent element, from index one 
            // upward, represents a captured group, if the regular expression includes capturing groups.
            Assert.Equal(3, sut.Groups.Count);
            Assert.Equal(true, sut.Groups[0].Success);  
            Assert.Equal("June 24", sut.Groups[0].Value);
            Assert.Equal("June", sut.Groups[1].Value);
            Assert.Equal("24", sut.Groups[2].Value);
            Assert.Equal(1, sut.Captures.Count);
            Assert.Equal("June 24", sut.Value);
            Assert.Equal("June 24", sut.Captures[0].Value);
        }

        [Fact]
        public void CapturingGroups()
        {
            const string pattern = @"([a-zA-Z]+) (\d+)";

            MatchCollection sut = Regex.Matches("June 24, August 9, Dec 12", pattern);

            Assert.Equal(3, sut.Count);
            Assert.Equal(0, sut[0].Index);
            Assert.Equal("June 24", sut[0].Value);
            Assert.Equal(9, sut[1].Index);
            Assert.Equal("August 9", sut[1].Value);
            Assert.Equal(19, sut[2].Index);
            Assert.Equal("Dec 12", sut[2].Value);

            // The main difference between a Group object and a Capture object is that each Group object 
            // contains a collection of Captures representing all the intermediary matches by the group 
            // during the match, as well as the final text matched by the group.
            Assert.Equal("June", sut[0].Groups[1].Value);
            Assert.Equal("24", sut[0].Groups[2].Value);
            Assert.Equal("August", sut[1].Groups[1].Value);
            Assert.Equal("9", sut[1].Groups[2].Value);
            Assert.Equal("Dec", sut[2].Groups[1].Value);
            Assert.Equal("12", sut[2].Groups[2].Value);

            // The real utility of the Captures property occurs when a quantifier is applied to a capturing 
            // group so that the group captures multiple substrings in a single regular expression. 
            // In this case, the Group object contains information about the last captured substring, 
            // whereas the Captures property contains information about all the substrings captured 
            // by the group. In the following example, the regular expression \b(\w+\s*)+. matches 
            // an entire sentence that ends in a period. The group (\w+\s*)+ captures the individual 
            // words in the collection. Because the Group collection contains information only about 
            // the last captured substring, it captures the last word in the sentence, "sentence". 
            // However, each word captured by the group is available from the collection returned by 
            // the Captures property.
        }
    }
}
