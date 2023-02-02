using System;
using System.Data;

namespace DevinSite.ViewModels;

public class AssignmentVM
{
    public string Title { get; set; } = default!;
    public string? Course { get; set; }
    public string? Details { get; set; }
    public DateTime DueDate { get; set; }

    public static implicit operator Assignment(AssignmentVM vm) => new Assignment()
    {
        Title = vm.Title,
        Details = vm.Details,
        DueDate = vm.DueDate,
        IsEditting = true
    };

    public override string ToString()
    {
        return $"{Title}\n{Course}\n{Details}\n{DueDate}";
    }
}

