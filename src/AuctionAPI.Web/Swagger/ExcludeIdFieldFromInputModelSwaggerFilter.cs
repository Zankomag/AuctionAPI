using System;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace AuctionAPI.Web.Swagger {
	
	public class ExcludeIdFieldFromInputModelSwaggerFilter : ISchemaFilter {

		/// <inheritdoc />
		public void Apply(OpenApiSchema schema, SchemaFilterContext context) {
			if(schema?.Properties == null
				|| context?.Type == null
				|| !context.Type.Name.EndsWith("InputModel", StringComparison.Ordinal)) {
				
				return;
			}

			schema.Properties.Remove("id");
		}
	}

}