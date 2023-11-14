---
theme: dracula
background: /ante-hamersmit-qg6MDcCWBfM-unsplash.jpg
highlighter: shiki
lineNumbers: false
drawings:
  persist: false
transition: slide-up
title: C# async
mdc: true
colorSchema: dark
---

# C# async / await

by Kenneth Fossen

---
layout: default

---

# async keyword

<v-click>

```csharp
T SomeMethod(args..) {
  OneOrMoreOperations<T>()

  ...
  return OneOrMoreOperations<T>()
}
```
</v-click>

<v-click>

```csharp {all|1|2,4}
async Task<T> SomeMethodAsync(args...) {
  await OneOrMoreAsyncOperations<T>()
  ...
  return await OneOrMoreAsyncOperations<T>()
}
```
</v-click>

---
layout: center
class: text-center
---

# What does this mean for your method(s)?

<div v-click>

It will under the hood implement `IAsycStateMachine`

</div>

---
layout: two-cols
---
# Async Main

<div v-click>

```csharp
namespace AsyncMain;

public static class Program
{
    public static async Task Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        const string url = "https://httpstat.us/200";

        var httpClient = new HttpClient();
        var response = await httpClient
            .GetAsync(url);

        var content = response.Content
            .ReadAsStringAsync();

        Console.WriteLine(content);
    }
}
```

</div>

---
layout: default
---
# IL Code

```csharp {all|6,8|9-15|20-24}
namespace AsyncMain
{
  public static class Program
  {
    [NullableContext(1)]
    [AsyncStateMachine(typeof (Program.<Main>d__0))]
    [DebuggerStepThrough]
    public static Task Main(string[] args)
    {
      Program.<Main>d__0 stateMachine = new Program.<Main>d__0();
      stateMachine.<>t__builder = AsyncTaskMethodBuilder.Create();
      stateMachine.args = args;
      stateMachine.<>1__state = -1;
      stateMachine.<>t__builder.Start<Program.<Main>d__0>(ref stateMachine);
      return stateMachine.<>t__builder.Task;
    }

    [DebuggerStepThrough]
    [SpecialName]
    private static void <Main>([Nullable(1)] string[] args)
    {
      Program.Main(args).GetAwaiter().GetResult();
    }
```

---
layout: default
---

```csharp {all|16-19|21|30-32|24} {maxHeight:'400px'}
[CompilerGenerated]
    private sealed class <Main>d__0 : IAsyncStateMachine
    {
      public int <>1__state;
      public AsyncTaskMethodBuilder <>t__builder;
      [Nullable(new byte[] {0, 1})]
      public string[] args;
      private HttpClient <httpClient>5__1;
      private HttpResponseMessage <response>5__2;
      [Nullable(new byte[] {0, 1})]
      private Task<string> <content>5__3;
      private HttpResponseMessage <>s__4;
      [Nullable(new byte[] {0, 1})]
      private TaskAwaiter<HttpResponseMessage> <>u__1;

      public <Main>d__0()
      {
        base..ctor();
      }

      void IAsyncStateMachine.MoveNext()
      {
        int num1 = this.<>1__state;
        try
        {
          TaskAwaiter<HttpResponseMessage> awaiter;
          int num2;
          if (num1 != 0)
          {
            Console.WriteLine("Hello, World!");
            this.<httpClient>5__1 = new HttpClient();
            awaiter = this.<httpClient>5__1.GetAsync("https://httpstat.us/200").GetAwaiter();
            if (!awaiter.IsCompleted)
            {
              this.<>1__state = num2 = 0;
              this.<>u__1 = awaiter;
              Program.<Main>d__0 stateMachine = this;
              this.<>t__builder.AwaitUnsafeOnCompleted<TaskAwaiter<HttpResponseMessage>, Program.<Main>d__0>(ref awaiter, ref stateMachine);
              return;
            }
          }
          else
          {
            awaiter = this.<>u__1;
            this.<>u__1 = new TaskAwaiter<HttpResponseMessage>();
            this.<>1__state = num2 = -1;
          }
          this.<>s__4 = awaiter.GetResult();
          this.<response>5__2 = this.<>s__4;
          this.<>s__4 = (HttpResponseMessage) null;
          this.<content>5__3 = this.<response>5__2.Content.ReadAsStringAsync();
          Console.WriteLine((object) this.<content>5__3);
        }
        catch (Exception ex)
        {
          this.<>1__state = -2;
          this.<httpClient>5__1 = (HttpClient) null;
          this.<response>5__2 = (HttpResponseMessage) null;
          this.<content>5__3 = (Task<string>) null;
          this.<>t__builder.SetException(ex);
          return;
        }
        this.<>1__state = -2;
        this.<httpClient>5__1 = (HttpClient) null;
        this.<response>5__2 = (HttpResponseMessage) null;
        this.<content>5__3 = (Task<string>) null;
        this.<>t__builder.SetResult();
      }

      [DebuggerHidden]
      void IAsyncStateMachine.SetStateMachine([Nullable(1)] IAsyncStateMachine stateMachine)
      {
      }
    }
  }
}

```


---
layout: two-cols
transition: slide-up
---

:: default ::

# Async Main done right

```csharp
namespace AsyncMain;

public static class Program
{
    public static void Main(string[] args)
    {
        Console.WriteLine("Hello, World!");

        const string url = "https://httpstat.us/200";

        var httpClient = new HttpClient();
        var response = httpClient
            .GetAsync(url)
            .GetAwaiter()
            .GetResult();

        var content = response.Content
            .ReadAsStringAsync()
            .GetAwaiter()
            .GetResult();

        Console.WriteLine(content);
    }
}

```
:: right ::


<div v-click>

# IL Code


```csharp
using System;
using System.Net.Http;
using System.Runtime.CompilerServices;

namespace AsyncMain
{
  public static class Program
  {
    [NullableContext(1)]
    public static void Main(string[] args)
    {
      Console.WriteLine("Hello, World!");
      Console.WriteLine(new HttpClient()
          .GetAsync("https://httpstat.us/200")
            .GetAwaiter()
            .GetResult()
            .Content
            .ReadAsStringAsync()
            .GetAwaiter()
            .GetResult()
          );
    }
  }
}

```

</div>







---


# Async Best Practices


| **Name**           | **Description**                       | **Exceptions**                                                                                        |
| ------------------ | ------------------------------------- | ----------------------------------------------------------------------------------------------------- |
| Avoid `async void` | Prefer `async Task`                   | Event handlers                                                                                        |
| Async all the way  | Don’t mix blocking and async code.    | Console Main method                                                                                   |
| Configure Context  | `.ConfigureAwait(false)` when you can | Methods that require context, and in frameworks that don't have a SyncronizationContext (ASP.NET Core) |


---
layout: default
---


| **To Do this**                           | **Instead of This**      | **Use This**       |
| ---------------------------------------- | ------------------------ | ------------------ |
| Retrigve the reult of an background task | `Task.Wait` or `Task.Result` | `await`              |
| Wait for any task to complete            | `Task.WaitAny`             | `await Task.WhenAny` |
| Retrive the results of multiple tasks    | `Task.WaitAll`             | `await Task.WhenAll` |
| Wait a period of time                    | `Thread.Sleep`             | `await Task.Delay`   |

- Use `.GetWaiter().GetResult()`


---
layout: two-cols
---

:: default ::

# When async? 

<v-clicks at="1">

# `async` all the way!

</v-clicks>

<v-clicks at="2">

- Except `return await`

</v-clicks>

:: right ::

# Example

<div v-click-hide>

```csharp
async Task<T> SomeOperationAsync(args..)
{
  var operation = await OperationAsync();
  .. non async transform data ..
  return T
}

```
</div>

<div v-click>

```csharp
async Task<T> SomeOperationAsync(args...) 
{
  return await OperationAsync(args)
}

```
</div>
<div v-click>

```csharp
Task<T> SomeOperationAsync(args...) 
{
  return OperationAsync(args) // returns Task<T>
}
```

</div>

---

# AsyncEnumerable

Creating streams

```csharp
await foreach (var item in IntegerListAsync(start, count).WithCancellation(cts.Token))
{
    Console.WriteLine($"PrintRange: {Environment.CurrentManagedThreadId}");
    Console.WriteLine(item);
}

private async IAsyncEnumerable<int> IntegerListAsync(int start, int count,
        [EnumeratorCancellation] CancellationToken cancellationToken = default)

```
---
layout: two-cols
---

:: default ::

# async / await and PLINQ

- They don't mix
- But there is a way!

:: right ::

<div v-click>

# PLINQ and async mix

```csharp
kefoPlinq
    .GetRange() // Enumerable
    .AsParallel() // PLINQ
    .WithDegreeOfParallelism(20)
    .Select(task =>
    {
        KefoPlinq.WaitBeforeReturn(task)
          .GetAwaiter()
          .GetResult(); // async task
        return task;
    })
    .ForAll(x =>
    {
        Console.WriteLine(
          $"RangeID: {x} : ThreadID: {ThreadId}"
        );
    });
```
</div>

---
layout: center
class: text-center
---

END

