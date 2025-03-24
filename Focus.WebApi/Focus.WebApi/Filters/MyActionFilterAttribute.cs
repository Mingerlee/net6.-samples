using Infrastructure.Enums;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Focus.WebApi.Filters
{
    public class MyActionFilterAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext actionContext)
        {
            if (!actionContext.ModelState.IsValid)
            {
                var modelStateErrors = actionContext.ModelState
                    .Select(p => p.Value.Errors)
                    .ToList();

                var listErrorMsg = new List<string>();
                modelStateErrors.ForEach(item =>
                {
                    item.Where(p => p != null).ToList()
                        .ForEach(r =>
                        {
                            listErrorMsg.Add(r.Exception == null ? r.ErrorMessage : r.Exception.Message);
                        });
                });

                if (listErrorMsg.Count > 0)
                {
                }

                actionContext.Result = new OkObjectResult(string.Join("</br>", listErrorMsg.AsEnumerable()));
                return;
            }

            Console.WriteLine($"OnActionExecuting:{DateTime.Now:yyyyMMddHHmmssfff}");
            base.OnActionExecuting(actionContext);
        }
        public override void OnActionExecuted(ActionExecutedContext context)
        {
            switch (context.Result)
            {
                case ObjectResult objectResult:
                    context.Result =
                        new OkObjectResult(new ResultModel(objectResult.Value));
                    break;
                case OkResult okResult:
                    context.Result =
                        new OkObjectResult(new ResultModel( ResponseCode.sys_success,""));
                    break;
            }
            Console.WriteLine($"OnActionExecuted:{DateTime.Now:yyyyMMddHHmmssfff}");
            base.OnActionExecuted(context);
        }
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            Console.WriteLine($"OnResultExecuting:{DateTime.Now:yyyyMMddHHmmssfff}");
            base.OnResultExecuting(context);
        }
        public override void OnResultExecuted(ResultExecutedContext context)
        {
            Console.WriteLine($"OnResultExecuted:{DateTime.Now:yyyyMMddHHmmssfff}");
            base.OnResultExecuted(context);
        }
    }
}
