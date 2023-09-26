using System;
using System.Collections.Generic;

namespace LibraryDesktop.DataLayer;

public partial class Tag
{
    public string TagId { get; set; } = null!;

    public virtual ICollection<Book> BooksBooks { get; set; } = new List<Book>();
}
