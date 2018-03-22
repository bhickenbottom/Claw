using System;
using ComponentFramework;

namespace ComponentFrameworkSample
{
    public class Thing : ComponentObject
    {
        public void DoSomething(string s, int i)
        {
            Console.WriteLine($"Thing: DoSomething({s}, {i})");
            this.InvokeComponents(s, Boxer.Box(i));
        }

        public bool DoSomethingResult(string s, int i)
        {
            Console.WriteLine($"Thing: DoSomethingResult({s}, {i})");
            Boxed<bool> result = Boxer.Box(false);
            this.InvokeComponents(s, Boxer.Box(i), result);
            return result;
        }
    }

    public class Behavior : Component
    {
        public void DoSomething(Thing source, string s, Boxed<int> i)
        {
            Console.WriteLine($"Behavior: DoSomething({s}, {i})");
        }

        public void DoSomethingResult(Thing source, string s, Boxed<int> i, Boxed<bool> result)
        {
            Console.WriteLine($"Behavior: DoSomethingResult({s}, {i})");
            result.Value = true;
        }
    }

    public class Program
    {
        static void Main(string[] args)
        {
            Thing thing = new Thing();
            thing.AddComponent<Behavior>();
            thing.DoSomething("abcd", 1234);
            bool result = thing.DoSomethingResult("efgh", 5678);
            Console.WriteLine($"Result: {result}");
            Console.ReadLine();
        }
    }
}
