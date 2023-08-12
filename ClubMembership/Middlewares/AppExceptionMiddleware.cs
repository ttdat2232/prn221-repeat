using Application.Exceptions;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace ClubMembership.Middlewares
{
    public class AppExceptionMiddleware
    {
        private readonly RequestDelegate request;
        private readonly ILogger<AppExceptionMiddleware> logger;
        public AppExceptionMiddleware(RequestDelegate request, ILogger<AppExceptionMiddleware> logger)
        {
            this.request = request;
            this.logger = logger;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await request.Invoke(context);
            }
            catch (Exception ex)
            {
                logger.LogError(ex.Message);
                ITempDataDictionaryFactory factory = context.RequestServices.GetService(typeof(ITempDataDictionaryFactory)) as ITempDataDictionaryFactory ?? throw new ArgumentNullException ("Null for type of ITempDataDictionFactory");
                ITempDataDictionary tempData = factory.GetTempData(context);
                context.Response.Redirect(context.Request.Path);
                switch (ex)
                {
                    case AppException: 
                        tempData["Error"] = ex.Message;
                        break;
                    case NotFoundException:
                        tempData["Error"] = ex.Message.Substring(ex.Message.IndexOf(":") + 1);
                        context.Response.Redirect("./");
                        break;
                    default:
                        tempData["Error"] = "Errored occurred";
                        break;
                }
                tempData.Save();
            }
        }
    }
}
