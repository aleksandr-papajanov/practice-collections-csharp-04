// See https://aka.ms/new-console-template for more information  

ExamineList();
ExamineQueue();
ExamineStack();
CheckBrackets();


void ExamineList()
{
    Console.WriteLine("Examining List<T> capacity changes...");

    List<int> ints = new();

    int capacity = ints.Capacity;
    for (int i = 0; i < 1000000; i++)
    {
        ints.Add(i);

        if (ints.Capacity != capacity)
        {
            Console.WriteLine($"At item {i + 1}, capacity changed from {capacity} to {ints.Capacity} ({ints.Capacity - capacity} palces)");
            capacity = ints.Capacity;
        }
    }

    ints.Clear();

    if (ints.Capacity == capacity)
    {
        Console.WriteLine($"After removing all items, capacity remains at {capacity}.");
    }

    Console.WriteLine();
}

void ExamineQueue()
{
    Console.WriteLine("Examining Queue<T>...");

    Queue<string> q = new();
    Stack<string> names = new Stack<string>([ "Alice", "Bob", "Charlie", "Diana", "Eve" ]);

    string? customer = null;
    string? lastEnqueued = null;

    do
    {
        // checkout
        if (q.Count == 1)
        {
            var first = q.Peek();
            customer = customer != first ? first : null;
        }

        if (lastEnqueued != customer)
        {
            Console.WriteLine($"{q.Dequeue()} leaves the queue");
        }

        if (names.TryPop(out string? name))
        {
            q.Enqueue(name);
            lastEnqueued = name;
            Console.WriteLine($"{name} stands in the queue");
        }
    } while (q.Count != 0);

    Console.WriteLine();
}

void ExamineStack()
{
    Console.WriteLine("Examining Stack<T>...");
    Console.Write("Enter text: ");
    var text = Console.ReadLine() ?? string.Empty;

    var reversed = ReverseText(text);

    Console.WriteLine($"Reversed text: {reversed}");
    Console.WriteLine();
}

string ReverseText(string text)
{
    Stack<char> stack = new();

    foreach (char c in text)
    {
        stack.Push(c);
    }

    var reversed = string.Empty;
    while(stack.TryPop(out char top))
    {
        reversed += top;
    }

    return reversed;
}


void CheckBrackets()
{
    Console.WriteLine("Examining brackets...");
    Console.Write("Enter any text: ");

    var text = Console.ReadLine() ?? string.Empty;
    //var text = "[{}{}[]() ({[]})]";

    Console.WriteLine($"Checking brackets in: {text}");

    Dictionary<char, char> pairs = new()
    {
        { '(', ')' },
        { '{', '}' },
        { '[', ']' }
    };

    if (IsSymbolBalanced(text, pairs))    
    {
        Console.WriteLine("Brackets are balanced!");
    }
    else
    {
        Console.WriteLine("Brackets are not balanced!");
    }
}

bool IsSymbolBalanced(string text, Dictionary<char, char> pairs)
{
    Stack<char> stack = new();

    foreach (char current in text)
    {
        if (pairs.Select(x => x.Key).Any(x => x == current))
        {
            stack.Push(current);
            continue;
        } 

        var closing = pairs.Select(x => x.Value).FirstOrDefault(x => x == current);

        if (closing != char.MinValue)
        {
            // If there's no respective bracket for the closing one, return false
            if (!stack.TryPeek(out char top))
            {
                return false;
            }

            // If there is no match for the closing bracket, return false
            if (!pairs.Where(x => x.Key == top && x.Value == closing).Any())
            {
                return false;
            }

            stack.Pop();
        }
    }

    return stack.Count == 0;
}