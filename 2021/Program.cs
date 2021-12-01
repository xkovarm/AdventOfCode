﻿using System.Reflection;
using AdventOfCode;

var currentDate = DateOnly.FromDateTime(DateTime.UtcNow);

Console.WriteLine($"Current day: {currentDate.Day}");

var currentType = Assembly.GetExecutingAssembly().GetTypes().Where(t => t.IsSubclassOf(typeof(PuzzleBase)) && (t?.FullName?.Contains($".Day{currentDate.Day:D02}.") ?? false)).FirstOrDefault();
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

return 0;