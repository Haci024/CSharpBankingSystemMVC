﻿using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace EntityLayer.Concrete
{
    public class BlogNews
    {
        public int ID { get; set; }

        public DateTime CreateDate { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string AuthorName { get; set; }

        public bool IsDeactive { get; set; }

        public string Image { get; set; }
        [NotMapped]
        public IFormFile Photo { get; set; }

    }
}
