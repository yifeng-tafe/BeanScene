namespace BeanSceneXUnitTest
{
    public class UnitTest1
    {
        [Fact]
        public async void Index_Categroy_Test()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "BeanScene")
                .Options;

            var context = new ApplicationDBContext(options);

            var c = new Category();

            c.Name = "Lunch";
            c.Id = 10;

            context.Add(c);
            context.SaveChanges();

            var controller = new CategoryController(context);

            // Act
            // var result = await controller.Index() as ViewResult;
            var result = await controller.Index();
            //var model = result.ViewData.Model;

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Category>>(
                viewResult.ViewData.Model);
            Assert.Equal(1, model.Count());
        }

        public async void Index_Category_Test()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "Index_Category_Test")
                .Options;

            var context = new ApplicationDBContext(options);

            var c = new Category();

            c.Id = 10;
            c.Name = "Lunch";
            

            context.Add(c);
            context.SaveChanges();

            var controller = new CategoryController(context);

            // Act
            // var result = await controller.Index() as ViewResult;
            var result = await controller.Index();
            //var model = result.ViewData.Model;

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Category>>(
                viewResult.ViewData.Model);
            Assert.Equal(1, model.Count());
        }
    }
}