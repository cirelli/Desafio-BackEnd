using System.Collections;
using Domain.ServiceResults;
using Microsoft.AspNetCore.Mvc;
using Tests.Overrides.Api;

namespace Tests.Api
{
    public class BaseControllerTest
    {
        [ClassData(typeof(HandleServiceResult_Data))]
        [Theory]
        public void HandleServiceResult(IServiceResult resultToTest, Type expected)
        {
            var controller = new BaseControllerOverride();
            var result = controller.HandleServiceResult(resultToTest);

            Assert.Equal(expected, result.GetType());
        }
    }

    class HandleServiceResult_Data : IEnumerable<object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return [new SuccessServiceResult<string>(""), typeof(OkObjectResult)];
            yield return [new SuccessServiceResult(), typeof(NoContentResult)];
            yield return [(ServiceResult<string>)new SuccessServiceResult(), typeof(NoContentResult)];
            yield return [new ForbiddenServiceResult(), typeof(ForbidResult)];
            yield return [(ServiceResult<string>)new ForbiddenServiceResult(), typeof(ForbidResult)];
            yield return [new InvalidServiceResult(""), typeof(BadRequestObjectResult)];
            yield return [(ServiceResult<string>)new InvalidServiceResult(""), typeof(BadRequestObjectResult)];
            yield return [new ValidationErrorServiceResult("", ""), typeof(BadRequestObjectResult)];
            yield return [(ServiceResult<string>)new ValidationErrorServiceResult("", ""), typeof(BadRequestObjectResult)];
            yield return [new ConflictServiceResult(""), typeof(ConflictObjectResult)];
            yield return [(ServiceResult<string>)new ConflictServiceResult(""), typeof(ConflictObjectResult)];
            yield return [new NotFoundServiceResult(""), typeof(NotFoundObjectResult)];
            yield return [(ServiceResult<string>)new NotFoundServiceResult(""), typeof(NotFoundObjectResult)];
            yield return [new UnauthorizedServiceResult(), typeof(UnauthorizedResult)];
            yield return [(ServiceResult<string>)new UnauthorizedServiceResult(), typeof(UnauthorizedResult)];
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
