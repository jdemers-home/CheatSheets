namespace TddTests
{
    public class Bar
    {
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public byte Age { get; set; }

        public Bar()
        {
            FirstName = "Jane";
            MiddleName = "";
            LastName = "Smith";
            Age = 25;
        }
    }
}