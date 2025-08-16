public class Order
// establishing a new type called "Order", in this set we are telling C# to gather this
// data from the rows
{
    public string Product { get; set; } = ""; //incase of null 
    public int Quantity { get; set; }
    public decimal Price { get; set; }
}