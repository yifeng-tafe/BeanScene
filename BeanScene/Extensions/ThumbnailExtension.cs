using BeanScene.Areas.Identity.Data;
using BeanScene.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace BeanScene.Extensions
{
    public static class ThumbnailExtension
    {
        public static IEnumerable<ThumbnailModel> GetFoodThumbnail(this List<ThumbnailModel>thumbnails, ApplicationDBContext db = null, string search = null)
        {
            try
            {
                thumbnails = (from b in db.Food select new ThumbnailModel
                {
                    FoodId = b.Id,
                    Name = b.Name,
                    Description = b.Description,
                    ImageUrl = b.ImageURL,
                    Price = b.Price,
                }).ToList();

                if (search != null)
                {
                    return thumbnails.Where(t => t.Name.ToLower().Contains(search.ToLower())).OrderBy(t => t.Name);
                }
            }
            catch (Exception ex)
            {

            }

            return thumbnails.OrderBy(t => t.Name);  
                              
                              
                         
            
        }
    }
}
