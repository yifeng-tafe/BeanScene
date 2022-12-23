namespace BeanSceneXUnitTest
{
    public class UnitTest2
    {
        [Fact]
        
        public async void Index_Area_Test()
        {
            var options = new DbContextOptionsBuilder<ApplicationDBContext>()
                .UseInMemoryDatabase(databaseName: "Index_Area_Test")
                .Options;

            var context = new ApplicationDBContext(options);

            var c = new Area();

            c.Id = 1;
            c.Name = "Main";
            
            context.Add(c);
            context.SaveChanges();

            var controller = new AreaController(context);

            // Act
            // var result = await controller.Index() as ViewResult;
            var result = await controller.Index();
            //var model = result.ViewData.Model;

            // Assert
            var viewResult = Assert.IsType<ViewResult>(result);
            var model = Assert.IsAssignableFrom<IEnumerable<Area>>(
                viewResult.ViewData.Model);
            Assert.Equal(1, model.Count());
        }
    }
}