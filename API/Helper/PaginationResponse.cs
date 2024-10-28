using System;

namespace API.Helper;

public class PaginationResponse<T>
{
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
        public int TotalPages { get; set; }
        public T data { get; set; }

        public PaginationResponse(int currentPage, int pageSize, int totalItems, int totalPages, T data) 
        { 
            CurrentPage = currentPage;
            PageSize = pageSize;
            TotalItems = totalItems;
            TotalPages = totalPages;
            this.data = data;
    }
    }
