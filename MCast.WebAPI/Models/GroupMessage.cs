using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCast.WebAPI.Models
{
    public class GroupMessage
    {
        public int Id { get; set; }

        [Required]
        public string Content { get; set; }

        public DateTimeOffset Sent { get; set; }

        #region ForeignKeys
        public int MessagingGroupId { get; set; } 
        #endregion
    }
}
