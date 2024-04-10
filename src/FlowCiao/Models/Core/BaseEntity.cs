using System;
using System.ComponentModel.DataAnnotations;

namespace FlowCiao.Models.Core;

public class BaseEntity
{
    [Key]
    public Guid Id { get; set; }
}