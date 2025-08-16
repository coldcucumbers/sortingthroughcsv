using System.Globalization; // for culture info??
using System.Runtime.CompilerServices;
using CsvHelper; //main csv helper functions
using System.Linq;
using System.Text.Json;

using StreamReader reader = new StreamReader("sample_orders.csv");
using CsvReader csv = new CsvReader(reader, CultureInfo.InvariantCulture);
List<Order> orders = csv.GetRecords<Order>().ToList();

/* decimal totalUnits = orders.Sum((order) =>
{
    if (order.Quantity % 2 == 0)
    {
        return order.Quantity * order.Price;
    }
    else return 0;
}); */

int totalUnits = orders.Sum(order => order.Quantity);
decimal totalRevenue = orders.Sum(order => order.Quantity * order.Price);

File.WriteAllText("orders.json", JsonSerializer.Serialize(orders));
File.WriteAllText("orders_selected.json", JsonSerializer.Serialize(
    orders.Select(x => new { x.Product, x.Price }) ));

var productUnitTotals = orders
    .GroupBy(o => o.Product) // group products by name 
    .Select(g => new
    {
        Product = g.Key,
        Units = g.Sum(o => o.Quantity)
    })
.ToList();

var bestSeller = productUnitTotals
.OrderByDescending(x => x.Units)
.First ();

static void WriteColored(string label, ConsoleColor labelColor, string value, ConsoleColor valueColor)
{
    // 1) Save whatever color the console is currently using.
    var original = Console.ForegroundColor;

    // 2) Print the label (e.g., "Total revenue: ") in the label color.
    Console.ForegroundColor = labelColor;
    Console.Write(label);           // Write (no newline) keeps the next part on the same line.

    // 3) Print the value (e.g., "$12,345.67") in the value color.
    Console.ForegroundColor = valueColor;
    Console.WriteLine(value);       // WriteLine finishes the line with a newline.

    // 4) Put the console color back 
    Console.ForegroundColor = original;
}

var unitsColor = (bestSeller.Units % 2 == 0 ? ConsoleColor.Green : ConsoleColor.Red);
WriteColored("Total units sold: ", ConsoleColor.Blue, totalUnits.ToString(), ConsoleColor.White);
WriteColored("Total revenue: ", ConsoleColor.Yellow, totalRevenue.ToString("C", CultureInfo.CurrentCulture), ConsoleColor.White);
var original = Console.ForegroundColor;
Console.ForegroundColor = ConsoleColor.Magenta;
Console.Write("Best-selling product: ");

Console.ForegroundColor = ConsoleColor.White;
Console.Write(bestSeller.Product);
Console.Write(" (Units: ");

Console.ForegroundColor = unitsColor;
Console.Write(bestSeller.Units);

Console.ForegroundColor = ConsoleColor.White;
Console.WriteLine(")");

Console.ForegroundColor = original;

