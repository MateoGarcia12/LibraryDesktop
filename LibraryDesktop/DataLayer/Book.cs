using System;
using System.Collections.Generic;

namespace LibraryDesktop.DataLayer;

public partial class Book
{
    public int BookId { get; set; }

    public string Title { get; set; } = null!;

    public string Description { get; set; } = null!;

    public DateTime PublishedOn { get; set; }

    public string Publisher { get; set; } = null!;

    public decimal Price { get; set; }

    public string ImageUrl { get; set; } = null!;

    public virtual ICollection<BookAuthor> BookAuthors { get; set; } = new List<BookAuthor>();

    public virtual PriceOffer? PriceOffer { get; set; }

    public virtual ICollection<Review> Reviews { get; set; } = new List<Review>();

    public virtual ICollection<Tag> TagsTags { get; set; } = new List<Tag>();
}
