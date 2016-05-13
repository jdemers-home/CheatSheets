namespace TddTests
{
    public interface IFoo
    {
        bool DoSomething(string str);
        string DoSomethingStringy(string str);
        bool TryParse(string str, out string strout);
        bool Submit(ref Bar bar);
        int GetCount();
        int GetCountThing();
        bool Add(int number);

        string Name { get; set; }
    }
}