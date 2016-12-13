using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace wall.Models
{
 public class Message : BaseEntity
 {
  [Key]
  public long Id { get; set; }
  [Required]
  [MinLength(8)]
  public string message { get; set; }
  public User user { get; set;}
  public DateTime created_at { get; set; }
  public ICollection<Comment> comments { get; set; }
  public long user_id {get;set;}
  }
}