using System;

namespace API.Helper;

public class PaginationParams
{
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
    public string FilterBy { get; set; } = String.Empty;
    public string FilterQuery { get; set; } = string.Empty;
    public int SortDirection { get; set; } = 1;
    public string SortBy { get; set; } = String.Empty;
}
