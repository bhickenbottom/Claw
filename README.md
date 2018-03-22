# Claw
## .NET Core Component Framework

Claw is a very simple .NET Core component framework that doesn't allocate after the first call and is fast enough for most uses (including games).

## Getting Started

### Defining a Component Object

```
public class Thing : ComponentObject
{
    public void DoSomething(string s, int i)
    {
        Console.WriteLine($"Thing: DoSomething({s}, {i})");
        this.InvokeComponents(s, Boxer.Box(i));
    }
}
```

### Defining a Component

```
public class Behavior : Component
{
    public void DoSomething(Thing source, string s, Boxed<int> i)
    {
        Console.WriteLine($"Behavior: DoSomething({s}, {i})");
    }
}
```

### Creating a Component Object and extending it with a Component

```
Thing thing = new Thing();
thing.AddComponent<Behavior>();
thing.DoSomething("abcd", 1234);
Console.ReadLine();
```

Result:

```
Thing: DoSomething(abcd, 1234)
Behavior: DoSomething(abcd, 1234)
```

### Returning Results

```
public class Thing : ComponentObject
{
    public bool DoSomethingResult(string s, int i)
    {
        Console.WriteLine($"Thing: DoSomethingResult({s}, {i})");
        Boxed<bool> result = Boxer.Box(false);
        this.InvokeComponents(s, Boxer.Box(i), result);
        return result;
    }
}
```

```
public class Behavior : Component
{
    public void DoSomethingResult(Thing source, string s, Boxed<int> i, Boxed<bool> result)
    {
        Console.WriteLine($"Behavior: DoSomethingResult({s}, {i})");
        result.Value = true;
    }
}
```

```
Thing thing = new Thing();
thing.AddComponent<Behavior>();
bool result = thing.DoSomethingResult("efgh", 5678);
Console.WriteLine($"Result: {result}");
Console.ReadLine();
```

Result:

```
Thing: DoSomethingResult(efgh, 5678)
Behavior: DoSomethingResult(efgh, 5678)
Result: True
```
