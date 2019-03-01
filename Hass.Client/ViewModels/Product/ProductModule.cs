using System;
using System.Collections.Generic;
using System.Text;

namespace Hass.Client.ViewModels.Product
{
    public class ProductModule: AppModule<ProductGrouping>
    {
        public ProductModule()
        {
            Title = "Products";
            AddSection(new ProductGrouping() { Title = "Security" });
            AddSection(new ProductGrouping() { Title = "Switches" });
        }
    }

}
