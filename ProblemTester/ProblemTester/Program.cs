using System;
using System.Diagnostics;

// for example, problemToRun = "Week1_A_Echo"
Console.WriteLine("Write name of problem to run");
var problemToRun = Console.ReadLine();

try
{
    // Find the c# application
    var thisDirectory = System.IO.Directory.GetCurrentDirectory();
    var baseDirectory = System.IO.Directory.GetParent(thisDirectory)!.Parent!.Parent!.Parent!.Parent;

    var problemDirectory = $"{baseDirectory!.FullName}\\{problemToRun}\\{problemToRun}";
    var problemExecutable = $"{problemDirectory}\\bin\\Debug\\net7.0\\{problemToRun}.exe";

    var testDataInputDirectory = new DirectoryInfo($"{problemDirectory}\\TestData\\Input");
    var testDataOutputDirectory = new DirectoryInfo($"{problemDirectory}\\TestData\\Output");
    var noOfTestCases = testDataInputDirectory.EnumerateFiles().Count();

    Console.WriteLine($"Running {noOfTestCases} test cases for problem: {problemToRun}");

    var failureDictionary = new Dictionary<int, bool>();

    for (int i = 0; i < noOfTestCases; i++)
    {
        Console.WriteLine($"Running test case no. {i + 1}");

        var input = File.ReadAllText($"{testDataInputDirectory}\\TestCase_{i + 1}_Input.txt");
        var output = File.ReadAllText($"{testDataOutputDirectory}\\TestCase_{i + 1}_Output.txt");
        var correct = RunProblem(problemExecutable, input, output);

        failureDictionary[i] = correct;
    }

    var totalSuccesses = 0;
    for (int dictEntry = 0; dictEntry < failureDictionary.Keys.Count; dictEntry++)
    {
        var dictValue = failureDictionary[dictEntry];
        if (dictValue)
        {
            totalSuccesses++;
        }

        var color = dictValue ? ConsoleColor.Green : ConsoleColor.DarkRed;
        Console.Write($"Test case no. {dictEntry} succeeded: ");
        Console.ForegroundColor = color;
        Console.Write(dictValue);
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine();
    }

    Console.WriteLine($"Success rate: {(totalSuccesses / noOfTestCases) * 100}%");

    Console.WriteLine("Press any key to close this window");
    Console.ReadLine();
}
catch (Exception)
{
    Console.WriteLine("Could not find/run project. Exiting.");
    Environment.Exit(0);
}

bool RunProblem(string filePath, string input, string expectedOutput)
{
    Console.WriteLine("--- --- --- --- ---");
    Console.WriteLine($"Input data: {input}");

    // Settings for running the c# application
    var startInfo = new ProcessStartInfo(filePath)
    {
        CreateNoWindow = false,
        UseShellExecute = false,
        RedirectStandardInput = true,
        RedirectStandardOutput = true
    };

    Process process = Process.Start(startInfo)!;
    process.StandardInput.WriteLine(input);
    var actualOutput = process.StandardOutput.ReadToEnd();
    actualOutput = actualOutput.Replace("\n", "").Replace("\r", ""); //clean output

    Console.WriteLine($"Expected output:    {expectedOutput} \n" +
                      $"Actual output:      {actualOutput}");

    var isCorrect = expectedOutput == actualOutput;
    Console.WriteLine($"Output is equal: {isCorrect}");

    process!.WaitForExit();
    Console.WriteLine("--- --- --- --- ---");

    return isCorrect;
}


