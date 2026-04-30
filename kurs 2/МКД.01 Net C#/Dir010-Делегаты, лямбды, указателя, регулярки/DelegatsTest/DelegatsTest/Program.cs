int Add(int a, int b) => a + b;
int Multi(int a, int b) => a * b;

Operation operation;
operation = Add;
operation += Multi;
Console.WriteLine(operation(2, 4));

delegate int Operation(int a, int d);


