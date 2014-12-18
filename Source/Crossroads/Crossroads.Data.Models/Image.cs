using Crossroads.Data.Common.Models;
using System;
using System.ComponentModel.DataAnnotations;

namespace Crossroads.Data.Models
{
    public class Image
    {
        [Key]
        public int Id { get; set; }

        public byte[] Content { get; set; }

        public string FileExtension { get; set; }
    }
}