// See https://aka.ms/new-console-template for more information
using Day1;

Console.WriteLine("Hello, World!");

var zerosReached = Example1.ReturnZerosReached();

var zerosClicked = Example2.ReturnZerosClicked();

var zerosClickedNew = Example2.ReturnZerosClickedNew();

Console.WriteLine($"Zeros reached: {zerosReached}, zeros clicked: {zerosClicked}, {zerosClickedNew}");

Console.ReadKey(); 
