using System.Web.Mvc;
using System.Web.Routing;

namespace titi
{
    public class RouteConfig
    {
        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute(
                name: "Chi tiết sản phẩm",
                url: "san-pham/{category}/{sanPhamURL}-{productId}",
                defaults: new { controller = "Products", action = "ProductDetails", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Chi tiết giỏ hàng",
                url: "cart/cartdetails",
                defaults: new { controller = "Products", action = "CartDetails", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "đăng nhập mua hàng",
                url: "cart/CheckOut",
                defaults: new { controller = "Products", action = "CheckOut", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "địa chỉ giao hàng",
                url: "cart/Shipping",
                defaults: new { controller = "Products", action = "Shipping", id = UrlParameter.Optional }
            );
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional }
            );
        }
    }
}
