using System.Reflection;
using AdventOfCode;

var currentDay = 1;// DateOnly.FromDateTime(DateTime.UtcNow).Day;

Console.WriteLine($"Current day: {currentDay}");

var currentType = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(PuzzleBase)) && (t?.FullName?.Contains($".Day{currentDay:D02}.") ?? false)).FirstOrDefault();
if (currentType == null)
{
    Console.WriteLine("No calculation found for the current day.");
    return -1;
}

Console.WriteLine($"Starting calculation...");

var puzzle = (PuzzleBase)Activator.CreateInstance(currentType);
await puzzle.AssertAsync();
var result = await puzzle.CalculateAsync();

Console.WriteLine($"Result: {result}");
ClipboardService.SetText(result);

Console.WriteLine($"Press any key...");
Console.ReadKey();

return 0;