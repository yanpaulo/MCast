using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MCast.WebAPI.Models
{
    public class MessagingGroup
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public ICollection<GroupMessage> Messages { get; set; }

        #region FKs
        public string ApplicationUserId { get; set; }
        #endregion
    }
}
