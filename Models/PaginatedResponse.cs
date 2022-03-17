namespace SWGEmuModManagerApi.Models;

public class PaginatedResponse<T> : Response<T>
{
    public PaginatedResponse(T? data, int totalItemCount, int pageSize, int pageNumber, int pageCount, bool isLastPage, 
        bool isFirstPage, int lastItemOnPage, int firstItemOnPage, bool hasPreviousPage, bool hasNextPage)
    {
        HasNextPage = hasNextPage;
        HasPreviousPage = hasPreviousPage;
        FirstItemOnPage = firstItemOnPage;
        LastItemOnPage = lastItemOnPage;
        IsFirstPage = isFirstPage;
        IsLastPage = isLastPage;
        PageCount = pageCount;
        PageNumber = pageNumber;
        PageSize = pageSize;
        TotalItemCount = totalItemCount;

        Succeeded = true;
        Message = string.Empty;
        Errors = null;
        Data = data;
    }

    public bool HasNextPage { get; set; }
    public bool HasPreviousPage { get; set; }
    public int FirstItemOnPage { get; set; }
    public int LastItemOnPage { get; set; }
    public bool IsFirstPage { get; set; }
    public bool IsLastPage { get; set; }
    public int PageCount { get; set; }
    public int PageNumber { get; set; }
    public int PageSize  { get; set; }
    public int TotalItemCount { get; set; }
}
