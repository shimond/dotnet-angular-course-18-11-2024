

//Console.WriteLine("Started");
//Do1();
//Console.WriteLine( "Finished");
//Console.ReadLine();

Person p = new(1, "asdasd");
Change1(p);
Change(p);
Console.WriteLine(  p.ID);
Console.WriteLine(  p.Name);

void Change(Person person)
{
    person.Name = "MOshe";
}

void Change1(Person person)
{
    person.ID= 99;
}

async Task Do1()
{
    Console.WriteLine(  "Do1 A");
    await Task.Delay(1000);
    Do2();
    Console.WriteLine(  "Do1 B");
}

async Task Do2()
{
    Console.WriteLine("Do2 A");
    for (int i = 0; i < 10; i++)
    {
        Console.WriteLine("DO2 = " + i);
        await Task.Delay(5);
    }
    Console.WriteLine("Do2 B");
}


public record Person(int ID ,string Name );
