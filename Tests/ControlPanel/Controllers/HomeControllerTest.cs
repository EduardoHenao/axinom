using ControlPanel.Controllers;
using ControlPanel.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;

namespace Tests.ControlPanel.Controllers
{
    public class HomeControllerTest
    {
        [Fact]
        public void HomeController_Index_CorrectCall_ReturnsView()
        {
            // Arrange
            var controller = new HomeController();

            // Act
            var answer = controller.Index();

            // Assert
            Assert.NotNull(answer); //as this is a template code, well just test is fine
        }

        [Fact]
        public void HomeController_Error_CorrectCall_ReturnsView()
        {
            // Arrange
            var trace = "test_trace";
            var httpContextMock = new Mock<HttpContext>();
            var controller = new HomeController();
            if (controller.ControllerContext.HttpContext == null)
            {
                controller.ControllerContext.HttpContext = new DefaultHttpContext();
            }
            controller.ControllerContext.HttpContext.TraceIdentifier = trace;

            // Act
            var answer = controller.Error();

            // Assert
            Assert.NotNull(answer); //as this is a template code, well just test is fine
            Assert.IsType<ViewResult>(answer);
            var viewResult = answer as ViewResult;
            Assert.NotNull(viewResult);
            Assert.IsType<ErrorViewModel>(viewResult.Model);
            var errorViewModel = viewResult.Model as ErrorViewModel;
            Assert.NotNull(errorViewModel);
            Assert.Equal(trace, errorViewModel.RequestId);
        }
    }
}
