﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GunStore.Models
{
    public class Dealer
    {
        [Key]
        public int Id { get; set; }

        [DisplayName("First Name")]
        public string FirstName { get; set; }

        [DisplayName("Last Name")]
        public string LastName { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public int Reliability { get; set; }

        public virtual List<Gun> Guns { get; set; }

        public Dealer()
        {
        }
    }
}