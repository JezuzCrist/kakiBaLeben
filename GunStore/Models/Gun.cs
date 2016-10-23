using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace GunStore.Models
{
    public class Gun
    {
        [Key]
        public int Id { get; set; }
        [DisplayName("Dealer")]
        public int? DealerId { get; set; }

        [DisplayName("Gun Name")]
        public string Name { get; set; }
        public string Description { get; set; }

        [DisplayFormat(DataFormatString = "US ${0:0.00}", ApplyFormatInEditMode = false)]
        public double Price { get; set; }

        [DisplayName("Ammo Type")]
        public string AmmoType { get; set; }
        public bool IsPhotoExists { get; set; }
        public virtual List<Review> Reviews { get; set; }
        public Dealer Dealer { get; set; }

        public Gun()
        {
        }
    }
}