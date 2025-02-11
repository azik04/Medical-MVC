﻿namespace MedicalMVC.Response;
public class PagedResponse<T> : BaseResponse<T>
{
    public int? CurrentPage { get; set; }
    public int? TotalPages { get; set; }
    public int? PageSize { get; set; }
    public int? TotalCount { get; set; }
}