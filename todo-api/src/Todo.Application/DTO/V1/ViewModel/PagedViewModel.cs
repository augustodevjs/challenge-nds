﻿namespace Todo.Application.DTO.V1.ViewModel;

public class PagedViewModel<T>
{
    public List<T> Items { get; set; } = new();
    public int Page { get; set; }
    public int Total { get; set; }
    public int PerPage { get; set; }
    public int PageCount { get; set; }
}