using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

var input = Console.ReadLine();
var charQueue = new Queue<char>(input!.ToCharArray());
var charStack = new Stack<char>();

while (charQueue.Count > 0)
{
    var c = charQueue.Dequeue();
    if (c == '<')
    {
        charStack.Pop();
    }
    else
    {
        charStack.Push(c);
    }
}

var newString = new string(charStack.ToArray().Reverse().ToArray()); //lmao
Console.WriteLine(newString);