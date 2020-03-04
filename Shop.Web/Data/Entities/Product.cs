using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Web.Data.Entities
{
    public class Product : IEntity
    {
		public int Id { get; set; }

		[MaxLength(50, ErrorMessage ="El campo {0} unicamente puede  almaxcena {1} caracteres")]
		[Required]
		public string Name { get; set; }

		[DisplayFormat(DataFormatString = "{0:C2}", ApplyFormatInEditMode = false)]
		public decimal Price { get; set; }

		[Display(Name = "Image")]
		public string ImageUrl { get; set; }

		[Display(Name = "Last Purchase")]
		public DateTime? LastPurchase { get; set; }

		[Display(Name = "Last Sale")]
		public DateTime? LastSale { get; set; }

		[Display(Name = "Is Availabe?")]
		public bool IsAvailabe { get; set; }

		[DisplayFormat(DataFormatString = "{0:N2}", ApplyFormatInEditMode = false)]
		public double Stock { get; set; }

		// Crear una relacion entre la tabla Productos con la tabla User
		public User User { get; set; }

		public string ImageFullPath
		{
			get
			{
				if (string.IsNullOrEmpty(this.ImageUrl))
				{
					return null;
				}
				//return $"https://shopzulu.azurewebsites.net{this.ImageUrl.Substring(1)}";
				return $"https://shopvale.azurewebsites.net{this.ImageUrl.Substring(1)}";
				// Interpolacion 
			}
		}

//		https://shopvale.azurewebsites.net/Products
//      https://shopvale.azurewebsites.net/Api/Products



	}
}
