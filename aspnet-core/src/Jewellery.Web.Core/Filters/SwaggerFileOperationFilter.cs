using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Jewellery.Filters
{
    public class FormFileDescriptorAttribute : Attribute
    {
        public FormFileDescriptorAttribute(string title, string description, bool required, int maxLength)
        {
            Title = title;
            Description = description;
            Required = required;
            MaxLength = maxLength;
        }

        public string Title { get; set; }

        public string Description { get; set; }

        public int MaxLength { get; set; }

        public bool Required { get; set; }
    }

    public class SwaggerFileOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var fileParams = context.MethodInfo.GetParameters().Where(p => p.ParameterType.FullName?.Equals(typeof(Microsoft.AspNetCore.Http.IFormFile).FullName) == true);

            if (fileParams.Any() && fileParams.Count() == 1)
            {
                var title = "The file to be uploaded";
                var description = "The file to be uploaded";
                int? maxLength = 5_242_880;
                bool required = true;

                var descriptionAttribute = fileParams.First().CustomAttributes.FirstOrDefault(a => a.AttributeType == typeof(FormFileDescriptorAttribute));
                if (descriptionAttribute?.ConstructorArguments.Count > 3)
                {
                    title = descriptionAttribute.ConstructorArguments[0].Value.ToString();
                    description = descriptionAttribute.ConstructorArguments[1].Value.ToString();
                    required = (bool)descriptionAttribute.ConstructorArguments[2].Value;
                    maxLength = (int)descriptionAttribute.ConstructorArguments[3].Value;
                }

                var uploadFileMediaType = new OpenApiMediaType()
                {
                    Schema = new OpenApiSchema()
                    {
                        Type = "object",
                        Properties =
            {
              [fileParams.First().Name] = new OpenApiSchema()
              {
                  Description = description,
                  Type = "file",
                  Format = "binary",
                  Title = title,
                  MaxLength = maxLength
              }
            }
                    }
                };

                if (required)
                {
                    uploadFileMediaType.Schema.Required = new HashSet<string>() { fileParams.First().Name };
                }

                operation.RequestBody = new OpenApiRequestBody
                {
                    Content = { ["multipart/form-data"] = uploadFileMediaType }
                };
            }
        }
    }
}
