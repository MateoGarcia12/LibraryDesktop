using System;
using System.Collections.Generic;

namespace LibraryDesktop.DataLayer;

public partial class PriceOffer
{
    public int PriceOfferId { get; set; }

    public decimal NewPrice { get; set; }

    public string PromotionalText { get; set; } = null!;

    public int BookId { get; set; }

    public virtual Book Book { get; set; } = null!;
}
