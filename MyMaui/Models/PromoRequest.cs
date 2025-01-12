namespace MyMaui.Models;

public class PromoRequest
{
    public string PromoAppCode { get; set; } = string.Empty;

    public string TransDate { get; set; } = string.Empty;

    public string ZoneCode { get; set; } = string.Empty;

    public string SiteCode { get; set; } = string.Empty;

    public List<ItemProduct> ItemProduct { get; set; } = [];

    public List<Mop> Mop { get; set; } = [];
}

public class ItemProduct
{
    public int Price { get; set; }

    public int Qty { get; set; }
}

public class Mop
{
    public string MopCode { get; set; } = string.Empty;

    public int Amount { get; set; }
}