using System.ComponentModel.DataAnnotations;
using System;
using System.Collections.Generic;

namespace wall.Models
{
 public abstract class BaseEntity {}
 public class Comment : BaseEntity
 {
  [Key]
  public long Id { get; set; }
  [Required]
  [MinLength(8)]
  public string comment { get; set; }
  public User user { get; set;}
  public DateTime created_at { get; set; }
  public Message message { get; set;}
  public long message_id { get; set;}
  public long user_id {get;set;}
  }
}