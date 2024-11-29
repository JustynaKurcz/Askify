namespace Askify.Shared.Pagination;

public abstract class PagedQuery
{
    private int? _pageNumber;
    private int? _pageSize;
    
    public int? PageNumber 
    { 
        get => _pageNumber; 
        set => _pageNumber = value < 1 ? 1 : value; 
    }
    
    public int? PageSize 
    { 
        get => _pageSize; 
        set => _pageSize = value < 1 ? 10 : value; 
    }

    public int GetPageNumber() => PageNumber ?? 1;
    public int GetPageSize() => PageSize ?? 10;
}